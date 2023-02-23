using System;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Core.Collections;
using GG.Mathe;

namespace Rogue.Map
{
    public class PathFinder
    {
        public class Parameters
        {
            /// <summary>
            /// Function to check whether a coordinate is solid or not.
            /// </summary>
            public Func<GameMap, Vec2i, bool> solid;

            /// <summary>
            /// Function to calculate the cost to move between two coordinates.
            /// </summary>
            public Func<GameMap, Vec2i, Vec2i, float> cost;

            /// <summary>
            /// Function to calculate the heuristic between two coordinates.
            /// </summary>
            public Func<GameMap, Vec2i, Vec2i, float> heuristic;

            /// <summary>
            /// Function to calculate the heuristic between two coordinates.
            /// </summary>
            public Func<GameMap, float, float, float> priority;

            public void Set(Parameters parameters)
            {
                if (parameters == null)
                {
                    Clear();
                }
                else
                {
                    solid     = parameters.solid;
                    cost      = parameters.cost;
                    heuristic = parameters.heuristic;
                    priority  = parameters.priority;
                }
            }

            public void Clear()
            {
                solid     = null;
                cost      = null;
                heuristic = null;
                priority  = null;
            }
        }

        /// <summary>
        /// Define a node.
        /// </summary>
        private struct Node
        {
            /// <summary>
            /// Constant with the values for the nodes at the start of the path finding algorithm.
            /// </summary>
            public static readonly Node Initial = new Node{open = false, source = -1, gCost = 0, hCost = 0};

            /// <summary>
            /// Flag indicating if the node is in the open list.
            /// </summary>
            public bool open;

            /// <summary>
            /// Index of the source node.
            /// </summary>
            public int source;

            /// <summary>
            /// Goal cost.
            /// </summary>
            public float gCost;

            /// <summary>
            /// Heuristic cost.
            /// </summary>
            public float hCost;
        }

        /// <summary>
        /// Horizontal or vertical distance factor for the diagonal distance.
        /// </summary>
        private static readonly float D1 = 1.0f;

        /// <summary>
        /// Slode factor for the diagonal distance.
        /// </summary>
        private static readonly float D2 = Mathf.Sqrt(D1);

        /// <summary>
        /// Array with the differences to calculate the neighbour nodes.
        /// </summary>
        private static readonly int[,] Neighbour8 = new int[8, 2]
        {
            {-1, -1}, {-1, 0}, {-1, 1}, {0, 1}, {1, 1}, {1, 0}, {1, -1}, {0, -1}
        };

        private GameMap m_map = null;

        //private Node[] m_nodes = null;

        private Grid<Node> m_nodes = null;

        //private List<int> m_open = null;

        private List<Vec2i> m_open = null;

        private Parameters m_parameters = new Parameters();
        /*
        public PathFinder(GameMap map)
        {
            //m_nodes = new Node[map.Width * map.Height];
            //m_open  = new List<int>();
            //m_map   = map;
        }
        */
        public void Setup(GameMap map)
        {
            m_nodes = new Grid<Node>(map.Rows, map.Cols);
            m_open  = new List<Vec2i>();
            m_map   = map;
        }

        public bool Find2(Vec2i origin, Vec2i target, Parameters parameters, bool includeOrigin, bool includeTarget, Path2i path)
        {
            // TODO: What should be returned when origin and target are the same position?.

            ClearFrontier();
            ClearNodes();
            // Set the paremeters and the default functions if no one was supplied.
            m_parameters.Set(parameters);
            m_parameters.solid     ??= CalculateSolid;
            m_parameters.cost      ??= CalculateCost;
            m_parameters.heuristic ??= CalculateHeuristic;
            m_parameters.priority  ??= CalculatePriority;

            //int originIdx = m_nodes2.GetIndex(origin);
            //int targetIdx = m_nodes2.GetIndex(target);

            ref Node originNode = ref m_nodes.GetRef(origin);
            ref Node targetNode = ref m_nodes.GetRef(target);

            originNode.open   =  true;
            originNode.source = -1;
            originNode.gCost  =  0;
            originNode.hCost  =  0;

            //m_open.Add(originIdx);
            m_open.Add(origin);

            while (m_open.Count > 0)
            {
                float max   = float.MaxValue;
                int   found = 0;
                // Find the node with less cost.
                // TODO: Replace this with a heap.
                for (int i = 0; i < m_open.Count; i++)
                {
                    //var totalCost = CalculateTotalCost(m_nodes[m_open[i]].gCost, m_nodes[m_open[i]].hCost);
                    //var totalCost = m_parameters.priority(m_map, m_nodes[m_open[i]].gCost, m_nodes[m_open[i]].hCost);
                    var totalCost = m_parameters.priority(m_map, m_nodes.GetRef(m_open[i]).gCost, m_nodes.GetRef(m_open[i]).hCost);

                    if (totalCost < max)
                    {
                        found = i;
                        max   = totalCost;
                    }
                }
                // Swap with the last one and remove.
                //int current   = m_open[found];
                Vec2i current = m_open[found];
                m_open[found] = m_open[m_open.Count - 1];
                m_open.RemoveAt(m_open.Count - 1);
                // Check if the target has been reached.
                if (current == target)
                {
                    break;
                }
                // Mark the node as closed.
                //m_nodes2[current].open = false;
                m_nodes.GetRef(current).open = false;
                // Process the neigbours.
                for (int i = 0; i < Neighbour8.GetLength(0); i++)
                {
                    //Vec2i curCoord = GetCoord(m_map, current);
                    Vec2i curCoord = current;
                    Vec2i neiCoord = GetNeighbourCoord(curCoord, i);
                    // Check if the neigbour is out of the map.
                    if (neiCoord.x < 0 || neiCoord.x >= m_map.Width)  { continue; }
                    if (neiCoord.y < 0 || neiCoord.y >= m_map.Height) { continue; }
                    // Avoid diagonals.
                    if (neiCoord != target || includeTarget)
                    {
                        // Check the neigbours in the corners.
                        /*
                        if (i % 2 == 0)
                        {
                            Vec2i prev = GetNeighbourCoord(curCoord, GetPrevNeighbour(i));
                            Vec2i next = GetNeighbourCoord(curCoord, GetNextNeighbour(i));

                            //if (m_map.IsSolid(prev)) { continue; }
                            //if (m_map.IsSolid(next)) { continue; }
                            // Check if the neigbour is behind a narrow corner.
                            if (m_parameters.solid(m_map, prev) && m_parameters.solid(m_map, next)) 
                            {
                                continue;
                            }
                        }
                        */
                        // Check if wall is passable.
                        //if (m_map.IsSolid(neiCoord))
                        //{
                            //continue;
                        //}
                        if (m_parameters.solid(m_map, neiCoord))
                        {
                            continue;
                        }
                    }

                    //int   neighbour = GetIndex(m_map, neiCoord);
                    //float gCost     = m_nodes[current].gCost + curCoord.DiagonalDistance(neiCoord);
                    //float gCost     = m_nodes[current].gCost + m_parameters.cost(m_map, curCoord, neiCoord);
                    float gCost = m_nodes.GetRef(current).gCost + m_parameters.cost(m_map, curCoord, neiCoord);
                    // Add the neighbour to the open list if needed.
                    if (m_nodes.GetRef(neiCoord).source < 0 || gCost < m_nodes.GetRef(neiCoord).gCost)
                    {
                        if (!m_nodes.GetRef(neiCoord).open)
                        {
                            m_open.Add(neiCoord);
                        }
                        //m_nodes[neighbour].open   = true;
                        //m_nodes[neighbour].source = current;
                        //m_nodes[neighbour].gCost  = gCost;
                        //m_nodes[neighbour].hCost  = m_parameters.heuristic(m_map, neiCoord, target);
                        m_nodes.GetRef(neiCoord).open   = true;
                        m_nodes.GetRef(neiCoord).source = m_nodes.GetCoordAsIndex(current);
                        m_nodes.GetRef(neiCoord).gCost  = gCost;
                        m_nodes.GetRef(neiCoord).hCost  = m_parameters.heuristic(m_map, neiCoord, target);
                    }
                }
            }

            //if (m_nodes[targetIdx].source >= 0)
            if (m_nodes.GetRef(target).source >= 0)
            {
                int c = 0;
                int s = includeTarget ? m_nodes.GetCoordAsIndex(target) : m_nodes.GetRef(target).source;

                for (int i = s; i != m_nodes.GetCoordAsIndex(origin); i = m_nodes.GetRef(m_nodes.GetIndexAsCoord(i)).source)
                {
                    //path.Add(GetCoord(m_map, i));
                    path.Add(m_nodes.GetIndexAsCoord(i));
                    c++;
                }

                if (includeOrigin)
                {
                    path.Add(origin);
                }

                path.Reverse();

                return true;
            }

            return false;
        }

        /*
        public bool Find(GameMap map, Vec2i origin, Vec2i target, Parameters parameters, bool includeOrigin, bool includeTarget, Path2i path)
        {
            ClearFrontier();
            ClearNodes();
            // Set the paremeters and the default functions if no one was supplied.
            m_parameters.Set(parameters);
            m_parameters.solid     ??= CalculateSolid;
            m_parameters.cost      ??= CalculateCost;
            m_parameters.heuristic ??= CalculateHeuristic;
            m_parameters.priority  ??= CalculatePriority;

            int originIdx = GetIndex(map, origin);
            int targetIdx = GetIndex(map, target);

            m_nodes[originIdx].open   = true;
            m_nodes[originIdx].source = -1;
            m_nodes[originIdx].gCost  =  0;
            m_nodes[originIdx].hCost  =  0;

            m_open.Add(originIdx);

            while (m_open.Count > 0)
            {
                float max   = float.MaxValue;
                int   found = 0;
                // Find the node with less cost.
                // TODO: Replace this with a heap.
                for (int i = 0; i < m_open.Count; i++)
                {
                    //var totalCost = CalculateTotalCost(m_nodes[m_open[i]].gCost, m_nodes[m_open[i]].hCost);
                    var totalCost = m_parameters.priority(map, m_nodes[m_open[i]].gCost, m_nodes[m_open[i]].hCost);

                    if (totalCost < max)
                    {
                        found = i;
                        max   = totalCost;
                    }
                }
                // Swap with the last one and remove.
                int current   = m_open[found];
                m_open[found] = m_open[m_open.Count - 1];
                m_open.RemoveAt(m_open.Count - 1);
                // Check if the target has been reached.
                if (current == targetIdx)
                {
                    break;
                }
                // Mark the node as closed.
                m_nodes[current].open = false;
                // Process the neigbours.
                for (int i = 0; i < Neighbour8.GetLength(0); i++)
                {
                    Vec2i curCoord = GetCoord(map, current);
                    Vec2i neiCoord = GetNeighbourCoord(curCoord, i);
                    // Check if the neigbour is out of the map.
                    if (neiCoord.x < 0 || neiCoord.x >= map.Width)  { continue; }
                    if (neiCoord.y < 0 || neiCoord.y >= map.Height) { continue; }
                    // Avoid diagonals.
                    if (neiCoord != target || includeTarget)
                    {
                        if (m_parameters.solid(map, neiCoord))
                        {
                            continue;
                        }
                    }

                    int   neighbour = GetIndex(map, neiCoord);
                    //float gCost     = m_nodes[current].gCost + curCoord.DiagonalDistance(neiCoord);
                    float gCost     = m_nodes[current].gCost + m_parameters.cost(map, curCoord, neiCoord);
                    // Add the neighbour to the open list if needed.
                    if (m_nodes[neighbour].source < 0 || gCost < m_nodes[neighbour].gCost)
                    {
                        if (!m_nodes[neighbour].open)
                        {
                            m_open.Add(neighbour);
                        }

                        m_nodes[neighbour].open   = true;
                        m_nodes[neighbour].source = current;
                        m_nodes[neighbour].gCost  = gCost;
                        m_nodes[neighbour].hCost  = m_parameters.heuristic(map, neiCoord, target);
                    }
                }
            }

            if (m_nodes[targetIdx].source >= 0)
            {
                int c = 0;
                int s = includeTarget ? targetIdx : m_nodes[targetIdx].source;

                for (int i = s; i != originIdx; i = m_nodes[i].source)
                {
                    path.Add(GetCoord(map, i));
                    c++;
                }

                if (includeOrigin)
                {
                    path.Add(origin);
                }

                path.Reverse();

                return true;
            }

            return false;
        }
        */

        // ------------------
        // DEFAULT PARAMETERS
        // ------------------

        public static float CalculatePriority(GameMap map, float accum, float heuristic)
        {
            return accum + heuristic;
        }

        public static float CalculateHeuristic(GameMap map, Vec2i a, Vec2i b)
        {
            return a.DiagonalDistance(b);
        }

        public static float CalculateCost(GameMap map, Vec2i a, Vec2i b)
        {
            return a.DiagonalDistance(b);
        }

        public static bool CalculateSolid(GameMap map, Vec2i coord)
        {
            return map.IsSolid(coord);
        }

        // ---------
        // UTILITIES
        // ---------

        private void ClearFrontier()
        {
            m_open.Clear();
        }

        public void ClearNodes()
        {
            m_nodes.Clear(Node.Initial);
        }

        /*
        private void ClearNodes()
        {
            for (int i = 0; i < m_nodes.Length; i++)
            {
                m_nodes[i].open   = false;
                m_nodes[i].source = -1;
                m_nodes[i].gCost  =  0;
                m_nodes[i].hCost  =  0;
            }
        }
        */

        /*
        private void ClearFrontier()
        {
            m_open.Clear();
        }
        */

        // -------
        // HELPERS
        // -------

        private static int GetPrevNeighbour(int i)
        {
            return i <= 0 ? 7 : i - 1;
        }

        private static int GetNextNeighbour(int i)
        {
            return i >= 7 ? 0 : i + 1;
        }

        private static Vec2i GetNeighbourCoord(Vec2i coord, int i)
        {
            return new Vec2i(coord.x + Neighbour8[i, 0], coord.y + Neighbour8[i, 1]);
        }
    }
}
