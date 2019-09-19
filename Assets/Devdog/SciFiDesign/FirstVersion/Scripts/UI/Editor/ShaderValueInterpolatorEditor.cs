using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using Devdog.SciFiDesign.UI;

namespace Devdog.SciFiDesign.Editors
{
    [CustomEditor(typeof(ShaderValueInterpolator), true)]
    public class ShaderValueInterpolatorEditor : Editor
    {

        private int _scanDepth = 5;
        private float _delayIncrease = 0.02f;
        private float _textScale32 = 1f; // Use 32 as the base, 16 will use 50% effect scale.

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, new string[] { });

            EditorGUILayout.LabelField("Editor: automated", EditorStyles.boldLabel);
            _scanDepth = EditorGUILayout.IntField("Scan depth", _scanDepth);
            _delayIncrease = EditorGUILayout.FloatField("Delay increase", _delayIncrease);
            _textScale32 = EditorGUILayout.FloatField("Text scale effect", _textScale32);

            if (GUILayout.Button("Search for graphics in children"))
            {
                var t = (ShaderValueInterpolator) target;
                var graphics = t.gameObject.GetComponentsInChildren<Graphic>();

                int curDepth = GetDepth(t.transform);
                int maxDepth = curDepth + _scanDepth;

                float timer = 0f;
                var l = new List<ShaderValueInterpolator.GraphicRow>(graphics.Length);
                for (int i = 0; i < graphics.Length; i++)
                {
                    if (GetDepth(graphics[i].transform) > maxDepth)
                    {
                        continue;
                    }

                    var row = new ShaderValueInterpolator.GraphicRow();
                    row.graphic = graphics[i];
                    row.waitTime = (float)System.Math.Round(timer++ * _delayIncrease, 3);

                    var text = graphics[i] as Text;
                    if (text != null)
                    {
                        row.intensityFactor = 1f * (text.fontSize / (32 * _textScale32));
                    }
                    else
                    {
                        row.intensityFactor = 1f;
                    }

                    l.Add(row);
                }

                t.graphics = l.ToArray();
                
                EditorUtility.SetDirty(t);
                GUI.changed = true; // Update
//                EditorUtility.SetDirty(t);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private int GetDepth(Transform t)
        {
            int counter = 0;
            var parent = t.parent;
            while (parent != null)
            {
                counter++;
                parent = parent.parent;
            }

            return counter;
        }
    }
}