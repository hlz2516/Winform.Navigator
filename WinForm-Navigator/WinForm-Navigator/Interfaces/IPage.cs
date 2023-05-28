namespace Navigators.Interfaces
{
    public interface IPage
    {
        /// <summary>
        /// Route path
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// Is cached
        /// </summary>
        bool Cached { get; set; }
        /// <summary>
        /// Page permissions(Authority)
        /// </summary>
        Authority Authority { get; set; }
        /// <summary>
        /// This method will trigger when switching to another page
        /// </summary>
        void Pause();
        /// <summary>
        /// This method will trigger when switching back to the page and setting the cache
        /// </summary>
        void Restore();
        /// <summary>
        /// This method will trigger when switching back to the page and setting no caching
        /// </summary>
        void Reset();
    }
}
