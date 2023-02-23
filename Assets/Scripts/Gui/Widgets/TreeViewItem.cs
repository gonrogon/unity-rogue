using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rogue.Gui.Widgets
{
    public class TreeViewItem : MonoBehaviour
    {
        public virtual TreeViewItem Setup(TreeView tree, int id, int depth, bool open, string label)
        {
            return this;
        }
    }
}
