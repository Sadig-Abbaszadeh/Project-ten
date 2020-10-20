using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    BlockSpawner blockSpawner;
    [SerializeField]
    LineSum lineSum;
    [SerializeField]
    GridOps gridOps;

    [SerializeField]
    int blockSpawnAmount, maxCellValue;

    int[] columnSum, rowSum;

    int settledGroups = 0;

    public int MaxCellValue => maxCellValue;

    private void Start()
    {
        gridManager.SpawnGrid();
        lineSum.Spawn();
        SpawnNewWave();

        columnSum = new int[gridManager.Width];
        rowSum = new int[gridManager.Height];
    }

    public void UpdateSums(CoordPair coordPair, int value)
    {
        columnSum[coordPair.x] += value;
        rowSum[coordPair.y] += value;
    }

    public void CheckLineClear(CoordPair coordPair, int value = 0)
    {
        int column = columnSum[coordPair.x] += value;
        int row = rowSum[coordPair.y] += value;

        if (column == maxCellValue + 1)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                //value = 0;
                int cellValue = gridManager.GetCellValue(coordPair.x, y);

                gridManager.ClearCell(coordPair.x, y);

                if (cellValue != 0)
                    CheckLineClear(new CoordPair(coordPair.x, y), -cellValue);
            }
        }

        if (row == maxCellValue + 1)
        {
            for (int x = 0; x < gridManager.Width; x++)
            {
                int cellValue = gridManager.GetCellValue(x, coordPair.y);

                gridManager.ClearCell(x, coordPair.y);

                if (cellValue != 0)
                    CheckLineClear(new CoordPair(x, coordPair.y), -cellValue);
            }
        }

        lineSum.UpdateColumnAndRowText(true, coordPair.x, columnSum[coordPair.x]);
        lineSum.UpdateColumnAndRowText(false, coordPair.y, rowSum[coordPair.y]);
    }

    public void SettleComplete()
    {
        settledGroups++;
        if (settledGroups == blockSpawnAmount)
        {
            settledGroups = 0;
            SpawnNewWave();
        }

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (!blockSpawner.CurrentWave.Any(gridOps.CanBePlaced))
            GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Game over");
    }

    private void SpawnNewWave()
    {
        blockSpawner.SpawnNewWave(blockSpawnAmount);
    }
}