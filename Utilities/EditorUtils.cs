#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VTLTools
{
    public static class EditorUtils
    {
        private static Dictionary<System.Type, List<string>> _cachedImplementations = new Dictionary<System.Type, List<string>>();

        /// <summary>
        /// Lấy danh sách tên của tất cả các class (non-abstract)
        /// implement một interface generic T.
        /// </summary>
        public static List<string> GetImplementations<T>()
        {
            System.Type interfaceType = typeof(T);

            if (_cachedImplementations.ContainsKey(interfaceType))
            {
                return _cachedImplementations[interfaceType];
            }

            var types = TypeCache.GetTypesDerivedFrom(interfaceType);

            var results = types
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Select(t => t.Name)
                .ToList();
            _cachedImplementations[interfaceType] = results;

            return results;
        }

        public static int GetHash(this GameObject prefab)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            return path.GetHashCode();
        }

        public static T GetAsset<T>(string name = "") where T : Object
        {
            string[] assets = AssetDatabase.FindAssets((string.IsNullOrEmpty(name) ? "" : name + " ") + "t:" + typeof(T).Name);
            if (assets.Length > 0)
            {
                return (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(T));
            }
            return null;
        }
    }
}
#endif