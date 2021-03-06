﻿using UnityEngine;
using TMPro;
using SimetraCustomLib.GeneralUtils;

public class LineSum : MonoBehaviour
{
    [SerializeField]
    GameMaster gameMaster;
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    float fontSize;

    TextMeshPro[] columnTexts, rowTexts;

    public void Spawn()
    {
        Transform lineValueParent = (new GameObject("Line Values")).transform;
        lineValueParent.position = Vector3.forward * 5;

        columnTexts = new TextMeshPro[gridManager.Width];
        rowTexts = new TextMeshPro[gridManager.Height];

        // columns
        for (int x = 0; x < gridManager.Width; x++)
            columnTexts[x] = WorldObjects.CreateWorldText("0", lineValueParent, gridManager.GetCellPosition(x, gridManager.Height - 1) + Vector3.up * gridManager.cellSize, fontSize, Color.white, TextAlignmentOptions.Center);
        // rows
        for (int y = 0; y < gridManager.Height; y++)
            rowTexts[y] = WorldObjects.CreateWorldText("0", lineValueParent, gridManager.GetCellPosition(0, y) - Vector3.right * gridManager.cellSize, fontSize, Color.white, TextAlignmentOptions.Center);
    }

    public void UpdateColumnAndRowText(bool column, int index, int value)
    {
        TextMeshPro text = column ? columnTexts[index] : rowTexts[index];

        text.text = ValueToString(value);
    }

    private string ValueToString(int value) => value <= gameMaster.MaxCellValue ? "" + value : "+";
}
