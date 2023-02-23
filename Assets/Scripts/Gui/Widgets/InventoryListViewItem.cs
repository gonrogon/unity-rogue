using UnityEngine;
using UnityEngine.UI;

namespace Rogue.Gui.Widgets
{
    public class InventoryListViewItem : ListViewItem<InventoryListView.Note>
    {
        [SerializeField]
        private TMPro.TMP_Text m_label;

        [SerializeField]
        private TMPro.TMP_Text m_stock;

        private InventoryListView m_list;

        private int m_id;

        public string Label 
        {
            get { return m_label.text; }
            set { m_label.text = value; }
        }

        public int Stock
        {
            get { return int.Parse(m_stock.text); }
            set { m_stock.text = value.ToString(); }
        }

        public override void Setup(ListView<InventoryListView.Note> list, int id, InventoryListView.Note data) 
        {
            m_id   = id;
            m_list = (InventoryListView)list;

            m_label.text   = data.name;
            m_stock.text   = data.stock.ToString();
        }
    }
}