using System.Collections;
using System.Collections.Generic;
using System;
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

    [SerializeField]
    int blockSpawnAmount;

    int[] columnSum, rowSum;

    int settledGroups = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gridManager.SpawnGrid();
        lineSum.Spawn();
        blockSpawner.SpawnNewWave(blockSpawnAmount);

        columnSum = new int[gridManager.Width];
        rowSum = new int[gridManager.Height];
    }

    public void TrySettleGroup(Transform groupParent)
    {
        int snapCount = 0, childCount = groupParent.childCount;
        CoordPair[] coordPairs = new CoordPair[groupParent.childCount];

        for (int i = 0; i < childCount; i++)
        {
            int x, y;

            if (gridManager.WorldToGridPosition(groupParent.GetChild(i).position, out x, out y) && gridManager.GetCellValue(x, y) == 0)
            {
                coordPairs[i] = new CoordPair(x, y);
                snapCount++;
            }
        }

        if (snapCount == childCount)
            SettleGroup(groupParent, coordPairs);
    }

    private void SettleGroup(Transform groupParent, CoordPair[] coordPairs)
    {
        Transform[] blocks = DetachReturnChildren(groupParent);

        for(int i = 0; i < blocks.Length; i++)
        {
            // set snap coordinates
            blocks[i].position = gridManager.GetCellPosition(coordPairs[i].x, coordPairs[i].y) + Vector3.forward * 4;

            // upgrade cell
            int value = Convert.ToInt32(blocks[i].name);
            gridManager.UpdateCell(coordPairs[i].x, coordPairs[i].y, blocks[i].gameObject, value);

            // update line sum
            columnSum[coordPairs[i].x] += value;
            rowSum[coordPairs[i].y] += value;

            // update text
            lineSum.UpdateColumnAndRowText(true, coordPairs[i].x, columnSum[coordPairs[i].x]);
            lineSum.UpdateColumnAndRowText(false, coordPairs[i].y, rowSum[coordPairs[i].y]);

            Debug.Log(value + ": " + coordPairs[i].x + "-" + coordPairs[i].y);
        }

        Destroy(groupParent.gameObject);

        settledGroups++;

        if(settledGroups == blockSpawnAmount)
        {
            settledGroups = 0;
            blockSpawner.SpawnNewWave(blockSpawnAmount);
        }
    }

    private Transform[] DetachReturnChildren(Transform parent)
    {
        int childCount = parent.childCount;
        Transform[] children = new Transform[childCount];

        for(int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(0);
            children[i] = child;
            child.SetParent(null);
        }

        return children;
    }
}