/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 22/08/2021
 |  Description: To acquire players input, which is the mouse position on the ground (PC version)
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IInputController
{
    // Create a Delegate (= Publisher) => Send notification to its all subscribers
    private Action<Vector3> OnPointerDownHandler;
    private Action OnPointerUpHandler;
    private Action<Vector3> OnPointerChangeHandler;

    private LayerMask mouseInputMask;

    // Get Layer Mask from Inspector  and set to Input Controller
    LayerMask IInputController.MouseInputMask { get => mouseInputMask; set => mouseInputMask = value; }

    void Update()
    {
        GetPointerPosition();
    }

    /// <summary>
    /// Get Mouse Position On the Layer (Ground)
    /// </summary>
    private void GetPointerPosition()
    {
        // To Check if the player has clicked on the ground
        //      First,  check if the left mouse button has been down
        //      And the pointer is over a gameobejct but not over an UI gameobject
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            CallActionOnPointer((position) => OnPointerDownHandler?.Invoke(position));
        }
        if (Input.GetMouseButton(0))
        {
            CallActionOnPointer((position) => OnPointerChangeHandler?.Invoke(position));
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnPointerUpHandler?.Invoke();
        }
    }

    private void CallActionOnPointer(Action<Vector3> action)
    {
        Vector3? position = GetMousePosition();
        if (position.HasValue)
        {
            action(position.Value);
        }
    }

    private Vector3? GetMousePosition()
    {
        // Create a ray from the screen point to ray passing its mouse position
        // Troubleshooting: Ensure the camera is tagged to MainCamera
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3? position = null;
        // Check if the ray intersect the ground by Shooting array from the camera towarding the point where the player have clicked
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mouseInputMask))
        {
            // If it crosses the ground (the ray intersects with a collider), it will shows where the point that the player have hit
            position = hit.point - transform.position;
            // ------ Debug ------
            //Debug.Log(position);
        }

        return position;
    }

    #region Assign Listeners

    public void AddListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        OnPointerDownHandler += listener;
    }

    public void RemoveListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        OnPointerDownHandler -= listener;
    }

    public void AddListenerOnPointerUpEvent(Action listener)
    {
        OnPointerUpHandler += listener;
    }

    public void RemoveListenerOnPointerUpEvent(Action listener)
    {
        OnPointerUpHandler -= listener;
    }

    public void AddListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        OnPointerChangeHandler += listener;
    }

    public void RemoveListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        OnPointerChangeHandler -= listener;
    }
    #endregion
}

