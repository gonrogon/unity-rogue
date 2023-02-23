using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    [System.Serializable]
    public class JobPlow : Job
    {
        /// <summary>
        /// Identifier of the entity to be build.
        /// </summary>
        public int crop;

        public Ident cropId;

        /// <summary>
        /// Number of builders already assigned to this job.
        /// </summary>
        public int builders = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location.</param>
        public JobPlow(int crop, Ident cropId, Vec2i location)
            :
            base(1, location)
        {
            this.cropId = cropId;
            this.crop = crop;
        }
    }
}
