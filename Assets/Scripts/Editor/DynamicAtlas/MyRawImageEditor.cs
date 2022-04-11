using Scripts.UIDynamic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Scripts.Editor.DynamicAtlas
{
    [CustomEditor(typeof(UIDynamicRawImage))]
    [CanEditMultipleObjects]
    public class MyRawImageEditor : RawImageEditor
    {
        SerializedProperty _atlasGroup;

        protected override void OnEnable()
        {
            base.OnEnable();
            _atlasGroup = serializedObject.FindProperty("AtlasGroup");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_atlasGroup);
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}