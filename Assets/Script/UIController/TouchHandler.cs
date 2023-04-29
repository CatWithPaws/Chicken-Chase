using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchHandler : MonoBehaviour
{
    public  System.Action<Vector2> OnTouchMove;
    
    private Vector2 startPosition;
    private float minDistanceToMove = 1f;

    private Vector2 moveVector = Vector3.zero;
    [SerializeField] private bool isTouchMoved = false;

    private int prevTouchCount;
    public void Update()
    {
        if(Input.touchCount > 0 && !isTouchMoved)
        {
			Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) 
            {
                startPosition = touch.position; 
            }
            else if (touch.phase == TouchPhase.Moved)
            {

                if (Vector2.Distance(startPosition, touch.position) > minDistanceToMove)
                {
                    isTouchMoved = true;
                    moveVector = (touch.position - startPosition).normalized;
                    OnTouchMove?.Invoke(moveVector);

                    print(moveVector);

                    ResetMoveVector();
                    ResetStartPosition();
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                print("ended");
                isTouchMoved = false;
            }
		}
        if(prevTouchCount != Input.touchCount && prevTouchCount == 1)
        {
            isTouchMoved = false;
        }
        prevTouchCount = Input.touchCount;
    }

    private void ResetStartPosition()
    {
        startPosition = Vector2.zero;
    }

    private void ResetMoveVector()
    {
        moveVector = Vector2.zero;
    }
}
