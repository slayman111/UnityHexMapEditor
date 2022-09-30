using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedInputs : MonoBehaviour
{
    public float minSwipeDistance;

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, isDragging = false;
    private Vector2 startTouch, swipeDelta;

    public bool Tap { get => tap; }
    public Vector2 SwipeDelta { get => swipeDelta; }
    public bool SwipeLeft { get => swipeLeft; }
    public bool SwipeRight { get => swipeRight; }
    public bool SwipeUp { get => swipeUp; }
    public bool SwipeDown { get => swipeDown; }

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = isDragging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) Reset();
        #endregion Standalone Inputs

        #region Mobile Inputs
        if (Input.touchCount != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                Reset();
        }
        #endregion Mobile Inputs

        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touchCount != 0) swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0)) swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }
        if (swipeDelta.magnitude > minSwipeDistance)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x < 0) swipeLeft = true;
                else swipeRight = true;
            }
            else
            {
                if (swipeDelta.y < 0) swipeDown = true;
                else swipeUp = true;
            }
        }
    }

    private void Reset()
    {
        isDragging = false;
        startTouch = swipeDelta = Vector2.zero;
    }
}