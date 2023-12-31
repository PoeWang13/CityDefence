﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map_Manager))]
public class Map_Manager_Editor : Editor
{
    private SerializedProperty width;
    private SerializedProperty height;
    private SerializedProperty wayParent;
    private SerializedProperty mapGrids;
    private SerializedProperty pathLoopWaysSize;

    private Map_Manager map_Manager;
    private void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
        wayParent = serializedObject.FindProperty("wayParent");
        mapGrids = serializedObject.FindProperty("mapGrids");
        pathLoopWaysSize = serializedObject.FindProperty("pathLoopWaysSize");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        map_Manager = (Map_Manager)target;

        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);
        EditorGUILayout.PropertyField(wayParent);
        EditorGUILayout.PropertyField(mapGrids);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("After change the list, Close and open the list for sorting.", MessageType.Warning);
        EditorGUILayout.PropertyField(pathLoopWaysSize);
        if (EditorGUI.EndChangeCheck())
        {
            int crossWaysSizeLenght = pathLoopWaysSize.arraySize - 1;
            for (int e = crossWaysSizeLenght; e >= 0; e--)
            {
                Vector2Int crossWaysSizeElement = pathLoopWaysSize.GetArrayElementAtIndex(e).vector2IntValue;
                if (crossWaysSizeElement.x < 3)
                {
                    pathLoopWaysSize.GetArrayElementAtIndex(e).vector2IntValue = new Vector2Int(3, crossWaysSizeElement.y);
                    Debug.LogWarning("Cross Way Size X and Y should be min 3.");
                }
                if (crossWaysSizeElement.y < 3)
                {
                    pathLoopWaysSize.GetArrayElementAtIndex(e).vector2IntValue = new Vector2Int(crossWaysSizeElement.x, 3);
                    Debug.LogWarning("Cross Way Size X and Y should be min 3.");
                }
            }
            map_Manager.ReOrderLoopWayList();
        }
        if (EditorApplication.isPlaying && GUILayout.Button("Create New Map", GUILayout.Height(20)))
        {
            map_Manager.CreateMap();
        }

        serializedObject.ApplyModifiedProperties();
    }
}