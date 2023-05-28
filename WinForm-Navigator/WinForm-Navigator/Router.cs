using Navigators.Attributes;
using Navigators.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Navigators
{
    public class Router
    {
        public delegate bool PageChangedEventHandler(IPage oldPage, IPage newPage);
        public static event PageChangedEventHandler PageChanged;
        public static event PageChangedEventHandler PageBeforeChanged;
        public static event EventHandler DefaultPageFirstShown;
        public static event EventHandler<AuthorityMismatchedEventArgs>? AuthorityMismatched;
        /// <summary>
        /// 存储所有页面实例的列表
        /// </summary>
        private static List<IPage> pages;
        /// <summary>
        /// 页面名称与页面的映射字典
        /// </summary>
        private static Dictionary<string, IPage> nameToPage;
        /// <summary>
        /// 页面容器
        /// </summary>
        private static ScrollableControl? _container;
        /// <summary>
        /// 页面实例栈，用于记录跳转历史
        /// </summary>
        private static Stack<IPage> pageHistory;
        /// <summary>
        /// 保存被移出栈的页面实例的栈
        /// </summary>
        private static Stack<IPage> popedPages;
        /// <summary>
        /// The permissions owned by the user role/identity of the current application
        /// </summary>
        public static Authority Role { get; set; }
        /// <summary>
        /// Enable permission verification
        /// </summary>
        public static bool EnableAuthority { get; set; }

        static Router()
        {
            pages = new List<IPage>();
            nameToPage = new Dictionary<string, IPage>();
            pageHistory = new Stack<IPage>();
            popedPages = new Stack<IPage>();
            Role = Authority.VISITOR | Authority.USER;  //默认身份
        }
        /// <summary>
        /// Load configuration from App.config.
        /// </summary>
        public static void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;

            List<Route> routes = new List<Route>();

            try
            {
                string configPath = Process.GetCurrentProcess().MainModule.FileName + ".config";
                doc.Load(configPath);
                XmlNode routesNode = doc.SelectSingleNode("//configuration/routes");

                if (routesNode.Attributes["enableAuthority"] != null)
                {
                    EnableAuthority = bool.Parse(routesNode.Attributes["enableAuthority"].Value.Trim());
                }

                XmlNodeList routeList = routesNode.SelectNodes("route");
                foreach (XmlNode routeNode in routeList)
                {
                    Route route = new Route();
                    var path = routeNode.SelectSingleNode("path");
                    if (path != null)
                    {
                        route.Path = path.InnerText.Trim();
                    }

                    var name = routeNode.SelectSingleNode("name");
                    if (name != null)
                    {
                        route.Name = name.InnerText.Trim();
                    }

                    if (routeNode.Attributes["cache"] != null)
                    {
                        route.Cached = bool.Parse(routeNode.Attributes["cache"].Value.Trim());
                    }

                    if (routeNode.Attributes["defaultPage"] != null)
                    {
                        if (!routes.Any(r=>r.IsDefault))
                        {
                            route.IsDefault = bool.Parse(routeNode.Attributes["defaultPage"].Value.Trim());
                        }
                    }

                    routes.Add(route);
                }

                Assembly asm = Assembly.GetEntryAssembly();
                var classTypes = asm.GetTypes().Where(t => t.IsClass && !t.IsAbstract);
                //遍历入口程序集所有声明为class的类型
                foreach (var cls in classTypes)
                {
#if NET40
                   bool isRoute = Attribute.GetCustomAttributes(cls).Any(c => c is RouteAttribute);
#else
                    bool isRoute = cls.GetCustomAttributes().Any(c => c is RouteAttribute);
#endif
                    if (isRoute)
                    {
#if NET40
                        RouteAttribute routeAttr = Attribute.GetCustomAttribute(cls,typeof(RouteAttribute)) as RouteAttribute;
#else
                        RouteAttribute routeAttr = cls.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
#endif
                        foreach (var r in routes)
                        {
                            if (r.Path == routeAttr.RoutePath)
                            {
                                //调用该类的构造函数并将实例存入pages列表
                                var page = asm.CreateInstance(cls.FullName, true);
                                if (!(page is IPage))  //如果该实例不继承IPage接口，则不存入
                                {
                                    break;
                                }
                                IPage page1 = (IPage)page;
                                page1.Path = r.Path;
                                page1.Cached = r.Cached;
                                pages.Add(page1);

                                if (r.IsDefault)
                                {
                                    pageHistory.Clear();
                                    pageHistory.Push(page1);
                                }

                                //判断xml表有没有设置该路由的name，如果有则存入nameToPage字典
                                if (!string.IsNullOrEmpty(r.Name))
                                {
                                    nameToPage[r.Name] = page1;
                                }
                                //再判断该类属性上是否有设置该路由的name，如果有则存入或更新nameToPage字典
                                if (!string.IsNullOrEmpty(routeAttr.RouteName))
                                {
                                    nameToPage[routeAttr.RouteName] = page1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Set Page Container
        /// </summary>
        /// <param name="container"> The Container Control</param>
        public static void SetContainer(ScrollableControl container)
        {
            if (_container != null)
            {
                _container.VisibleChanged -= ShowDefaultPage;
            }
            _container = container;
            _container.VisibleChanged += ShowDefaultPage;
        }

        private static void ShowDefaultPage(object? sender,EventArgs e)
        {
            if (pageHistory.Count == 1)
            {
                Flush();
                DefaultPageFirstShown?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Route to the specified page
        /// </summary>
        /// <param name="path">the route path or name</param>
        /// <param name="paramMap">Page parameters included during route</param>
        public static void RouteTo(string path, Dictionary<string, object>? paramMap = null)
        {
            //在pages中寻找符合的page，若没有，再去nameToPage中找
            IPage page = pages.Where(p => p.Path == path).FirstOrDefault();
            if (page == default(IPage))
            {
                if (nameToPage.ContainsKey(path))
                {
                    page = nameToPage[path];
                }
            }

            if (page == null)
            {
                return;
            }

            if (EnableAuthority && (Role & page.Authority) == 0)
            {
                var eveArgs = new AuthorityMismatchedEventArgs();
                eveArgs.MyAuthority = Role;
                eveArgs.PageRequired = page.Authority;
                AuthorityMismatched?.Invoke(null, eveArgs);
                return;
            }

            if (pageHistory.Count > 0)
            {
                bool? flag = PageBeforeChanged?.Invoke(pageHistory.ElementAtOrDefault(0), page);
                if (flag != null && flag == false)
                {
                    return;
                }

                if (pageHistory.First() == page)
                {
                    return;
                }
                pageHistory.First().Pause();
            }
            SetPageParams(page, paramMap);
            pageHistory.Push(page);
            Flush();
        }

        private static void SetPageParams(IPage page, Dictionary<string, object>? paramMap)
        {
            if (paramMap == null)
            {
                return;
            }
            Type t = page.GetType();
            foreach (var key in paramMap.Keys)
            {
                var fieldInfo1 = t.GetField(key, BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo1 != null)
                {
                    fieldInfo1.SetValue(page, paramMap[key]);
                    continue;
                }
                var fieldInfo2 = t.GetField(key);
                if (fieldInfo2 != null)
                {
                    fieldInfo2.SetValue(page, paramMap[key]);
                    continue;
                }
                var propInfo = t.GetProperty(key, paramMap[key].GetType());
                if (propInfo != null)
                {
                    propInfo.SetValue(page, paramMap[key],null);
                }
            }
        }

        /// <summary>
        /// Refresh UI(only including the interior of the container).
        /// </summary>
        public static void Flush()
        {
            if (_container == null)
            {
                return;
            }

            if (_container.Controls.Count > 0)
            {
                if (pageHistory.Count > 0)
                {
                    //判定容器内的页面是否与栈顶一致
                    if (_container.Controls[0] != pageHistory.First())
                    {
                        var newPage = pageHistory.First();
                        if (newPage.Cached)
                        {
                            newPage.Restore();
                        }
                        else
                        {
                            newPage.Reset();
                        }

                        AddPage(newPage as Control);
                    }
                }
                else
                {
                    _container.Controls.Clear();
                    _container.Update();
                }
            }
            else
            {
                if (pageHistory.Count > 0)
                {
                    var newPage = pageHistory.First();
                    if (newPage.Cached)
                    {
                        newPage.Restore();
                    }
                    else
                    {
                        newPage.Reset();
                    }
                    AddPage(newPage as Control);
                }
            }
        }

        private static void AddPage(Control pageCtrl)
        {
            Control oldPage = _container.Controls.Count > 0 ? _container!.Controls[0] : null;
            _container!.Controls.Clear();
            if (pageCtrl is Form)
            {
                Form form = (Form)pageCtrl;
                form.ControlBox = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.TopLevel = false;
            }
            pageCtrl.Dock = DockStyle.Fill;
            _container.Controls.Add(pageCtrl);
            pageCtrl.Show();
            _container.Update();
            PageChanged?.Invoke((IPage)oldPage, (IPage)pageCtrl);
        }

        /// <summary>
        /// Returns a page instance declared in the configuration
        /// </summary>
        /// <param name="path">the page(route) path or name</param>
        /// <returns>page instance</returns>
        public static IPage GetPage(string path)
        {
            //在pages中寻找符合的page，若没有，再去nameToPage中找
            IPage page = pages.FirstOrDefault(p => p.Path == path);
            if (page == default(IPage))
            {
                if (nameToPage.ContainsKey(path))
                {
                    page = nameToPage[path];
                }
            }
            return page;
        }

        /// <summary>
        /// Return to the previous page.
        /// </summary>
        /// <returns>The page before performing this operation(Old page)</returns>
        public static IPage Back()
        {
            if (pageHistory.Count > 1)
            {
                var oldPage = pageHistory.Pop();
                oldPage.Pause();
                popedPages.Push(oldPage);
                Flush();
                return oldPage;
            }
            return null;
        }

        /// <summary>
        /// Cancel Return to Previous Page
        /// </summary>
        public static void GoBackBack()
        {
            if (popedPages.Count > 0)
            {
                var page = popedPages.Pop();
                if (pageHistory.Count > 0)
                {
                    pageHistory.First().Pause();
                }

                pageHistory.Push(page);
                Flush();
            }
        }
        /// <summary>
        /// Clear the page display in the container, and then destroy all page instances.
        /// </summary>
        public static void Destroy()
        {
            if (!_container.IsDisposed)
            {
                _container.Controls.Clear();
            }

            pages.Clear();
            nameToPage.Clear();
            pageHistory.Clear();
            popedPages.Clear();
        }
    }

    public struct Route
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Cached { get; set; }
        public bool IsDefault { get; set; }
    }
}
