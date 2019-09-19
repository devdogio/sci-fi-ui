using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEngine.UI;


namespace Devdog.SciFiDesign
{
    public class SciFiSceneLoaderInstantiator : MonoBehaviour
    {
        [Required]
        public SciFiSceneLoaderHelper helper;

        public string sceneToLoad;

        public void Instantiate()
        {
            var h = Instantiate<SciFiSceneLoaderHelper>(helper);
            h.LoadScene(sceneToLoad);
        }
    }
}