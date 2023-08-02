using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int points;
    public int maxPlayerHealth = 100;
    [SerializeField]
    private int currentPlayerHealth;

    private void Start()
    {
        // Initialize currentPlayerHealth to maxPlayerHealth
        currentPlayerHealth = maxPlayerHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the player
            int damage = 10; // You can adjust this value according to your game's balance
            currentPlayerHealth -= damage;

            // Check if the player's health reaches zero
            if (currentPlayerHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
