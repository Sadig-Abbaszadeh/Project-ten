using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject testObject;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int x, y;
            Vector2 position = cam.ScreenToWorldPoint(Input.mousePosition);
            testObject.transform.position = position;

            if (gridManager.WorldToGridPosition(position, out x, out y))
            {
                Debug.Log(x + "; " + y);
            }
        }
    }
}
