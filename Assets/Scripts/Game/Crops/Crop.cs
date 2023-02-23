using System.Collections.Generic;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Crops
{
    public class Crop : Map.IMulticell
    {
        /// <summary>
        /// Identifier of this crop in the crop system.
        /// </summary>
        private int m_cid;

        /// <summary>
        /// Identifier of the entity linked to this crop.
        /// </summary>
        private Ident m_eid;

        /// <summary>
        /// Type of plant to grow in the crop.
        /// </summary>
        private PlantType m_plantType = null;

        /// <summary>
        /// List of fields.
        /// </summary>
        private Field m_field = null;

        /// <summary>
        /// Gets the identifier of the entity linked to this crop.
        /// </summary>
        public Ident EntityId => m_eid;

        /// <summary>
        /// Gets the zone occupied by the crop.
        /// </summary>
        public Rect2i Zone => m_field.Zone;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cid">Identifier.</param>
        /// <param name="plantType">Type of plant to grow in the crop.</param>
        public Crop(int cid, PlantType plantType, Rect2i zone)
        {
            m_cid       = cid;
            m_plantType = plantType;
            m_field     = new Field(zone);
        }

        public void SetEntity(Ident eid)
        {
            this.m_eid = eid;
        }

        public Vec2i GetOrigin() => m_field.Zone.Min;

        public bool ContainsCoord(Ident eid, Vec2i coord) => m_field.HasCoord(coord);

        public bool ContainsCoord(Vec2i coord) => m_field.HasCoord(coord);

        public bool Overlaps(Rect2i zone) => m_field.Overlaps(zone);

        public void Update(int elapsed)
        {
            m_field.Update(elapsed);

            Context.World.Send(m_eid, new Msg.Grow());

            CreateJobs();
        }

        #region @@@ SOIL @@@

        public Soil GetSoil(Vec2i coord) => m_field.GetSoil(coord);

        public bool Plow(Vec2i coord) => m_field.Plow(coord);

        public bool Seed(Vec2i coord) => m_field.Seed(coord, m_plantType);

        public bool Harvest(Vec2i coord) => m_field.Harvest(coord);

        #endregion

        #region ### PLANTS ###

        public PlantType GetPlantType() => m_plantType;

        public Plant GetPlant(Vec2i coord) => m_field.GetPlant(coord);

        #endregion

        #region @@@ JOBS @@@

        public void OnJobCompleted(Jobs.Job job)
        {
            switch (job)
            {
                case Jobs.JobPlow:
                case Jobs.JobSeed:    m_field.ClearSoilJob (job.Location); break;
                case Jobs.JobHarvest: m_field.ClearPlantJob(job.Location); break;
            }

            //CreateJobs();
        }

        public void CreateJobs()
        {
            for (int index = 0; index < m_field.Count; index++)
            {
                if (m_field.HasPendingJob(index))
                {
                    continue;
                }

                // SOIL JOBS

                var soil  = m_field.GetSoil (index);
                var plant = m_field.GetPlant(index);

                if (soil.plowed == false)
                {
                    soil.job = CreateJobPlow(m_field, index);
                    break;
                }
                
                if (plant == null)
                {
                    soil.job = CreateJobSeed(m_field, index);
                    break;
                }

                // PLANT JOBS

                if (plant.HarvestReady)
                {
                    plant.job = CreateJobHarvest(m_field, index);
                    break;
                }
            }
        }

        private int CreateJobHarvest(Field field, int index)
        {
            Jobs.JobHarvest job = new (m_cid, m_eid, field.GetCoord(index))
            {
                onCompleted = OnJobCompleted
            };

            return Context.Jobs.AddJob(job);
        }

        private int CreateJobSeed(Field field, int index)
        {
            Jobs.JobSeed job = new (m_cid, m_eid, field.GetCoord(index))
            {
                onCompleted = OnJobCompleted
            };

            return Context.Jobs.AddJob(job);
        }

        private int CreateJobPlow(Field field, int index)
        {
            Jobs.JobPlow job = new (m_cid, m_eid, field.GetCoord(index))
            {
                onCompleted = OnJobCompleted
            };

            return Context.Jobs.AddJob(job);
        }

        #endregion
    }
}
