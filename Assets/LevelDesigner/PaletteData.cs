using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner
{
    [CreateAssetMenu(fileName = "PaletteData.asset", menuName = "Level Designer/Palette Data")]
    internal class PaletteData : ScriptableObject
    {
        private const string _dataPath =
            GeneralSettings.rootPath + "/Datas/PaletteData.asset";

        [SerializeField]
        private PaletteCollection _palette;

        public string[] categories
        {
            get {
                var keys = _palette.Keys;
                var strArray = new string[keys.Count];
                keys.CopyTo(strArray, 0);
                return strArray;
            }
        }

        public PaletteData()
        {
            _palette = new PaletteCollection();
        }

        public GameObject[] GetItemsInCategory(string category)
        {
            if (!_palette.ContainsKey(category))
                throw new KeyNotFoundException($"{category} is not in the palette");

            return _palette[category].ToArray();
        }

        public static PaletteData GetData()
        {
            var data = AssetDatabase.LoadAssetAtPath<PaletteData>(_dataPath);
            if (data == null) {
                data = CreateInstance<PaletteData>();
                AssetDatabase.CreateAsset(data, _dataPath);
                AssetDatabase.SaveAssets();
            }

            return data;
        }

        public static SerializedObject GetSerializedObject()
        {
            return new SerializedObject(GetData());
        }
    }

    [Serializable]
    internal class PaletteCollection :
        SerializableDictionary<string, List<GameObject>, PaletteItems>
    {}

    [Serializable]
    internal class PaletteItems : SerializableDictionary.Storage<List<GameObject>>
    {}
}
