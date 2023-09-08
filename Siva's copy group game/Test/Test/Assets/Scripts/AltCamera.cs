using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltCamera : MonoBehaviour
{
    public Transform player;      // The player's transform
    public Vector3 offset;        // The offset between the camera and the player
    public float zoomSpeed = 5f;  // The speed at which the camera zooms

    private float zoomDistance = 10f;  // The initial distance between the camera and the player

    private void LateUpdate()
    {
        // Calculate the target position based on the player's position and the offset
        Vector3 targetPosition = player.position + offset;
        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        Vector3 zoomVector = transform.forward * -zoomDistance;
        transform.position += zoomVector;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        zoomDistance += scrollInput * zoomSpeed;
        zoomDistance = Mathf.Clamp(zoomDistance, 5f, 20f);
    }
}
