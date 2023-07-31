using System.IO;
using UnityEditor;

//[CustomEditor(typeof(CreateBaseFolders))]
public class CreateBaseFolders_Editor : EditorWindow
{
    //private SerializedProperty floorFolderName;
    //private void OnEnable()
    //{
    //    floorGenerating = serializedObject.FindProperty("floorGenerating");
    //}
    //public override void OnInspectorGUI()
    //{
    //serializedObject.Update();
    //Floor_Generator floor_Generator = (Floor_Generator)target;
    //EditorGUILayout.PropertyField(allRoomPrefabs);
    //    if (GUILayout.Button("Choose Floor Prefab Folder", GUILayout.Height(20)))
    //    {
    //    }
    //serializedObject.ApplyModifiedProperties();
    //}
    /// <summary>
    /// Create base folders.
    /// </summary>
    [MenuItem("Tools/Create Base Folders")]
    public static void CreateBaseFolders()
    {
        #region Prefabs
        // Prefabs folder control
        if (!Directory.Exists("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        // Prefabs/Menu folder control
        if (!Directory.Exists("Assets/Prefabs/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Menu");
        }
        // Prefabs/Game folder control
        if (!Directory.Exists("Assets/Prefabs/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Game");
        }
        // Prefabs/Genel folder control
        if (!Directory.Exists("Assets/Prefabs/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Genel");
        }
        #endregion

        #region Scriptable
        // Scriptable folder control
        if (!Directory.Exists("Assets/Scriptable"))
        {
            AssetDatabase.CreateFolder("Assets", "Scriptable");
        }
        // Scriptable/Pools folder control
        if (!Directory.Exists("Assets/Scriptable/Pools"))
        {
            AssetDatabase.CreateFolder("Assets/Scriptable", "Pools");
        }
        // Scriptable/Pools/Menu folder control
        if (!Directory.Exists("Assets/Scriptable/Pools/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Scriptable/Pools", "Menu");
        }
        // Scriptable/Pools/Game folder control
        if (!Directory.Exists("Assets/Scriptable/Pools/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Scriptable/Pools", "Game");
        }
        // Scriptable/Pools/Genel folder control
        if (!Directory.Exists("Assets/Scriptable/Pools/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Scriptable/Pools", "Genel");
        }
        #endregion

        #region Scripts
        // Scripts folder control
        if (!Directory.Exists("Assets/Scripts"))
        {
            AssetDatabase.CreateFolder("Assets", "Scripts");
        }
        // Scripts/PreLoad folder control
        if (!Directory.Exists("Assets/Scripts/PreLoad"))
        {
            AssetDatabase.CreateFolder("Assets/Scripts", "PreLoad");
        }
        // Scripts/Menu folder control
        if (!Directory.Exists("Assets/Scripts/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Scripts", "Menu");
        }
        // Scripts/Game folder control
        if (!Directory.Exists("Assets/Scripts/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Scripts", "Game");
        }
        // Scripts/Genel folder control
        if (!Directory.Exists("Assets/Scripts/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Scripts", "Genel");
        }
        #endregion

        #region Sprite
        // Sprite folder control
        if (!Directory.Exists("Assets/Sprite"))
        {
            AssetDatabase.CreateFolder("Assets", "Sprite");
        }
        // Sprite/Menu folder control
        if (!Directory.Exists("Assets/Sprite/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Sprite", "Menu");
        }
        // Sprite/Game folder control
        if (!Directory.Exists("Assets/Sprite/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Sprite", "Game");
        }
        // Sprite/Genel folder control
        if (!Directory.Exists("Assets/Sprite/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Sprite", "Genel");
        }
        #endregion

        #region Animations
        // Animations folder control
        if (!Directory.Exists("Assets/Animations"))
        {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }
        #endregion

        #region Materials
        // Materials folder control
        if (!Directory.Exists("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        #endregion

        #region Resources
        // Resources folder control
        if (!Directory.Exists("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        // Resources/Prefab folder control
        if (!Directory.Exists("Assets/Resources/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Prefabs");
        }
        // Resources/Prefabs/Menu folder control
        if (!Directory.Exists("Assets/Resources/Prefabs/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Menu");
        }
        // Resources/Prefabs/Game folder control
        if (!Directory.Exists("Assets/Resources/Prefabs/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Game");
        }
        // Resources/Prefabs/Genel folder control
        if (!Directory.Exists("Assets/Resources/Prefabs/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Genel");
        }

        // Resources/Scriptable folder control
        if (!Directory.Exists("Assets/Resources/Scriptable"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Scriptable");
        }
        // Resources/Scriptable/Menu folder control
        if (!Directory.Exists("Assets/Resources/Scriptable/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Scriptable", "Menu");
        }
        // Resources/Scriptable/Game folder control
        if (!Directory.Exists("Assets/Resources/Scriptable/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Scriptable", "Game");
        }
        // Resources/Scriptable/Genel folder control
        if (!Directory.Exists("Assets/Resources/Scriptable/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Scriptable", "Genel");
        }

        // Resources/Sprite folder control
        if (!Directory.Exists("Assets/Resources/Sprite"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Sprite");
        }
        // Resources/Sprite/Menu folder control
        if (!Directory.Exists("Assets/Resources/Sprite/Menu"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Sprite", "Menu");
        }
        // Resources/Sprite/Game folder control
        if (!Directory.Exists("Assets/Resources/Sprite/Game"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Sprite", "Game");
        }
        // Resources/Sprite/Genel folder control
        if (!Directory.Exists("Assets/Resources/Sprite/Genel"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Sprite", "Genel");
        }
        #endregion

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}