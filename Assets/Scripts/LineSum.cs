using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimetraCustomLib.GeneralUtils;

public class LineSum : MonoBehaviour
{
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    float fontSize;

    private void Start()
    {
        Transform lineValueParent = (new GameObject("Line Values")).transform;

        for (int x = 0; x < gridManager.Width; x++)
            WorldObjects.CreateWorldText("0", lineValueParent, gridManager.GetCellPosition(x, gridManager.Height - 1) + Vector3.up * gridManager.cellSize, fontSize, Color.white, TMPro.TextAlignmentOptions.Center);
        for (int y = 0; y < gridManager.Height; y++)
            WorldObjects.CreateWorldText("0", lineValueParent, gridManager.GetCellPosition(0, y) - Vector3.right * gridManager.cellSize, fontSize, Color.white, TMPro.TextAlignmentOptions.Center);
    }
}
