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

    public void UpdateValues(CoordPair coordPair, int value)
    {
        if (value == 0)
            return;

        if ((columnSum[coordPair.x] += value) == maxCellValue + 1)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                int cellValue = gridManager.GetCellValue(coordPair.x, y);

                gridManager.ClearCell(coordPair.x, y);

                UpdateValues(new CoordPair(coordPair.x, y), -cellValue);
            }
        }

        if ((rowSum[coordPair.y] += value) == maxCellValue + 1)
        {
            for (int x = 0; x < gridManager.Width; x++)
            {
                int cellValue = gridManager.GetCellValue(x, coordPair.y);

                gridManager.ClearCell(x, coordPair.y);

                UpdateValues(new CoordPair(x, coordPair.y), -cellValue);
            }
        }

        lineSum.UpdateColumnAndRowText(true, coordPair.x, columnSum[coordPair.x]);
        lineSum.UpdateColumnAndRowText(false, coordPair.y, rowSum[coordPair.y]);
    }

    public void SettleComplete()
    {
        settledGroups++;
        if(settledGroups == blockSpawnAmount)
        {
            settledGroups = 0;
            SpawnNewWave();
        }

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (!blockSpawner.CurrentWave.All(gridOps.CanBePlaced))
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