using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public GameObject self;
    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.grabbing == true) {
            Vector3 targetPosition = player.transform.position;
            transform.position = targetPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the Enemy component from the collided enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Calculate damage based on the object's velocity or other criteria
                int damage = 20; // You can adjust this value according to your game's balance

                // Call the TakeDamage method of the enemy to apply damage
                enemy.TakeDamage(damage);
            }
        }
    }

    /*
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerControllerScript.canGrab = true;
        }
        
    }
    */
}
