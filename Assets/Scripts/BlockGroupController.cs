using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroupController : MonoBehaviour
{
    Camera cam;
    
    GridManager gridManager;

    Vector3 initialPosition;

    static float moveSpeed = 20, returnSpeed = 50;

    bool followPointer = false;

    private void Start()
    {
        cam = Camera.main;
        gridManager = FindObjectOfType<GridManager>();
        
        initialPosition = transform.position;
    }

    private void Update()
    {
        Vector2 targetPosition = initialPosition;

        if (followPointer)
        {
#if UNITY_EDITOR
            if(Input.GetMouseButton(0))
            {
                targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            }
//#endif
#elif UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                targetPosition = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
#endif
            else
            {
                int snapCount = 0;

                foreach(Transform child in transform)
                {
                    int x, y;

                    if (gridManager.WorldToGridPosition(child.position, out x, out y))
                        snapCount++;
                }

                if (snapCount == transform.childCount)
                    SettleGroup();

                followPointer = false;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, returnSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        followPointer = true;
    }

    private void SettleGroup()
    {
        foreach(Transform child in transform)
        {
            int x, y;

            gridManager.WorldToGridPosition(child.position, out x, out y);
            child.position = gridManager.GetCellPosition(x, y);
        }

        transform.DetachChildren();
        Destroy(gameObject);
    }
}
