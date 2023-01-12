/*======================================================*
|  Author: Yifan Song
|  Creation Date: 19/08/2021
|  Latest Modified Date: 22/08/2021
|  Description: To control camera movement
|  Bugs: N/A
*=======================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Create a field to set movement speed of camera's moving left/right/forward/back (default: 0.5f)
    public float movementSpeed = 0.05f;

    // Create a field to set zoom in/out speed (default: 10f)
    public float zoomSpeed = 10f;

    // Create a field to set rotation speed (default: 0.05f)
    public float rotateSpeed = 0.05f;

    /* Create fields to limit the movement
     * minWidth = 0-10f
     * maxWidth = groundWidth+10f
     * minHeight = 4f
     * maxHeight = groundWidth+10f
     * minLength = 0-10f
     * maxLength = groundWidth+10f
     */
    private float minWidth = -10f; // x
    private float maxWidth = 160f;

    private float minHeight = 4f; // y 
    private float maxHeight = 40f;

    private float minLength = -10f; // z
    private float maxLength = 110f;

    public bool isAvailable = true;

    // Fields to compare the mouse position for rotation
    private Vector2 currentPosition, newPosition;

    private void Update()
    {
        if (isAvailable)
        {
            MoveCamera();
            GetCameraRotation();
        }

    }

    public void SetCameraLimitation(float minWidth, float maxWidth, float minHeight, float maxHeight, float minLength, float maxLength)
    {
        this.minWidth = minWidth;
        this.maxWidth = maxWidth;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    // For further usage: leave it for now.
    public void SetPosition(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }


    #region CameraMovementCalculation
    public void MoveCamera()
    {
        Vector3 verticalMovement = CameraVerticalMovement();
        Vector3 horizontalMovement = CameraHorizontalMovement();

        // Add movements into one vector where the camera will move to
        Vector3 movement = verticalMovement + horizontalMovement;

        // Update position by adding the current camera position and movement position 
        transform.position += movement;
    }

    private Vector3 CameraVerticalMovement()
    {
        // Using mouse scrollwheel to zoom in/out
        float scrollSpeed = -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        //----------------Vertical Movement / Zoom In & Out-----------//
        scrollSpeed = LimitCameraVerticalPositionInsideRange(scrollSpeed); // Limit the camera vertical movement inside the range of map
        Vector3 verticalMovement = new Vector3(0, scrollSpeed, 0); // Create a new position to record the movement action
        return verticalMovement;
    }

    private Vector3 CameraHorizontalMovement()
    {
        // Using keyboard button (left/a, right/d) to move sideways
        float horizontalSpeed = movementSpeed * Input.GetAxis("Horizontal");

        // Using keyboard button (Up/w, Down/s) to move forward/backward
        float verticalSpeed = movementSpeed * Input.GetAxis("Vertical");

        //---------------Horizontal Movement-----------//
        Vector3 horizontalLeftRightMovement = horizontalSpeed * transform.right; // Moving sideways based on the camera's direction facing instead of the world space
        Vector3 forwardMove = transform.forward; // Moving forward/backward based on the camera's direction facing instead of the world space

        /* Bug "Moving camera into the ground" Solved: 
         *      Logic: 
         *          The original direction of camera is angled down, the regular transform.forward will push camera into the camera's facing direction. 
         *      Solution:
         *          Vector Projection. (More details are referred in Readme)
         */
        forwardMove.y = 0; // Setting y axis to be zero, then the camera won't be moving down anymore
        forwardMove.Normalize(); // Setting magnitude to be 1
        forwardMove *= verticalSpeed; // Calculating the forward/backward movement

        Vector3 horizontalMovement = horizontalLeftRightMovement + forwardMove;

        //Debug.Log(horizontalMovement);

        // Limit the camera horizontal movement inside the map
        horizontalMovement.x = LimitCameraHorizontalPositionInsideWidthRange(horizontalMovement.x);
        horizontalMovement.z = LimitCameraHorizontalPositionInsideLengthRange(horizontalMovement.z);
        return horizontalMovement;
    }

    #endregion

    #region LimitCameraMovement
    /* Bug "Moving camera out of range horizontally' Solved: 
     *      Logic: 
     *          The horizontal movement is based on the direction of camera facing instead of the world space.
     *          The direction can be changed by rotating the camera.
     *          Ensure that wherever the camera is facing, the camera cannot move right/left/forward/backward if it will be out of range.
     *      Solution: 
     *          Calculate all possible new positive where camera moves to.
     *          If the new position is out of range, then cancel this movement action which is set to be zero.
     *          
     *      Same logic applied to Bug "Moving camera out of range vertically"
     */

    private float LimitCameraHorizontalPositionInsideWidthRange(float movement)
    {
        if (transform.position.x >= maxWidth && movement > 0)
        {
            movement = 0;

        }
        if (transform.position.x <= minWidth && movement < 0)
        {
            movement = 0;
        }

        if ((transform.position.x + movement) > maxWidth)
        {
            movement = maxWidth - transform.position.x;
        }

        if ((transform.position.x + movement) < minWidth)
        {
            movement = minWidth - transform.position.x;
        }

        return movement;
    }

    private float LimitCameraHorizontalPositionInsideLengthRange(float movement)
    {
        if (transform.position.z >= maxLength && movement > 0)
        {
            movement = 0;
        }
        if (transform.position.z <= minLength && movement < 0)
        {
            movement = 0;
        }
        if ((transform.position.z + movement) > maxLength)
        {
            movement = maxLength - transform.position.z;
        }
        if ((transform.position.z + movement) < minLength)
        {
            movement = minLength - transform.position.z;
        }
        return movement;
    }

    private float LimitCameraVerticalPositionInsideRange(float scrollSpeed)
    {
        if ((transform.position.y >= maxHeight) && (scrollSpeed > 0))
        {
            scrollSpeed = 0;
        }
        else if ((transform.position.y <= minHeight) && (scrollSpeed < 0))
        {
            scrollSpeed = 0;
        }

        if ((transform.position.y + scrollSpeed) > maxHeight)
        {
            scrollSpeed = maxHeight - transform.position.y;
        }
        else if (transform.position.y + scrollSpeed < minHeight)
        {
            scrollSpeed = minHeight - transform.position.y;
        }

        return scrollSpeed;
    }
    #endregion

    #region CameraRotationCalculation
    private void GetCameraRotation()
    {
        // Mousecode: right button
        if (Input.GetMouseButtonDown(1))
        {
            // the position when player presses the right button
            currentPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            // new position when player moves the mouse while pressing the right button
            newPosition = Input.mousePosition;


            // How far the mouse moves since the last frame
            float dx = (newPosition - currentPosition).x * rotateSpeed;
            float dy = (newPosition - currentPosition).y * rotateSpeed;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0)); // Update rotation to camera - Y rotation
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));// Update rotation to camera - X rotation

            // reset the position
            currentPosition = newPosition;

        }
    }
    #endregion

}
