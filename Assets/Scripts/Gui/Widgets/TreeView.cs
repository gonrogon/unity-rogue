using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Gui.Widgets
{
    public class TreeView : MonoBehaviour
    {
        public class TreeViewNode
        {
            public int depth;

            public int id;

            public string label;

            public bool open = false;

            public List<TreeViewNode> children = new ();

            public TreeViewNode AddChild(TreeViewNode node)
            {
                children.Add(node);
                return node;
            }
        }

        public TreeViewItem m_prefab;

        private List<TreeViewItem> m_items = new ();

        private Dictionary<int, TreeViewNode> m_nodes = new();

        public RectTransform m_content;

        private TreeViewNode m_root = new();

    

        // Start is called before the first frame update
        void Start()
        {
            AddNode(m_root.AddChild(new TreeViewNode() { id = 0, label = "Test 1"}));
            AddNode(m_root.AddChild(new TreeViewNode() { id = 1, label = "Test 2"}));
            AddNode(m_root.AddChild(new TreeViewNode() { id = 2, label = "Test 3"}));

            AddNode(m_root.children[0].AddChild(new TreeViewNode() { depth = 1, id = 3, label = "child 1"}));
            AddNode(m_root.children[0].AddChild(new TreeViewNode() { depth = 1, id = 4, label = "child 2"}));
            AddNode(m_root.children[1].AddChild(new TreeViewNode() { depth = 1, id = 5, label = "child 1"}));
            AddNode(m_root.children[1].AddChild(new TreeViewNode() { depth = 1, id = 6, label = "child 2"}));
            AddNode(m_root.children[1].AddChild(new TreeViewNode() { depth = 1, id = 7, label = "child 3"}));

            AddNode(m_nodes[6].AddChild(new TreeViewNode() { depth = 2, id = 8, label = "child 1"}));
            AddNode(m_nodes[6].AddChild(new TreeViewNode() { depth = 2, id = 9, label = "child 2"}));
        }

        private void AddNode(TreeViewNode node)
        {
            m_nodes.Add(node.id, node);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Open(int id)
        {
            m_nodes[id].open = true;
            Draw();
        }

        public void Close(int id)
        {
            m_nodes[id].open = false;
            Draw();
        }

        public void Draw()
        {
            foreach (TreeViewItem item in m_items)
            {
                Destroy(item.gameObject);
            }
            m_items.Clear();

            foreach (TreeViewNode node in m_root.children)
            {
                DrawNode(node);
            }
        }

        public void DrawNode(TreeViewNode node)
        {
            if (node == null)
            {
                return;
            }

            m_items.Add(Instantiate(m_prefab, m_content).Setup(this, node.id, node.depth, node.open, node.label));

            if (node.children == null || node.children.Count <= 0 || !node.open)
            {
                return;
            }

            foreach (TreeViewNode child in node.children)
            {
                DrawNode(child);
            }
        }
    }
}