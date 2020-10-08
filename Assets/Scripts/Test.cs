using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimetraCustomLib.GeneralUtils;

public class Test : MonoBehaviour
{
    [SerializeField]
    int width, height;
    [SerializeField]
    float cellSize, lineNumberFontSize;
    [SerializeField]
    Vector2 gridCenter;
    [SerializeField]
    Sprite[] numberSprites;
    [SerializeField]
    GameObject cellExample;

    GenericGrid<Cell> grid;

    private void Start()
    {
        grid = new GenericGrid<Cell>(width, height, cellSize, gridCenter, GridPivot.Center, false, (int x, int y) => new Cell());

        Transform gridParent = (new GameObject("Grid")).transform;
        Transform lineValueParent = (new GameObject("Line Values")).transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform cellWorldObject = Instantiate(cellExample).transform;
                cellWorldObject.position = grid.GridToWorldPosition(x, y);
                cellWorldObject.SetParent(gridParent);

                grid.GetCellObjectImmediate(x, y).SetSpriteRenderer(cellWorldObject.GetComponent<SpriteRenderer>());
            }
        }

        for (int x = 0; x < width; x++)
            WorldObjects.CreateWorldText("0", lineValueParent, grid.GridToWorldPosition(x, height - 1) + Vector3.up * cellSize, lineNumberFontSize, Color.white, TMPro.TextAlignmentOptions.Center);
        for(int y = 0; y < height; y++)
            WorldObjects.CreateWorldText("0", lineValueParent, grid.GridToWorldPosition(0, y) - Vector3.right * cellSize, lineNumberFontSize, Color.white, TMPro.TextAlignmentOptions.Center);
    }
}
