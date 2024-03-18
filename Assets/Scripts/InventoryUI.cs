using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Rogue.Core;

namespace Rogue
{
    public class InventoryUI : MonoBehaviour
    {
        struct ItemInfo
        {
            public Ident eid;
        }

        public TMP_Text itemNamePrefab;

        public TMP_Text itemDescription;

        public RectTransform itemContainer;

        private List<TMP_Text> texts = new();

        private List<ItemInfo> infos = new();

        private int selected = -1;

        public Button buttonEquip;

        public Button buttonDrop;

        private Ident eid;

        public void Sync(Ident eid, Ident iid)
        {
            this.eid = eid;
            int count = 0;

            var inventory = Context.Inventories.Get(iid);

            Game.Msg.Name msg = new();

            for (int i = 0; i < inventory.Count; i++)
            {
                Ident item = inventory.At(i);

                ItemInfo info = GetInfo(i);

                info.eid = item;
                infos[i] = info;

                Context.World.Send(item, msg);

                GetText(i).text = msg.name;
                GetText(i).gameObject.SetActive(true);
                
                count++;
            }

            for (int j = count; count < texts.Count; j++)
            {
                texts[j].gameObject.SetActive(false);
            }
        }

        private ItemInfo GetInfo(int index)
        {
            while (index >= infos.Count)
            {
                infos.Add(new ItemInfo());
            }

            return infos[index];
        }

        public TMP_Text GetText(int index)
        {
            while (index >= texts.Count)
            {
                TMP_Text text = Instantiate(itemNamePrefab, itemContainer);
                texts.Add(text);

                var trigger = text.GetComponent<EventTrigger>();

                EventTrigger.Entry entry = new();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(data => { OnItemClick(index); });
                trigger.triggers.Add(entry);
            }

            return texts[index];
        }

        public void OnItemClick(int index)
        {
            var msg = Context.World.Send(infos[index].eid, new Game.Msg.Name());

            itemDescription.text = msg.description;
            itemDescription.gameObject.SetActive(true);

            if (selected > 0)
            {
                GetText(selected).color = Color.white;
            }

            selected = index;
            GetText(selected).color = Color.yellow;
            buttonEquip.interactable = true;
            buttonDrop.interactable = true;

            Debug.Log($"ITEM CLICK: {index}");
        }

        public void OnEquip()
        {
            var oid = infos[selected].eid;
            var iid = Context.World.Find(eid).FindFirstComponent<Game.Comp.Inventory>().iid;
            var bid = Context.World.Find(eid).FindFirstComponent<Game.Comp.Body>().bid;

            Context.Inventories.Get(iid).Drop(oid);
            
            foreach (TMP_Text t in texts)
            {
                Destroy(t.gameObject);
            }
            texts.Clear();

            Sync(eid, iid);

            var lh = Context.Bodies.Get(bid).Find(Game.BodyMember.Type.Hand, "L");
            var rh = Context.Bodies.Get(bid).Find(Game.BodyMember.Type.Hand, "R");

            if (lh != null && lh.wield.IsZero)
            {
                lh.wield = oid;
                return;
            }

            if (rh != null && rh.wield.IsZero)
            {
                rh.wield = oid;
                return;
            }
        }

        public void OnDrop()
        {
            var oid = infos[selected].eid;
            var iid = Context.World.Find(eid).FindFirstComponent<Game.Comp.Inventory>().iid;
            var bid = Context.World.Find(eid).FindFirstComponent<Game.Comp.Body>().bid;

            Context.World.Send(eid, new Game.Msg.Drop(oid, Game.Query.GetPosition(eid).value));
            //Context.Inventories.Get(iid).Remove(oid);

            foreach (TMP_Text t in texts)
            {
                Destroy(t.gameObject);
            }
            texts.Clear();

            Sync(eid, iid);
        }
    }
}