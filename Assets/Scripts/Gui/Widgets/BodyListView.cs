using UnityEngine;
using Rogue.Game;
using Rogue.Game.Stock;
using Rogue.Core;

namespace Rogue.Gui.Widgets
{
    public class BodyListView : ListView<BodyListView.Note>
    {
        public struct Note
        {
            public string name;
        }

        public void AddItem(string name)
        {
            var node = CreateNode();

            node.data = new ()
            {
                name  = name,
            };
        }

        public void Sync(Ident eid)
        {
            RemoveAll();

            var entity = Rogue.Context.World.Find(eid);
            var cBody  = entity.FindFirstComponent<Game.Comp.Body>();

            if (cBody == null)
            {
                return;
            }

            var bid  = cBody.bid;
            var body = Rogue.Context.Bodies.Get(bid);

            for (int i = 0; i < Body.MaxMembers; i++)
            {
                BodyMember member = body.At(i);
                if (member == null)
                {
                    continue;
                }

                AddItem(member.name);
            }

            Draw();
        }
    }
}
