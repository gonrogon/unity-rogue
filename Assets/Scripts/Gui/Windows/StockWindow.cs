using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Gui
{
    public class StockWindow : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_root;

        public void Toggle()
        {
            m_root.SetActive(!m_root.activeSelf);
        }
    }
}
