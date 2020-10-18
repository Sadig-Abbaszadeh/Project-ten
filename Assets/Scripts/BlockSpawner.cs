using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    BlockGroup[] blockGroups; // possible block groups with index + 1 blocks
    [SerializeField]
    Sprite[] cellSprites;
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    int minCellValue, maxCellValue;
    [SerializeField]
    float spawnHeight, spawnableWidth;

    public void SpawnNewWave(int groupCount)
    {
        float distance = spawnableWidth / (groupCount + 1);

        for (int i = 0; i < groupCount; i++)
        {
            int n = Random.Range(0, blockGroups.Length);

            Transform tr = Instantiate(blockGroups[n].GetRandomArrangement(), new Vector3((i + 1) * distance - spawnableWidth / 2, spawnHeight), Quaternion.identity).transform;

            foreach(Transform child in tr)
            {
                int value = Random.Range(minCellValue, maxCellValue + 1);

                child.name = "" + value;
                child.GetComponent<SpriteRenderer>().sprite = cellSprites[value - 1];
            }
        }
    }
}