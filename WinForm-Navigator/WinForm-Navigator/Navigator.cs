using Navigator.Interfaces;
using System.Reflection;

namespace Navigator
{
    public partial class Navigator : Control
    {
        public delegate void PageChangedEventHandler(Control oldPage, Control newPage);
        public event PageChangedEventHandler PageChanged;
        public EventHandler<AuthorityMismatchedEventArgs>? AuthorityMismatched;
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
        /// 当前程序的使用者角色/身份拥有的权限
        /// </summary>
        public static Authority Role { get; set; }

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
            gh.DrawImage(Properties.Resources.地图导航_35,2,2,Width-4,Height-4);
            Pen pen = new Pen(Color.Black, 2);
            gh.DrawEllipse(pen,2, 2, Width-4, Height-4);
        }

        /// <summary>
        /// 设置页面容器
        /// </summary>
        /// <param name="container">容器</param>
        public void SetContainer(ScrollableControl container)
        {
            this.container = container;
        }

        /// <summary>
        /// 注册页面类。注册成功后该页面类的实例原则上交由页面管理器管理
        /// </summary>
        /// <typeparam name="T">页面类</typeparam>
        /// <param name="defaultPage">是否为初始页面</param>
        /// <returns>注册成功返回true，否则返回false</returns>
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
                    container!.VisibleChanged += (sender, e) => { Flush(); };
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回一个注册过的页面实例
        /// </summary>
        /// <typeparam name="T">页面类</typeparam>
        /// <returns>页面实例，若不存在该页面则返回Null</returns>
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
        /// 导航至指定页面类的页面
        /// </summary>
        /// <typeparam name="T">页面类</typeparam>
        /// <param name="paramMap">跳转时附带的页面参数</param>
        public void NavigateTo<T>(Dictionary<string, object>? paramMap = null) where T : Control, new()
        {
            foreach (var page in pageList)
            {
                if (page is T)
                {
                    if ((Role & page.Authority) > 0)
                    {
                        if (pageHistory.Count > 0)
                        {
                            if (pageHistory.First() == page)
                            {
                                break;
                            }
                            pageHistory.First().Pause();
                        }
                        SetPageParams(page,paramMap);
                        pageHistory.Push(page);
                        Flush();
                    }
                    else
                    {
                        var eveArgs = new AuthorityMismatchedEventArgs();
                        eveArgs.MyAuthority = Role;
                        eveArgs.PageRequired = page.Authority;
                        AuthorityMismatched?.Invoke(null,eveArgs);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 导航至指定页面实例
        /// </summary>
        /// <param name="page">页面实例</param>
        /// <param name="paramMap">跳转时附带的页面参数</param>
        public void NavigateTo(IPage page, Dictionary<string, object>? paramMap = null)
        {
            if (pageList.Contains(page))
            {
                if ((Role & page.Authority) > 0)
                {
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
                else
                {
                    var eveArgs = new AuthorityMismatchedEventArgs();
                    eveArgs.MyAuthority = Role;
                    eveArgs.PageRequired = page.Authority;
                    AuthorityMismatched?.Invoke(null, eveArgs);
                }
            }
        }

        /// <summary>
        /// 返回到上一个页面
        /// </summary>
        /// <returns>执行返回操作前的页面</returns>
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
        /// 撤销返回上一个页面
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
                    propInfo.SetValue(page, paramMap[key]);
                }
            }
        }

        /// <summary>
        /// 刷新页面
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
            PageChanged?.Invoke(oldPage, pageCtrl);
        }

        /// <summary>
        /// 设置初始页面
        /// </summary>
        /// <param name="page">页面实例</param>
        /// <param name="paramsMap">页面参数</param>
        public void SetDefaultPage(IPage page,Dictionary<string,object>? paramsMap = null)
        {
            if (pageHistory.Count > 0)
            {
                pageHistory.Clear();
            }
            if (popedPages.Count > 0)
            {
                popedPages.Clear();
            }

            NavigateTo(page,paramsMap);
        }

        public static void SetRole(Authority role)
        {
            Role = role;
        }

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
