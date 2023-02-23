using Rogue.Core.Collections;
using Rogue.Core;

namespace Rogue.Game
{
    public class BodySystem
    {
        private IdentMap<Body> m_bodies = new IdentMap<Body>();

        public Body Get(Ident bid)
        {
            return m_bodies.Get(bid);
        }

        public Ident Add()
        {
            return m_bodies.Add(new Body());
        }

        public Ident Add(Body body)
        {
            return m_bodies.Add(body);
        }

        public void Remove(Ident bid)
        {
            m_bodies.Release(bid);
        }
    }
}
