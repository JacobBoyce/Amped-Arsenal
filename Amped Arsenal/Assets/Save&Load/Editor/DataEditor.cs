using UnityEngine;
using UnityEditor;
using DataManagement;

// --- By VEOdev ---
// You are not allow to resell or show this asset.
// This asset is also under the unity asset store licence.

namespace VEO
{
    public class DataEditor
    {
        [MenuItem("Data/Configuration")]
        public static void Configuration()
        {
            try
            {
                DataConfiguration config = Resources.Load<DataConfiguration>("DataConfig");

                if (config == null)
                {
                    config = ScriptableObject.CreateInstance<DataConfiguration>();

                    string path = "Assets/Save&Load/Resources/DataConfig.asset";

                    AssetDatabase.CreateAsset(config, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = config;
            }
            catch
            {
                Debug.LogWarning("Couldn't create the data config asset, maybe you moved or renamed the save & load folder");
                throw;
            }

        }
    }
}