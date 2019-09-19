using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Devdog.SciFiDesign.Editors
{
    public class SciFiDocumentationLinkEditor : EditorWindow
    {
        [MenuItem("Tools/Sci-Fi Design/Documentation", false, 99)] // Always at bottom
        public static void ShowWindow()
        {
            Application.OpenURL("http://devdog.nl/product/sci-fi-design/");
        }
    }
}