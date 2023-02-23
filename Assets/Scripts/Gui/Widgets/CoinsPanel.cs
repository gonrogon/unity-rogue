using UnityEngine;

namespace Rogue.Gui.Widgets 
{
    public class CoinsPanel : MonoBehaviour
    {
        [SerializeField]
        private Director m_director;

        [SerializeField]
        private TMPro.TMP_Text m_label;

        private void FixedUpdate()
        {
            m_label.text = m_director.money.ToString();
        }
    }
}