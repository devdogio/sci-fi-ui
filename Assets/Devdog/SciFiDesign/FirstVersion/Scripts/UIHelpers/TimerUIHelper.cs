using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class TimerUIHelper : MonoBehaviour
    {
        public void EnableMaskAfterSeconds(float waitTime)
        {
            StartCoroutine(_SetEnabledAfterSeconds(GetComponent<Mask>(), true, waitTime));
        }

        public void DisableMaskAfterSeconds(float waitTime)
        {
            StartCoroutine(_SetEnabledAfterSeconds(GetComponent<Mask>(), false, waitTime));
        }

        private IEnumerator _SetEnabledAfterSeconds(MonoBehaviour comp, bool state, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            comp.enabled = state;
        }

        public void SetActiveAfter(float waitTime)
        {
            if (waitTime > 0f)
                StartCoroutine(_SetActive(waitTime, true));
            else
                _SetActive(true);
        }

        public void SetInActiveAfter(float waitTime)
        {
            if (waitTime > 0f)
                StartCoroutine(_SetActive(waitTime, false));
            else
                _SetActive(false);
        }

        private IEnumerator _SetActive(float waitTime, bool set)
        {
            yield return new WaitForSeconds(waitTime);
            _SetActive(set);
        }

        private void _SetActive(bool set)
        {
            gameObject.SetActive(set);
        }
    }
}