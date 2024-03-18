using UnityEngine;
using UnityEngine.UI;

namespace Rogue.Gui.Widgets
{
    public class BodyListViewItem : ListViewItem<BodyListView.Note>
    {
        [SerializeField]
        private TMPro.TMP_Text m_label;

        [SerializeField]
        private TMPro.TMP_Text m_stock;

        private BodyListView m_list;

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

        public override void Setup(ListView<BodyListView.Note> list, int id, BodyListView.Note data) 
        {
            m_id   = id;
            m_list = (BodyListView)list;

            m_label.text   = data.name;
        }
    }
}