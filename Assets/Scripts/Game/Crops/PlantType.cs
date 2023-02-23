using System.Collections.Generic;

namespace Rogue.Game.Crops
{
    public class PlantType
    {
        public string name;

        public string description;

        public int minWater = 0;

        public int maxWater = 100;

        public int water = 50;

        public int minTemperature = 0;

        public int maxTemperature = 40;

        public int temperature = 20;

        /// <summary>
        /// Growth time in seconds.
        /// </summary>
        public int growthTime = 1;

        /// <summary>
        /// List of view sorted in descending order by the growth level.
        /// </summary>
        private List<PlantView> m_views = new();

        /// <summary>
        /// Calcualtes the time between growths, in milliseconds.
        /// </summary>
        /// <returns>Time between growths, in milliseconds.</returns>
        public int GetGrowthInterval()
        {
            return growthTime * 1000 / 100;
        }

        /// <summary>
        /// Adds a view.
        /// </summary>
        /// <param name="view">View to add.</param>
        public void AddView(PlantView view)
        {
            int i = m_views.BinarySearch(view, Comparer<PlantView>.Create((a, b) => 
            {
                return b.growth - a.growth;
            }));

            m_views.Insert(i >= 0 ? i : ~i, view);
        }

        /// <summary>
        /// Gets the view for a growth level.
        /// </summary>
        /// <param name="growth">Groth level.</param>
        /// <returns>View if it exists; otherwise, null.</returns>
        public PlantView GetView(int growth)
        {
            foreach (PlantView view in m_views)
            {
                if (view.growth < growth)
                {
                    return view;
                }
            }

            return null;
        }
    }
}
