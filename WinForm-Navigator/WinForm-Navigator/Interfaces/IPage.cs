namespace Navigators.Interfaces
{
    public interface IPage
    {
        string Path { get; set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        bool Cached { get; set; }
        /// <summary>
        /// 页面权限
        /// </summary>
        Authority Authority { get; set; }
        /// <summary>
        /// 页面跳转时如果当前页面作为被替换的页面，会调用该方法
        /// </summary>
        void Pause();
        /// <summary>
        /// 页面跳转时如果当前页面作为替换的页面并且设置了缓存，会调用该方法
        /// </summary>
        void Restore();
        /// <summary>
        /// 页面跳转时如果当前页面作为替换的页面并且设置不缓存，会调用该方法
        /// </summary>
        void Reset();
    }
}
