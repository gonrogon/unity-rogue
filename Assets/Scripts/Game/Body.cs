using Rogue.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Game
{
    public class Body
    {
        public const int MaxMembers = 10;

        [JsonIgnore]
        private Body m_template = null;

        [JsonProperty(PropertyName = "members")]
        private readonly BodyMember[] m_members = new BodyMember[MaxMembers];

        public Body() {}

        public Body(Body body)
        {
            for (int i = 0; i < MaxMembers; i++)
            {
                if (body.m_members[i] == null)
                {
                    m_members[i] = null;
                }
                else
                {
                    m_members[i] = new BodyMember(body.m_members[i]);
                }
            }
        }

        public static Body CreateFromTemplate(Body template)
        {
            Body body       = new Body(template);
            body.m_template = template;

            return body;
        }

        public BodyMember At(int i) => m_members[i];

        /// <summary>
        /// Finds the nth member.
        /// </summary>
        /// <param name="type">Type of member.</param>
        /// <param name="n">Number.</param>
        /// <returns>Reference to the member if it is found; otherwise, null.</returns>
        public BodyMember Find(BodyMember.Type type, int n)
        {
            int c = 0;

            foreach (var member in m_members)
            {
                if (member != null && member.type == type)
                {
                    if (c == n)
                    {
                        return member;
                    }

                    c++;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a member by its identifier.
        /// </summary>
        /// <param name="type">Type of member.</param>
        /// <param name="id">Identifier.</param>
        /// <returns>Reference to the member if its found; otherwise, null.</returns>
        public BodyMember Find(BodyMember.Type type, string id)
        {
            foreach (var member in m_members)
            {
                if (member != null && member.type == type)
                {
                    if (member.id == id)
                    {
                        return member;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a member.
        /// </summary>
        /// <param name="member">Member to add.</param>
        public void Add(BodyMember member)
        {
            for (int i = 0; i < MaxMembers; i++)
            {
                if (m_members[i] is null)
                {
                    m_members[i] = member;
                    break;
                }
            }
        }

        public Ident Wield(int n)
        {
            int c = 0;

            foreach (var member in m_members)
            {
                if (member != null && !member.wield.IsZero)
                {
                    if (c == n)
                    {
                        return member.wield;
                    }

                    c++;
                }
            }

            return Ident.Zero;
        }

        public bool TryGetWield(int n, out Ident eid)
        {
            eid = Ident.Zero;
            int c = 0;

            foreach (var member in m_members)
            {
                if (member != null && !member.wield.IsZero)
                {
                    if (c == n)
                    {
                        eid = member.wield;
                        return true;
                    }

                    c++;
                }
            }

            return false;
        }

        public bool IsHeld(Ident eid)
        {
            foreach (var member in m_members)
            {
                if (member != null && member.wield == eid)
                {
                    return true;
                }
            }

            return false;
        }

        public bool FindHolding(Ident eid)
        {
            foreach (var member in m_members)
            {
                if (member != null && member.AllowWield && !member.IsHolding)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Hold(Ident eid)
        {
            foreach (var member in m_members)
            {
                if (member != null && member.AllowWield && !member.IsHolding)
                {
                    member.wield = eid;
                    return true;
                }
            }

            return false;
        }

        /*
        public void Hold(int i, Ident eid)
        {
            m_members[i].hold = eid;
        }

        public void Cover(int i, Ident eid)
        {
            m_members[i].armor = eid;
        }
        */

        public void Drop(Ident eid)
        {
            foreach (var member in m_members)
            {
                if (member != null && member.wield  == eid) { member.wield  = Ident.Zero; }
                if (member != null && member.armor == eid) { member.armor = Ident.Zero; }
            }
        }

        public void DropAll()
        {
            foreach (var member in m_members)
            {
                if (member != null)
                {
                    member.wield  = Ident.Zero;
                    member.armor = Ident.Zero;
                }
            }
        }
    }
}
