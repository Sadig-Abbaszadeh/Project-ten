using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroupController : MonoBehaviour
{
    Camera cam;

    Vector3 initialPosition;

    static float moveSpeed = 20, returnSpeed = 50;

    bool followPointer = false;

    private void Start()
    {
        cam = Camera.main;
        
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
                GameMaster.Instance.TrySettleGroup(transform);
                followPointer = false;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, returnSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        followPointer = true;
    }
}
