using System;
using System.Collections.Generic;

namespace Rogue.Core.Collections 
{
    public class IdentBagMap
    {
        /// <summary>
        /// List of bags.
        /// </summary>
        private List<IdentBag> mBags = new List<IdentBag>();

        /// <summary>
        /// List of free bags (bag ready to be reused).
        /// </summary>
        private Queue<int> mFree = new Queue<int>();

        /// <summary>
        /// Create a new bag.
        /// </summary>
        /// <returns>Index of the bag.</returns>
        public int Create()
        {
            int index;

            if (mFree.Count > 0)
            {
                index = mFree.Dequeue();
            }
            else
            {
                index = mBags.Count;
                        mBags.Add(new IdentBag());
            }

            return index;
        }

        /// <summary>
        /// Release a bag.
        /// 
        /// After the call to this function the bag is ready to be reused by a call to "create".
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        public void Release(int bag)
        {
            mBags[bag].Clear();
            mFree.Enqueue(bag);
        }

        /// <summary>
        /// Check if there are no entities inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <returns>True if there are not entities in the bag; otherwise, false.</returns>
        public bool Empty(int bag)
        {
            return mBags[bag].Count <= 0;
        }

        /// <summary>
        /// Get the number of entities inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <returns>Number of entities in the bag.</returns>
        public int Count(int bag)
        {
            return mBags[bag].Count;
        }

        /// <summary>
        /// Check if a bag contains an entity.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if the entity is in the bag; otherwise, false.</returns>
        public bool Contains(int bag, Ident eid)
        {
            return mBags[bag].FindIndex(i => i == eid) >= 0;
        }

        /// <summary>
        /// Get the first entity inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if there is an entity in the bag; otherwise, false.</returns>
        public bool TryGetFirst(int bag, out Ident eid)
        {
            return TryGetNth(bag, 0, out eid);
        }

        /// <summary>
        /// Get nth entity inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="nth">Number.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if there is a nth entity in the bag; otherwise, false.</returns>
        public bool TryGetNth(int bag, int nth, out Ident eid)
        {
            eid = Ident.Zero;

            if (nth >= Count(bag))
            {
                return false;
            }

            eid = mBags[bag][nth];

            return true;
        }
        
        /// <summary>
        /// Performs the specified action on each value in a coordinate.
        /// </summary>
        /// <param name="bag">Bag.</param>
        /// <param name="action">Action.</param>
        public void ForEach(int bag, Action<Ident> action)
        {
            for (int i = 0; TryGetNth(bag, i, out Ident eid); i++)
            {
                action(eid);
            }
        }

        /// <summary>
        /// Add an entity to a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="eid">Entity id.</param>
        public void Add(int bag, Ident eid)
        {
            mBags[bag].Add(eid);
        }

        /// <summary>
        /// Move an entitiy from a bag to another one.
        /// </summary>
        /// <param name="origin">Index of the origin bag.</param>
        /// <param name="target">Index of the target bag.</param>
        /// <param name="eid">Entity id.</param>
        public void Move(int origin, int target, Ident eid)
        {
            Remove(origin, eid);
            Add   (target, eid);
        }

        /// <summary>
        /// Move all entities from a bag to another one.
        /// </summary>
        /// <param name="origin">Index of the origin bag.</param>
        /// <param name="target">Index of the target bag.</param>
        public void MoveAll(int origin, int target)
        {
            if (Empty(origin))
            {
                return;
            }

            IdentBag originBag = mBags[origin];
            IdentBag targetBag = mBags[target];

            if (Empty(target))
            {
                mBags[origin] = targetBag;
                mBags[target] = originBag;
            }
            else
            {
                targetBag.AddRange(originBag);
                originBag.Clear();
            }
        }

        /// <summary>
        /// Remove an entities from a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>Number of entities removed from the bag.</returns>
        public int Remove(int bag, Ident eid)
        {
            if (Empty(bag))
            {
                return 0;
            }

            var list  = mBags[bag];
            int index = mBags[bag].FindIndex(i => i == eid);

            if (index < 0)
            {
                return 0;
            }

            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            return 1;
        }

        /// <summary>
        /// Remove all the entities from a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        public void RemoveAll(int bag)
        {
            mBags[bag].Clear();
        }
    }
}