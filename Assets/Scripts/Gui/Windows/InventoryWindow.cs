using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Gui
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField]
        private Widgets.InventoryListView m_list;

        [SerializeField]
        private TMPro.TMP_Text m_subtitle;

        [SerializeField]
        private GameObject m_root;

        public void Toggle()
        {
            m_root.SetActive(!m_root.activeSelf);
        }

        public void Select()
        {
            ContextSettings settings = new();
            //settings.start  = OnSelectionStart;
            settings.end    = OnSelectionFinish;
            //settings.notify = OnSelectionNotity;

            Rogue.Context.Input.Contexts.Push("selectOne", settings);
        }

        public void OnSelectionFinish(Context context)
        {
            var ctx = (Game.Gui.ContextSelectOne)context;
            if (ctx.Entity.IsZero)
            {
                return;
            }

            m_subtitle.text = Game.Query.GetName(ctx.Entity).value;

            m_list.Sync(ctx.Entity);
        }
    }
}
