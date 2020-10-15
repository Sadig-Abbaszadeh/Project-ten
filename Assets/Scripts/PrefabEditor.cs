using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabEditor : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabs;
    [SerializeField]
    GridManager gridManager;

    public GameObject[] Prefabs => prefabs;
    public GridManager Grid_Manager => gridManager;
}