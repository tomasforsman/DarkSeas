using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using DarkSeas.Gameplay.Hazards;

namespace DarkSeas.Editor
{
    public static class CreateIcePrefabs
    {
        private const string BasePath = "Assets/_DarkSeas/Prefabs/Resources/Hazards";

        [MenuItem("DarkSeas/Create Ice Prefabs")]
        public static void Create()
        {
            System.IO.Directory.CreateDirectory(BasePath);
            CreateIce("Ice_Small", 1, 8f, 1f);
            CreateIce("Ice_Medium", 2, 10f, 1.5f);
            CreateIce("Ice_Large", 3, 12f, 2f);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Created Ice prefabs under " + BasePath);
        }

        private static void CreateIce(string name, int size, float baseDamage, float scale)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.localScale = new Vector3(scale, scale, scale);
            var hazard = go.AddComponent<IceHazard>();
            // Set private fields via SerializedObject
            var so = new SerializedObject(hazard);
            so.FindProperty("_size").intValue = size;
            so.FindProperty("_baseDamage").intValue = Mathf.RoundToInt(baseDamage);
            so.ApplyModifiedProperties();

            var path = BasePath + "/" + name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(go, path);
            GameObject.DestroyImmediate(go);
        }
    }
}

