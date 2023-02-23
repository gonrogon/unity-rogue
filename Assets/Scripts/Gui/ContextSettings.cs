namespace Rogue.Gui
{
    /// <summary>
    /// Define the settings to configure a context activation.
    /// </summary>
    public struct ContextSettings
    {
        /// <summary>
        /// Empty settings.
        /// </summary>
        public static readonly ContextSettings Empty = new ContextSettings(null, null, null);

        /// <summary>
        /// Delegate to call when the context is started.
        /// </summary>
        public ContextCallback start;

        /// <summary>
        /// Delegate to call when the context is finished.
        /// </summary>
        public ContextCallback end;

        /// <summary>
        /// Delegate to call when the context notifies an event.
        /// </summary>
        public ContextCallback notify;

        public ContextSettings(ContextCallback start, ContextCallback end, ContextCallback notify)
        {
            this.start  = start;
            this.end    = end;
            this.notify = notify;
        }
    }
}
