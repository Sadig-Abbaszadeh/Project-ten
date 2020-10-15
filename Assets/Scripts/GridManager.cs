using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimetraCustomLib.GeneralUtils;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    int width, height;
    [SerializeField]
    float distanceBetweencells;
    [SerializeField]
    Vector2 gridCenter;
    [SerializeField]
    GameObject cellPrefab;

    GenericGrid<int> grid;

    public int Width => width;
    public int Height => height;
    public float cellSize => cellPrefab.transform.localScale.x + distanceBetweencells;

    private void Start()
    {
        grid = new GenericGrid<int>(width, height, cellSize, gridCenter, GridPivot.Center, false, (int x, int y) => 0);

        Transform gridParent = (new GameObject("Grid")).transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform cellWorldObject = Instantiate(cellPrefab).transform;
                cellWorldObject.localPosition = grid.GridToWorldPosition(x, y);
                cellWorldObject.SetParent(gridParent);
            }
        }
    }

    public Vector3 GetCellPosition(int x, int y) => grid.GridToWorldPosition(x, y);
}
