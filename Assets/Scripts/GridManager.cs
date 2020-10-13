using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimetraCustomLib.GeneralUtils;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    int width, height;
    [SerializeField]
    float cellSize;
    [SerializeField]
    Vector2 gridCenter;
    [SerializeField]
    Sprite[] cellSprites;
    [SerializeField]
    GameObject cellPrefab;

    GenericGrid<Cell> grid;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;

    private void Start()
    {
        grid = new GenericGrid<Cell>(width, height, cellSize, gridCenter, GridPivot.Center, false, (int x, int y) => new Cell());

        Transform gridParent = (new GameObject("Grid")).transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform cellWorldObject = Instantiate(cellPrefab).transform;
                cellWorldObject.position = grid.GridToWorldPosition(x, y);
                cellWorldObject.SetParent(gridParent);

                grid.GetCellObjectImmediate(x, y).SetSpriteRenderer(cellWorldObject.GetComponent<SpriteRenderer>());
            }
        }
    }

    public Vector3 GetCellPosition(int x, int y) => grid.GridToWorldPosition(x, y);
}
