using UnityEngine;

namespace Rogue.Gui.Widgets
{
    public abstract class ListViewItem<T> : MonoBehaviour
    {
        public virtual void Setup(ListView<T> list, int id, T data) 
        {}
    }
}