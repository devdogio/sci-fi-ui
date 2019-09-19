using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class SetDateTime : MonoBehaviour
    {
        [SerializeField]
        private Text _textTimeHour;

        [SerializeField]
        private Text _textTimeMinute;

        [SerializeField]
        private Text _textColon;

        [SerializeField]
        private Text _textDay;


        public bool blinkColon = false;
        public float blinkColonInterval = 0.5f;

        protected void Start()
        {
            InvokeRepeating("UpdateTime", 0f, 60f);

            if (blinkColon)
            {
                InvokeRepeating("UpdateColon", 0f, blinkColonInterval);
            }
        }

        protected void UpdateTime()
        {
            var n = System.DateTime.Now;

            if (_textTimeHour != null)
            {
                _textTimeHour.text = n.Hour < 10 ? "0" + n.Hour : n.Hour.ToString();
            }

            if (_textTimeHour != null)
            {
                _textTimeMinute.text = n.Minute < 10 ? "0" + n.Minute : n.Minute.ToString();
            }

            if (_textDay != null)
            {
                _textDay.text = n.DayOfWeek.ToString().ToUpper();
            }
        }

        protected void UpdateColon()
        {
            if (string.IsNullOrEmpty(_textColon.text))
            {
                _textColon.text = ":";
            }
            else
            {
                _textColon.text = "";
            }
        }
    }
}