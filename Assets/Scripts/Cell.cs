using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cell
{
    private int cellValue;
    private GameObject cellObject;

    public int CellValue => cellValue;
    public GameObject CellObject => cellObject;

    public void FillTheCell(GameObject cellObject, int cellValue)
    {
        this.cellObject = cellObject;
        this.cellValue = cellValue;
    }

    public void ClearCell()
    {
        UnityEngine.Object.Destroy(cellObject);
        cellObject = null;
        cellValue = 0;
    }
}
