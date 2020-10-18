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

    public void FillTheCell(GameObject gameObject)
    {
        cellObject = gameObject;
        cellValue = Convert.ToInt32(gameObject.name);
    }

    public void ClearCell()
    {
        cellObject = null;
        cellValue = 0;
    }
}
