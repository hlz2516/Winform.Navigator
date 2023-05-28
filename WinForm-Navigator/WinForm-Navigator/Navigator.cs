using Navigators.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Navigators
{
    public partial class Navigator : Control
    {
        public delegate bool PageChangedEventHandler(IPage oldPage, IPage newPage);
        public event PageChangedEventHandler PageChanged;
        public event PageChangedEventHandler PageBeforeChanged;
        public event EventHandler DefaultPageFirstShown;
        public event EventHandler<AuthorityMismatchedEventArgs>? AuthorityMismatched;
        /// <summary>
        /// 页面容器
        /// </summary>
        private ScrollableControl? container;
        /// <summary>
        /// 存储所有页面实例，方便查找
        /// </summary>
        private List<IPage> pageList;
        /// <summary>
        /// 页面实例栈，用于记录跳转历史
        /// </summary>
        private Stack<IPage> pageHistory;
        /// <summary>
        /// 保存被移出栈的页面实例的栈
        /// </summary>
        private Stack<IPage> popedPages;
        /// <summary>
        /// The permissions owned by the user role/identity of the current application
        /// </summary>
        public static Authority Role { get; set; }
        /// <summary>
        /// Enable permission verification
        /// </summary>
        public static bool EnableAuthority { get; set; }

        public Navigator()
        {
            InitializeComponent();
            Visible = false;
            pageList = new List<IPage>();
            pageHistory = new Stack<IPage>();
            popedPages = new Stack<IPage>();
        }

        private void Navigator_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null && Parent is ScrollableControl)
            {
                container = Parent as ScrollableControl;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var gh = e.Graphics;
            gh.DrawImage(Properties.Resources.地图导航_35, 2, 2, Width - 4, Height - 4);
            Pen pen = new Pen(Color.Black, 2);
            gh.DrawEllipse(pen, 2, 2, Width - 4, Height - 4);
        }

        /// <summary>
        /// Set page container.
        /// </summary>
        /// <param name="container">the container control</param>
        public void SetContainer(ScrollableControl container)
        {
            this.container = container;
        }

        /// <summary>
        /// Register page classes. After successful registration, the instance of this page 
        /// should be managed by Navigator in principle.
        /// </summary>
        /// <typeparam name="T">The class used as page.T must be a Control and inherited the IPage interface</typeparam>
        /// <param name="defaultPage">is the default page(Does the page automatically display when the container is first displayed)</param>
        /// <returns>Successfully registered returns true, otherwise returns false</returns>
        public bool RegisterPage<T>(bool defaultPage = false) where T : Control, new()
        {
            //pageType必须既是IPage接口类型又是Control类型，同时必须具有无参构造
            Type pageType = typeof(T);
            var x = pageType.GetInterface("IPage");
            if (x != null)
            {
                var page = new T() as IPage;
                pageList.Add(page);
                if (defaultPage)
                {
                    pageHistory.Clear();
                    pageHistory.Push(page);
                    container!.VisibleChanged += (sender, e) =>
                    {
                        Flush();
                        DefaultPageFirstShown?.Invoke(sender, e);
                    };
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return the specified registered page instance
        /// </summary>
        /// <typeparam name="T">the page class type</typeparam>
        /// <returns>Page instance, return null if the page does not exist</returns>
        public IPage GetPage<T>() where T : Control, new()
        {
            foreach (IPage page in pageList)
            {
                if (page is T)
                {
                    return page;
                }
            }
            return null;
        }

        /// <summary>
        /// Navigate to the page of the specified page class
        /// </summary>
        /// <typeparam name="T">the page class type</typeparam>
        /// <param name="paramMap">Page parameters included during navigate</param>
        public void NavigateTo<T>(Dictionary<string, object>? paramMap = null) where T : Control, new()
        {
            foreach (var page in pageList)
            {
                if (page is T)
                {
                    if (EnableAuthority && (Role & page.Authority) == 0)
                    {
                        var eveArgs = new AuthorityMismatchedEventArgs();
                        eveArgs.MyAuthority = Role;
                        eveArgs.PageRequired = page.Authority;
                        AuthorityMismatched?.Invoke(null, eveArgs);
                        return;
                    }

                    bool? flag = PageBeforeChanged?.Invoke(pageHistory.ElementAtOrDefault(0), page);
                    if (flag != null && flag == false)
                    {
                        return;
                    }

                    if (pageHistory.Count > 0)
                    {
                        if (pageHistory.First() == page)
                        {
                            break;
                        }
                        pageHistory.First().Pause();
                    }
                    SetPageParams(page, paramMap);
                    pageHistory.Push(page);
                    Flush();

                    break;
                }
            }
        }

        /// <summary>
        /// Navigate to the specified page instance
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="paramMap">Page parameters included during navigate</param>
        public void NavigateTo(IPage page, Dictionary<string, object>? paramMap = null)
        {
            if (pageList.Contains(page))
            {
                if (EnableAuthority && (Role & page.Authority) == 0)
                {
                    var eveArgs = new AuthorityMismatchedEventArgs();
                    eveArgs.MyAuthority = Role;
                    eveArgs.PageRequired = page.Authority;
                    AuthorityMismatched?.Invoke(null, eveArgs);
                    return;
                }

                bool? flag = PageBeforeChanged?.Invoke(pageHistory.ElementAtOrDefault(0), page);
                if (flag != null && flag == false)
                {
                    return;
                }

                if (pageHistory.Count > 0)
                {
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
        }

        /// <summary>
        /// Return to the previous page
        /// </summary>
        /// <returns>The page before performing this operation(Old page)</returns>
        public IPage Back()
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
        public void GoBackBack()
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

        private void SetPageParams(IPage page, Dictionary<string, object>? paramMap)
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
        public void Flush()
        {
            if (container == null)
            {
                return;
            }
            if (container.Controls.Contains(this))
            {
                container.Controls.Remove(this);
            }

            if (container.Controls.Count > 0)
            {
                if (pageHistory.Count > 0)
                {
                    //判定容器内的页面是否与栈顶一致
                    if (container.Controls[0] != pageHistory.First())
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
                    container.Controls.Clear();
                    container.Update();
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

        private void AddPage(Control pageCtrl)
        {
            Control oldPage = container.Controls.Count > 0 ? container!.Controls[0] : null;
            container!.Controls.Clear();
            if (pageCtrl is Form)
            {
                Form form = (Form)pageCtrl;
                form.ControlBox = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.TopLevel = false;
            }
            pageCtrl.Dock = DockStyle.Fill;
            container.Controls.Add(pageCtrl);
            pageCtrl.Show();
            container.Update();
            PageChanged?.Invoke((IPage)oldPage, (IPage)pageCtrl);
        }

        /// <summary>
        /// 设置初始页面
        /// </summary>
        /// <param name="page">页面实例</param>
        /// <param name="paramsMap">页面参数</param>
        //public void SetDefaultPage(IPage page,Dictionary<string,object>? paramsMap = null)
        //{
        //    if (pageHistory.Count > 0)
        //    {
        //        pageHistory.Clear();
        //    }
        //    if (popedPages.Count > 0)
        //    {
        //        popedPages.Clear();
        //    }

        //    NavigateTo(page,paramsMap);
        //}

        /// <summary>
        /// Set role
        /// </summary>
        public static void SetRole(Authority role)
        {
            Role = role;
        }
        /// <summary>
        /// Set permissions for the specified page
        /// </summary>
        /// <typeparam name="T">the page class type</typeparam>
        /// <param name="authority">Page permissions</param>
        public void SetAuthority<T>(Authority authority) where T : Control, new()
        {
            foreach (var page in pageList)
            {
                if (page is T)
                {
                    page.Authority = authority;
                    break;
                }
            }
        }
        /// <summary>
        /// Clear the page display in the container, and then destroy all page instances.
        /// </summary>
        public void Destroy()
        {
            if (!container.IsDisposed)
            {
                container.Controls.Clear();
                this.Dispose();
            }
        }
    }
}
