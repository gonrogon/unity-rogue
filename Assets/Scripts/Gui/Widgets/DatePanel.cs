using UnityEngine;

namespace Rogue.Gui.Widgets 
{
    public class DatePanel : MonoBehaviour
    {
        [SerializeField]
        private Director m_director;

        [SerializeField]
        private TMPro.TMP_Text m_label;

        private void FixedUpdate()
        {
            var tm =  Rogue.Context.TimeManager;
            if (tm == null) 
            {
                return;
            }

            int hours = tm.Date.hour;
            int days  = tm.Date.day;
            int month = tm.Date.month;
            int years = tm.Date.year;

            string text = "Year " + years.ToString() + ", Month " + month + ", Day " + days.ToString() + ", Hour " + hours.ToString();
            m_label.text = text;
        }
    }
}