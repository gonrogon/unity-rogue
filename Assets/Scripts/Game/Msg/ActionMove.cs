using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform an action.
    /// </summary>
    public class ActionMove : Action<ActionMove>
    {
        /// <summary>
        /// Direction of the movement.
        /// </summary>
        public Vec2i dir = Vec2i.Zero;

        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionMove() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dir">Direction.</param>
        public ActionMove(Vec2i dir)
        {
            this.dir = dir;
            Normalize();
        }

        /// <summary>
        /// Normalize the each component of the direction to be in the range [-1, 1].
        /// </summary>
        public void Normalize()
        {
            dir.x = Mathf.Clamp(dir.x, -1, 1);
            dir.y = Mathf.Clamp(dir.y, -1, 1);
        }
    }
}