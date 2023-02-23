using System;
using GG.Mathe;

namespace Rogue.Map
{
    public class PathRequest
    {
        /// <summary>
        /// Origin.
        /// </summary>
        public Vec2i origin;

        /// <summary>
        /// Target.
        /// </summary>
        public Vec2i target;

        /// <summary>
        /// Flag indicating whether the origin is included in the path or not.
        /// </summary>
        public bool includeOrigin;

        /// <summary>
        /// Flag indicating whether the target in included in the path or not.
        /// </summary>
        public bool includeTarget;

        /// <summary>
        /// Callback.
        /// </summary>
        public PathCallback callback;

        public PathFinder.Parameters parameters = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        /// <param name="callback">Callback.</param>
        public PathRequest(Vec2i origin, Vec2i target, PathCallback callback)
            :
            this(origin, target, true, true, callback)
        {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        /// <param name="callback">Callback.</param>
        public PathRequest(Vec2i origin, Vec2i target, bool includeOrigin, bool includeTarget, PathCallback callback)
        {
            this.origin        = origin;
            this.target        = target;
            this.includeOrigin = includeOrigin;
            this.includeTarget = includeTarget;
            this.callback      = callback;
        }
    }
}
