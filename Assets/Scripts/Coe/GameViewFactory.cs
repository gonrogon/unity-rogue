using System;

namespace Rogue.Coe
{
    public interface IGameViewFactory
    {
        /// <summary>
        /// Creates a new instance of the view.
        /// </summary>
        /// <param name="name">Name of the view.</param>
        /// <returns>View.</returns>
        IGameView Create(string type, string name);
    }
}
