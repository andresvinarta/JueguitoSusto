/*
 * 
 * Created by Andrés Viñarta Flores -> andresvinarta@gmail.com
 * 
 * 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReferenceFinder : EditorWindow
{
    GameObject targetObject;
    Vector2 scrollPosition;

    class ReferenceInfo
    {
        public GameObject gameObject;
        public List<string> componentReferences = new List<string>();
    }

    List<ReferenceInfo> referencingObjects = new List<ReferenceInfo>();

    [MenuItem("GameObject/Find GameObject References")]
    public static void ShowWindow()
    {
        GetWindow<ReferenceFinder>("Find GameObject References");
    }

    private void OnGUI()
    {
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Find References"))
        {
            if (targetObject == null)
            {
                Debug.LogError("Please assign a target object.");
                return;
            }

            FindReferences();
        }

        if (referencingObjects.Count > 0)
        {
            GUILayout.Label($"Found {referencingObjects.Count} reference(s):", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
            foreach (var reference in referencingObjects)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (GUILayout.Button(reference.gameObject.name, EditorStyles.linkLabel))
                {
                    EditorGUIUtility.PingObject(reference.gameObject);
                }

                foreach (var componentInfo in reference.componentReferences)
                {
                    GUILayout.Label($"  {componentInfo}", EditorStyles.label);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
        else if (targetObject != null)
        {
            GUILayout.Label("No references found.", EditorStyles.helpBox);
        }
    }

    private void FindReferences()
    {
        referencingObjects.Clear();

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            List<string> referencingComponents = new List<string>();

            Component[] components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null) continue;

                SerializedObject serializedObject = new SerializedObject(component);
                SerializedProperty property = serializedObject.GetIterator();
                while (property.NextVisible(true))
                {
                    if (property.propertyType == SerializedPropertyType.ObjectReference &&
                        property.objectReferenceValue != null &&
                        (property.objectReferenceValue == targetObject || IsComponentOfTarget(property.objectReferenceValue)))
                    {
                        string componentName = component.GetType().Name;
                        string targetComponentName = property.objectReferenceValue.GetType().Name;
                        string variableName = property.name;

                        referencingComponents.Add($"{componentName} -> {variableName} ({targetComponentName})"
);
                    }
                }
            }

            if (referencingComponents.Count > 0)
            {
                referencingObjects.Add(new ReferenceInfo
                {
                    gameObject = obj,
                    componentReferences = referencingComponents
                });
            }
        }
    }

    private bool IsComponentOfTarget(Object reference)
    {
        if (reference is Component component)
        {
            return component.gameObject == targetObject;
        }
        return false;
    }
}
