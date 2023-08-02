using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();
            playerInventory.points += 1;
            Destroy(gameObject);
        }
    }
}
