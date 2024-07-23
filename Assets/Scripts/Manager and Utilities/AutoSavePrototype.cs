using UnityEditor;
#if UNITY_EDITOR

using UnityEditor.SceneManagement;

#endif
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]

#endif
public class AutoSavePrototype
{
#if UNITY_EDITOR
    static AutoSavePrototype()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto saving... ---> Done!");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
