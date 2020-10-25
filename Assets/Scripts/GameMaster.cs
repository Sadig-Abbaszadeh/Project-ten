using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
    Odds odds;
    [SerializeField]
    VisualEffectsController effectsController;

    [SerializeField]
    int blockSpawnAmount, maxCellValue, scorePerLine, comboScoreIncrease, score;

    int[] columnSum, rowSum;

    int settledGroups, linesCleared;

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

        lineSum.UpdateColumnAndRowText(true, coordPair.x, columnSum[coordPair.x]);
        lineSum.UpdateColumnAndRowText(false, coordPair.y, rowSum[coordPair.y]);
    }

    public IEnumerator CheckLineClear() //int
    {
        int lines = 0;

        List<int> rows = GetFullLines(rowSum, ref lines);
        List<int> columns = GetFullLines(columnSum, ref lines);

        foreach (int row in rows)
        {
            rowSum[row] = 0;
            lineSum.UpdateColumnAndRowText(false, row, 0);

            for (int x = 0; x < gridManager.Width; x++)
            {
                columnSum[x] -= gridManager.GetCellValue(x, row);
                lineSum.UpdateColumnAndRowText(true, x, columnSum[x]);
                gridManager.ClearCell(x, row);
            }
        }

        foreach (int column in columns)
        {
            columnSum[column] = 0;
            lineSum.UpdateColumnAndRowText(true, column, 0);

            for (int y = 0; y < gridManager.Height; y++)
            {
                rowSum[y] -= gridManager.GetCellValue(column, y);
                lineSum.UpdateColumnAndRowText(false, y, rowSum[y]);
                gridManager.ClearCell(column, y);
            }
        }

        Debug.Log(lines);
        linesCleared += lines;

        if (lines != 0)
        {
            IEnumerator next = effectsController.LineClearAnim(rows, columns);

            while (next.MoveNext())
                yield return null;

            next = CheckLineClear();

            while (next.MoveNext())
                yield return null;
        }

        yield break;
    }

    private List<int> GetFullLines(int[] sums, ref int linesCleared)
    {
        List<int> lines = new List<int>();

        for (int i = 0; i < sums.Length; i++)
        {
            if (sums[i] == maxCellValue + 1)
            {
                lines.Add(i);
                linesCleared++;
            }
        }

        return lines;
    }

    public void SettleComplete(/*int linesCleared*/)
    {
        score += (2 * scorePerLine + (linesCleared - 1) * comboScoreIncrease) / 2 * linesCleared;
        linesCleared = 0;

        odds.TryChangeOdds(score);

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