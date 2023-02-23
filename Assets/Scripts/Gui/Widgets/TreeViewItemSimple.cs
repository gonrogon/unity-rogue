using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rogue.Gui.Widgets
{
    public class TreeViewItemSimple : TreeViewItem
    {
        [SerializeField]
        private RectTransform m_padding;

        [SerializeField]
        private Toggle m_toggle;

        [SerializeField]
        private TMP_Text m_label;

        private TreeView m_tree;

        private int m_id;

        private int m_depth;

        private string m_text;

        public override TreeViewItem Setup(TreeView tree, int id, int depth, bool open, string text)
        {
            m_id    = id;
            m_tree  = tree;
            m_depth = depth;
            m_text  = text;

            m_toggle.isOn = open;
            m_toggle.onValueChanged.AddListener(OnToogleValueChanged);
            m_label.text = text;

            var le = m_padding.GetComponent<LayoutElement>();
            le.minWidth       = depth * 20;
            le.preferredWidth = depth * 20;
            le.gameObject.SetActive(depth > 0);

            return this;
        }

        private void OnToogleValueChanged(bool value)
        {
            if (value)
            {
                m_tree.Open(m_id);
            }
            else
            {
                m_tree.Close(m_id);
            }
        }
    }
}
