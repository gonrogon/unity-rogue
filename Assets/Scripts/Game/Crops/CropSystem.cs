using System.Collections.Generic;
using GG.Mathe;

namespace Rogue.Game.Crops
{
    public class CropSystem
    {
        /// <summary>
        /// Database with the plant types.
        /// </summary>
        public readonly PlantDatabase m_plants = new();

        /// <summary>
        /// List of crops.
        /// </summary>
        public readonly List<Crop> m_crops = new();

        public PlantDatabase Plants => m_plants;

        public int CreateCrop(string planType, Rect2i zone)
        {
            if (m_plants.TryGet(planType, out PlantType realType))
            {
                return CreateCropImpl(realType, zone);
            }

            return -1;
        }

        private int CreateCropImpl(PlantType plantType, Rect2i zone)
        {
            if (Overlaps(zone))
            {
                return -1;
            }

            m_crops.Add(new Crop(m_crops.Count, plantType, zone));

            return m_crops.Count - 1;
        }

        public Crop At(int i)
        {
            return m_crops[i];
        }

        /// <summary>
        /// Update the crop system.
        /// </summary>
        /// <param name="elapsed">Time elapsed, in game units.</param>
        public void Update(int elapsed)
        {
            foreach (Crop crop in m_crops)
            {
                crop.Update(elapsed);
            }
        }

        public void CreateJobs(int crop)
        {
            m_crops[crop].CreateJobs();
        }

        #region @@@ UTILITIES @@@

        /// <summary>
        /// Checks if a zone overlaps any of the crops.
        /// </summary>
        /// <param name="zone">Zone.</param>
        /// <returns>True if the zone overlaps a crop; otherwise, false.</returns>
        public bool Overlaps(Rect2i zone)
        {
            foreach (Crop crop in m_crops)
            {
                if (crop.Overlaps(zone))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
