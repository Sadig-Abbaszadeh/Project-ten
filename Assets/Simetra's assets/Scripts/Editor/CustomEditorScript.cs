using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomEditorScript : Editor
{
    static Vector3? copiedPosition = null;
    static Quaternion? copiedRotation = null;
    static Vector3? copiedScale = null;

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetBundles()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath);

        AssetBundleManifest bundleManifest =
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,
            BuildAssetBundleOptions.ChunkBasedCompression,
            EditorUserBuildSettings.activeBuildTarget);

        if (bundleManifest != null)
            Debug.Log("Asset Bundles built successfully");
    }

    #region Mesh Saving Functionality
    [MenuItem("S_Tools/Save mesh as new")]
    static void SaveMeshAsNew()
    {
        bool hasCopiedAnyMesh = false;
        string folderName = "/Saved Assets"; // with forward slash

        Directory.CreateDirectory(Application.dataPath + folderName);

        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<MeshFilter>() == null)
                continue;
            
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            Mesh newMesh = new Mesh();

            newMesh.vertices = mesh.vertices;
            newMesh.triangles = mesh.triangles;
            newMesh.uv = mesh.uv;
            newMesh.normals = mesh.normals;
            newMesh.colors = mesh.colors;
            newMesh.tangents = mesh.tangents;

            string assetPath;

            do
            {
                assetPath = folderName + "/Mesh" + Random.Range(1000, 10000) + ".asset";
            } while (File.Exists(Application.dataPath + assetPath));

            AssetDatabase.CreateAsset(newMesh, "Assets" + assetPath);
            hasCopiedAnyMesh = true;
        }

        if (hasCopiedAnyMesh)
            Debug.Log("Saved mesh(es) at Assets" + folderName);
        else
            Debug.Log("No mesh copied");
    }

    [MenuItem("S_Tools/Save mesh as new", true)]
    static bool SaveMeshAsNewValidation()
    {
        return Selection.gameObjects.Length > 0;
    }
    #endregion

    #region Copy/paste position, rotation, scale
    // copying stuff
    // position
    [MenuItem("S_Tools/Copy Position")]
    static void CopyPosition()
    {
        copiedPosition = Selection.activeGameObject.transform.position;
    }

    // rotation
    [MenuItem("S_Tools/Copy Rotation")]
    static void CopyRotation()
    {
        copiedRotation = Selection.activeGameObject.transform.rotation;
    }

    // scale 
    [MenuItem("S_Tools/Copy Scale")]
    static void CopyScale()
    {
        copiedScale = Selection.activeGameObject.transform.lossyScale;
    }

    // validate function
    [MenuItem("S_Tools/Copy Position", true), MenuItem("S_Tools/Copy Rotation", true), MenuItem("S_Tools/Copy Scale", true)]
    static bool CopyingValidation()
    {
        return (Selection.gameObjects.Length == 1);
    }

    // pasting stuff
    // position
    [MenuItem("S_Tools/Paste Position")]
    static void PastePosition()
    {
        if(copiedPosition == null)
        {
            Debug.Log("No position is copied!");
            return;
        }

        Undo.RecordObjects(Selection.transforms, "pasting positions");

        foreach(Transform t in Selection.transforms)
            t.position = (Vector3)copiedPosition;
    }

    // rotation
    [MenuItem("S_Tools/Paste Rotation")]
    static void PasteRotation()
    {
        if(copiedRotation == null)
        {
            Debug.Log("No rotation copied!");
            return;
        }

        Undo.RecordObjects(Selection.transforms, "pasting rotations");
        
        foreach (Transform t in Selection.transforms)
            t.rotation = (Quaternion)copiedRotation;
    }

    // scale
    [MenuItem("S_Tools/Paste Scale")]
    static void PasteScale()
    {
        if(copiedScale == null)
        {
            Debug.Log("No scale copied");
            return;
        }

        Undo.RecordObjects(Selection.transforms, "pasting global scale");

        foreach (Transform t in Selection.transforms)
        {
            Transform parent = t.parent;
            int siblingIndex = t.GetSiblingIndex();

            t.SetParent(null);
            t.localScale = (Vector3)copiedScale;

            t.SetParent(parent);
            t.SetSiblingIndex(siblingIndex);
        }
    }

    // validate function
    [MenuItem("S_Tools/Paste Position", true), MenuItem("S_Tools/Paste Rotation", true), MenuItem("S_Tools/Paste Scale", true)]
    static bool PastingValidation()
    {
        return (Selection.transforms.Length > 0);
    }
    #endregion
}