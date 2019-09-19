using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class GeneralUIHelper : MonoBehaviour
    {



        public void DebugLog(string msg)
        {
            Debug.Log(msg, gameObject);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void PlayAudioClip(AudioClip clip)
        {
            AudioManager.AudioPlayOneShot(new AudioClipInfo()
            {
                audioClip = clip,
                loop = false,
                pitch = 1f,
                volume =  1f
            });
        }

        public void SwapSprite(Sprite sprite)
        {
            GetComponent<Image>().sprite = sprite;
        }

        public void LoadLevel(int id)
        {
            SceneUtility.LoadScene(id);
        }
        public void LoadLevel(string scene)
        {
            SceneUtility.LoadScene(scene);
        }
    }
}