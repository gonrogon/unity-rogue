using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Gui.Widgets
{
    public class ListView<T> : MonoBehaviour
    {
        public class ListViewNode
        {
            public int id;

            public T data;

            public ListViewItem<T> item;
        }
        
        [SerializeField]
        private ListViewItem<T> m_prefab;

        [SerializeField]
        private RectTransform m_content;

        private readonly List<ListViewNode> m_nodes = new ();

        private int m_nextId = 0;

        protected ListViewNode CreateNode()
        {
                   m_nodes.Add(new () { id = m_nextId++ });
            return m_nodes[^1];
        }

        protected ListViewNode GetNode(int id)
        {
            int index = m_nodes.FindIndex(node => node.id == id);
            if (index < 0)
            {
                return new () { id = -1 };
            }

            return m_nodes[index];
        }

        protected void RemoveNode(int id)
        {
            int index = m_nodes.FindIndex(node => node.id == id);
            if (index < 0)
            {
                return;
            }

            m_nodes.RemoveAt(index);
        }


        public void Remove(int id)
        {
            int index = m_nodes.FindIndex(node => node.id == id);
            if (index < 0)
            {
                return;
            }

            Destroy(m_nodes[index].item.gameObject);
            m_nodes.RemoveAt(id);
        }

        public void RemoveAll()
        {
            foreach (var node in m_nodes)
            {
                Destroy(node.item.gameObject);
            }
            m_nodes.Clear();
        }

        protected void Draw()
        {
            foreach (ListViewNode node in m_nodes)
            {
                if (node.item == null)
                {
                    DrawNode(node);
                }
            }
            // Reorder the items if it is needed.
            for (int i = 0; i < m_nodes.Count; i++)
            {
                if (m_nodes[i].item.transform.GetSiblingIndex() != i)
                {
                    m_nodes[i].item.transform.SetSiblingIndex(i);
                }
            }
        }

        protected void DrawNode(ListViewNode node)
        {
            if (node.id < 0)
            {
                return;
            }

            node.item = Instantiate(m_prefab, m_content);
            node.item.Setup(this, node.id, node.data);
        }
    }
}
