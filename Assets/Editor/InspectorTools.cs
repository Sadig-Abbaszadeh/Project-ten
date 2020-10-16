using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabEditor))]
public class PrefabEditorInspector : Editor
{
    float cellSize;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update prefab sizes"))
        {
            GameObject[] prefabs = (target as PrefabEditor).Prefabs;
            cellSize = (target as PrefabEditor).Grid_Manager.cellSize;

            UpdatePrefabs(UpdatePrefabSize, prefabs);

            Debug.Log("Updated sizes of prefabs");
        }
    }

    private void UpdatePrefabSize(GameObject prefab)
    {
        foreach (Transform child in prefab.transform)
        {
            child.localScale = Vector2.one * cellSize;
            child.localPosition = new Vector3(cellSize / 2 * Mathf.Sign(child.position.x), cellSize / 2 * Mathf.Sign(child.position.y));
        }

        prefab.GetComponent<BoxCollider2D>().size = Vector2.one * cellSize * 2;
    }

    private void UpdatePrefabs(Action<GameObject> action, GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            string prefabPath = AssetDatabase.GetAssetPath(prefab);

            if (prefabPath == null || prefabPath.Equals(""))
                continue;

            using (EditPrefabAsset editPrefabAsset = new EditPrefabAsset(prefabPath))
            {
                // editing code goes here, eg: editPrefabAsset.prefabRoot....
                action(editPrefabAsset.prefabRoot);
            }
        }
    }
}

public class EditPrefabAsset : IDisposable
{
    private string prefabPath;
    public readonly GameObject prefabRoot;

    // ctor
    public EditPrefabAsset(string prefabPath)
    {
        this.prefabPath = prefabPath;
        prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
    }

    public void Dispose()
    {
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
    }
}