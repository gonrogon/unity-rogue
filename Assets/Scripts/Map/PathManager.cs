using System;
using System.Collections.Generic;
using GG.Mathe;

namespace Rogue.Map
{
    public class PathManager
    {
        /// <summary>
        /// Queue of path request.
        /// </summary>
        Queue<PathRequest> m_requests = new Queue<PathRequest>();

        /// <summary>
        /// Map.
        /// </summary>
        GameMap m_map;

        /*
        public PathManager(GameMap map)
        {
            m_map = map;
        }
        */
        public void Setup(GameMap map)
        {
            m_map = map;
        }

        public void Enqueue(PathRequest request)
        {
            m_requests.Enqueue(request);
        }

        public void Process(PathFinder finder)
        {
            if (m_requests.Count == 0)
            {
                return;
            }

            var request = m_requests.Dequeue();
            var path    = new Path2i();

            var result  = finder.Find2(request.origin, request.target, request.parameters, request.includeOrigin, request.includeTarget, path);

            request.callback?.Invoke(result, path);
        }
    }
}
