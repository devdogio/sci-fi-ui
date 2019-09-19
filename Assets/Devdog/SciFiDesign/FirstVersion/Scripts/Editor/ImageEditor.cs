using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.Editors
{
    public class ImageEditor : EditorWindow
    {

        private Color _color = Color.white;

        [MenuItem("Tools/Sci-Fi Design/Tools/Image editor")]
        public static void Window()
        {
            var w = GetWindow<ImageEditor>();
            w.Show(); 
        }


        private void OnGUI()
        {
            _color = EditorGUILayout.ColorField("Color", _color);

            if (GUILayout.Button("Change child images to color"))
            {
                var o = Selection.activeGameObject;
                if (o == null)
                {
                    Debug.LogWarning("No object selected, can't change images.");
                    return;
                }

                var images = o.GetComponentsInChildren<Image>();
                if (EditorUtility.DisplayDialog("Change images",
                    "Are you sure you want to change the color on " + images.Length + " images", "Yes", "No"))
                {
                    foreach (var image in images)
                    {
                        image.color = _color;
                    }
                }
            }       
        }
    }
}