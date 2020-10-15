using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabEditor))]
public class PrefabEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Prefabs"))
        {
            GameObject[] prefabs = (target as PrefabEditor).Prefabs;
            float cellSize = (target as PrefabEditor).Grid_Manager.cellSize;
            Debug.Log(cellSize);

            foreach (GameObject prefab in prefabs)
            {
                string prefabPath = AssetDatabase.GetAssetPath(prefab);

                if (prefabPath == null || prefabPath.Equals(""))
                    continue;

                using (EditPrefabAsset editPrefabAsset = new EditPrefabAsset(prefabPath))
                {
                    // editing code goes here, eg: editPrefabAsset.prefabRoot....
                    foreach(Transform child in editPrefabAsset.prefabRoot.transform)
                    {
                        child.localPosition = new Vector3(cellSize / 2 * Mathf.Sign(child.position.x), cellSize / 2 * Mathf.Sign(child.position.y));
                    }

                    editPrefabAsset.prefabRoot.GetComponent<BoxCollider2D>().size = Vector2.one * cellSize * 2;
                }
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