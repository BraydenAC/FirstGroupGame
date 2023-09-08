using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 0.03f;
    private Rigidbody playerRb;
    public float jumpForce;
    public int health = 50;
    public bool isOnGround = true;
    public bool canGrab = false;
    public bool grabbing = false;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //this moves you relative to how you start, which means that whenever you turn everything gets weird
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            print("whoa");
            isOnGround = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRb.AddForce(transform.right * speed, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerRb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerRb.AddForce(-transform.right * speed, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRb.AddForce(-transform.forward * speed, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.E) && canGrab == true && grabbing == false)
        {
            grabbing = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && grabbing == true)
        {
            grabbing = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;
            if (health <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("whoa2");
            isOnGround = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canGrab = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canGrab = false;
        }
    }
}



