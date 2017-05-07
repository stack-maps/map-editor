using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoSave
{
  static AutoSave()
  {
    EditorApplication.playmodeStateChanged = () =>
    {
      if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
      {
        if (EditorSceneManager.SaveOpenScenes()) {
          Debug.Log("Auto-save success!");
        }

        AssetDatabase.SaveAssets();
      }
    };
  }
}
