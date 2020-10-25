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
    Odds odds;
    [SerializeField]
    float spawnHeight, spawnableWidth;

    Transform[] currentWave;

    public Transform[] CurrentWave => currentWave;
    public int MaxBlockCount => blockGroups.Length;
    public int MaxValueOnBlock => cellSprites.Length;

    public void SpawnNewWave(int groupCount)
    {
        Transform[] newWave = new Transform[groupCount];    

        float distance = spawnableWidth / (groupCount + 1);

        for (int i = 0; i < groupCount; i++)
        {
            int n = odds.GetRandomBlock();

            Transform tr = Instantiate(blockGroups[n].GetRandomArrangement(), new Vector3((i + 1) * distance - spawnableWidth / 2, spawnHeight), Quaternion.identity).transform;
            newWave[i] = tr;

            foreach(Transform child in tr)
            {
                int value = odds.GetRandomValue();

                child.name = "" + value;
                child.GetComponent<SpriteRenderer>().sprite = cellSprites[value - 1];
            }
        }

        currentWave = newWave;
    }
}