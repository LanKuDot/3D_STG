using UnityEditor;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        UpdatePoolItemName();
        serializedObject.ApplyModifiedProperties();
    }

    private void UpdatePoolItemName()
    {
        var poolItemsProp = serializedObject.FindProperty("_poolItems");
        var numOfItems = poolItemsProp.arraySize;

        for (var i = 0; i < numOfItems; ++i) {
            var itemProp = poolItemsProp.GetArrayElementAtIndex(i);
            var nameProp = itemProp.FindPropertyRelative("name");
            var obj =
                itemProp.FindPropertyRelative("objectToPool").objectReferenceValue;

            if (obj == null)
                nameProp.stringValue = "-- Unassigned --";
            else
                nameProp.stringValue = obj.name;
        }
    }
}
