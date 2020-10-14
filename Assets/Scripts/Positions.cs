using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockGroup
{
    public GameObject[] groupArrangement;

    public GameObject GetRandomArrangement() => groupArrangement[Random.Range(0, groupArrangement.Length)];
}