using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Camera camera;
    public GameObject target;
    public int zoom = 10;
    private float ScrollSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.y += zoom;
        transform.position = targetPosition;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            zoom++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            zoom--;
        }
        if (zoom < 2) {
            zoom = 2;
        }
        if (zoom > 30) {
            zoom = 30;
        }
    }

}
