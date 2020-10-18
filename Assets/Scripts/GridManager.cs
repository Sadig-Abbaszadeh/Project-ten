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
    GridPivot gridPivot;
    [SerializeField]
    GameObject cellPrefab;

    GenericGrid<Cell> grid;

    public int Width => width;
    public int Height => height;
    public float cellSize => cellPrefab.transform.localScale.x + distanceBetweencells;

    private void Start()
    {
        SpawnGrid();
    }

    private void SpawnGrid()
    {
        grid = new GenericGrid<Cell>(width, height, cellSize, gridCenter, gridPivot, false, (int x, int y) => new Cell());

        Transform gridParent = (new GameObject("Grid")).transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform cellWorldObject = Instantiate(cellPrefab).transform;
                cellWorldObject.localPosition = new Vector3(GetCellPosition(x, y).x, GetCellPosition(x, y).y, 5);
                cellWorldObject.SetParent(gridParent);
            }
        }
    }

    public Vector3 GetCellPosition(int x, int y) => grid.GridToWorldPosition(x, y);
    public bool WorldToGridPosition(Vector3 worldPos, out int x, out int y) => grid.WorldToGridPosition(worldPos, out x, out y);

    public void UpdateCell(GameObject obj, int x, int y)
    {
        if (obj == null)
            grid.GetCellObjectImmediate(x, y).ClearCell();
        else
            grid.GetCellObjectImmediate(x, y).FillTheCell(obj);
    }

    public int GetCellValue(int x, int y)
    {
        Cell cell = grid.GetCellObject(x, y);
        return cell == null ? -1 : cell.CellValue;
    }
}