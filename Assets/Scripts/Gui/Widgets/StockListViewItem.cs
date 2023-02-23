using UnityEngine;
using UnityEngine.UI;

namespace Rogue.Gui.Widgets
{
    public class StockListViewItem : ListViewItem<StockListView.Note>
    {
        [SerializeField]
        private TMPro.TMP_Text m_label;

        [SerializeField]
        private TMPro.TMP_Text m_stock;

        [SerializeField]
        private Button m_sell;

        [SerializeField]
        private Toggle m_trading;

        private StockListView m_list;

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

        public override void Setup(ListView<StockListView.Note> list, int id, StockListView.Note data) 
        {
            m_id   = id;
            m_list = (StockListView)list;

            m_label.text   = data.name;
            m_stock.text   = data.stock.ToString();
            m_trading.isOn = data.trading;
        }

        private void Awake()
        {
            m_sell.onClick.AddListener(OnSell);
        }

        private void OnSell()
        {
            m_list.Sell(m_id);
        }
    }
}