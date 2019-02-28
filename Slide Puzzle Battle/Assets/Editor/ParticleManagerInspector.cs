using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ParticleManager))] 
public class ParticleManagerInspector : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("items"), true, true, true, true);
    }

    public override void OnInspectorGUI()
    {
        ParticleManager instance = ParticleManager.Instance;
        
        instance.size = EditorGUILayout.IntField("Size", instance.size);

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Particle List");
        };

        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("key"), GUIContent.none);

                EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("value"), GUIContent.none);
            };
    }
}
