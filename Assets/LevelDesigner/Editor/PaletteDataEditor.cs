using UnityEditor;

namespace LevelDesigner.Editor
{
    [CustomPropertyDrawer(typeof(PaletteCollection))]
    internal class PaletteCollectionPropertyDrawer :
        SerializableDictionaryPropertyDrawer
    {}

    [CustomPropertyDrawer(typeof(PaletteItems))]
    internal class PaletteItemsPropertyDrawer :
        SerializableDictionaryStoragePropertyDrawer
    {}
}
