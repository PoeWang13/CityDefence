using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGrid))]
public class MapGrid_Editor : Editor
{
    private MapGrid myMapGrid;
    #region Property
    private SerializedProperty gridPrefab, yRot;
    #endregion

    private void OnEnable()
    {
        myMapGrid = (MapGrid)target;
        gridPrefab = serializedObject.FindProperty("gridPrefab");
        yRot = serializedObject.FindProperty("yRot");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(yRot);

        if (myMapGrid.gridPrefab != null)
        {
            Rect horizontalRect = EditorGUILayout.BeginHorizontal();
            int resimBaseHeight = 20;
            int resimIconAndTextureDistance = 30;
            Vector2Int textureSize = new Vector2Int(90, 60);
            GUILayout.Label("Map Grid Prefab", GUILayout.Height(textureSize.y));

            Rect resimRect = new Rect(horizontalRect.x, horizontalRect.y + ((horizontalRect.height - resimBaseHeight) / 2), horizontalRect.width - textureSize.x - (resimIconAndTextureDistance / 3), resimBaseHeight);
            Rect textureRect = new Rect(horizontalRect.width - textureSize.x + (resimIconAndTextureDistance / 3) * 2, horizontalRect.y + 5, textureSize.x, horizontalRect.height);

            EditorGUI.PropertyField(resimRect, gridPrefab, new GUIContent(" "));
            Texture2D texture2D = AssetPreview.GetAssetPreview(myMapGrid.gridPrefab);
            GUI.DrawTexture(textureRect, texture2D);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.PropertyField(gridPrefab);
        }
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
    }
}