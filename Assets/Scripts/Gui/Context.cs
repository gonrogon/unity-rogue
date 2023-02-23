namespace Rogue.Gui
{
    /// <summary>
    /// Defines a context.
    /// </summary>
    public abstract class Context
    {
        /// <summary>
        /// Manager.
        /// </summary>
        public ContextManager Manager { get; private set; } = null;

        /// <summary>
        /// Sets up the context.
        /// </summary>
        /// <param name="manager">Manager of this context.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Setup(ContextManager manager)
        {
            Manager = manager;

            return OnSetup();
        }

        /// <summary>
        /// Starts the context.
        /// </summary>
        public void Start()
        {
            Manager.ActiveSettings.start?.Invoke(this);
            OnStart();
        }

        /// <summary>
        /// Finishes the context.
        /// </summary>
        public void Finish()
        {
            OnFinish();
            Manager.ActiveSettings.end?.Invoke(this);
        }

        /// <summary>
        /// Notifies the object that triggered the context.
        /// </summary>
        public void Notify()
        {
            Manager.ActiveSettings.notify?.Invoke(this);
        }

        /// <summary>
        /// Notifies the context that it has recovered the context.
        /// </summary>
        public void Focus()
        {
            OnFocus();
        }

        /// <summary>
        /// Notifies the context that it has received an action.
        /// </summary>
        /// <param name="action"></param>
        public void Action(string action)
        {
            OnAction(action);
        }

        /// <summary>
        /// Updates the context.
        /// </summary>
        /// <param name="time">Time since the last update.</param>
        /// <returns>True if the context is finished; otherwise, false.</returns>
        public bool Update(float time)
        {
            return OnUpdate(time);
        }

        #region @@@ CONTEXT IMPLEMENTATION @@@

        protected virtual bool OnSetup() { return true; }

        protected virtual void OnStart() {}

        protected virtual void OnFinish() {}

        protected virtual void OnFocus() {}

        protected virtual void OnAction(string action) {}

        protected virtual bool OnUpdate(float time) { return false; }

        #endregion
    }
}
