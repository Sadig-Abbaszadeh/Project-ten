using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odds : MonoBehaviour
{
    [SerializeField]
    BlockSpawner blockSpawner;
    [SerializeField]
    int scoreForDiffChange = 100, maxChangeTimes = 20;
    [SerializeField]
    float changePerLevel = .5f;

    float[] blockOdds, valueOdds;

    private void Awake()
    {
        blockOdds = new float[blockSpawner.MaxBlockCount];
        valueOdds = new float[blockSpawner.MaxValueOnBlock];

        UpdateOdds(ref blockOdds, 0);
        UpdateOdds(ref valueOdds, 0);
    }

    private void UpdateOdds(ref float[] percentages, int level)
    {
        int center = percentages.Length / 2;
        float totalOffset = 0;
        float initial = 100f / percentages.Length;

        for (int i = 0; i < center; i++)
        {
            float offset = (maxChangeTimes - level) * changePerLevel;

            percentages[i] = initial + offset;
            totalOffset += offset;
        }

        Debug.Log(totalOffset);
        float individualDifference = totalOffset / (percentages.Length - center);

        for (int i = center; i < percentages.Length; i++)
        {
            percentages[i] = initial - individualDifference;
        }
    }

    private int GetRandom(float[] percentages)
    {
        int value = Random.Range(1, 101);
        float threshold = 0;

        for(int i = 0; i < percentages.Length; i++)
        {
            threshold += percentages[i];

            if (value <= threshold)
                return i;
        }

        return -1;
    }

    public int GetRandomBlock() => GetRandom(blockOdds);

    public int GetRandomValue() => GetRandom(valueOdds);

    public void TryChangeOdds(int score)
    {
        int level = score / scoreForDiffChange;

        if (level <= maxChangeTimes)
        {
            UpdateOdds(ref blockOdds, level);
            UpdateOdds(ref valueOdds, level);
        }
    }
}
