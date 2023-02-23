using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    [System.Serializable]
    public class JobBuilding : Job
    {
        /// <summary>
        /// Identifier of the entity to be build.
        /// </summary>
        public Ident building;

        /// <summary>
        /// Number of builders already assigned to this job.
        /// </summary>
        public int builders = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="scaffold">Entitiy identifier of the scaffold.</param>
        public JobBuilding(Ident building, int total, Vec2i location)
            :
            base(total, location)
        {
            this.building = building;
        }
    }
}
