using System;
using Devdog.SciFiDesign.UI;
using UnityEditor;

namespace Devdog.SciFiDesign.Editors
{
    [CustomEditor(typeof(Dropdown), true)]
    [CanEditMultipleObjects]
    public class DropdownEditor : UnityEditor.UI.DropdownEditor
    {
        private SerializedProperty _activeSprite;
        private SerializedProperty _inActiveSprite;
        private SerializedProperty _backgroundImage;

        private SerializedProperty _slideSpeed;
        private SerializedProperty _animationCurve;

        protected override void OnEnable()
        {
            base.OnEnable();
            _activeSprite = serializedObject.FindProperty("activeSprite");
            _inActiveSprite = serializedObject.FindProperty("inActiveSprite");
            _backgroundImage = serializedObject.FindProperty("backgroundImage");

            _slideSpeed = serializedObject.FindProperty("slideSpeed");
            _animationCurve = serializedObject.FindProperty("animationCurve");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_activeSprite);
            EditorGUILayout.PropertyField(_inActiveSprite);
            EditorGUILayout.PropertyField(_backgroundImage);

            EditorGUILayout.PropertyField(_slideSpeed);
            EditorGUILayout.PropertyField(_animationCurve);

            serializedObject.ApplyModifiedProperties();


            base.OnInspectorGUI();
            EditorGUILayout.Space();
        }
    }
}