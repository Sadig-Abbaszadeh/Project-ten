using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    BlockSpawner blockSpawner;
    [SerializeField]
    LineSum lineSum;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TrySettleGroup(Transform groupParent)
    {
        int snapCount = 0, childCount = groupParent.childCount;
        int[] xValues = new int[childCount], yValues = new int[childCount];

        for (int i = 0; i < childCount; i++)
        {
            int x, y;

            if (gridManager.WorldToGridPosition(groupParent.GetChild(i).position, out x, out y) && gridManager.GetCellValue(x, y) == 0)
            {
                xValues[i] = x;
                yValues[i] = y;
                snapCount++;
            }
        }

        if (snapCount == childCount)
            SettleGroup(groupParent, xValues, yValues);
    }

    private void SettleGroup(Transform groupParent, int[] xValues, int[] yValues)
    {
        for(int i = 0; i < groupParent.childCount; i++)
        {
            Transform child = groupParent.GetChild(i);

            child.position = gridManager.GetCellPosition(xValues[i], yValues[i]);
            gridManager.UpdateCell(child.gameObject, xValues[i], yValues[i]);
        }

        groupParent.DetachChildren();
        Destroy(groupParent.gameObject);
    }
}