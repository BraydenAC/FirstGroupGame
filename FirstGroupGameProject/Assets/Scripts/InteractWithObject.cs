using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    public float grabbingDistance = 7f;
    public float grabRadius = 2f;
    public string grabbableTag = "Interactable";
    public float throwForce = 10f;
    public string enemyTag = "Enemy";
    public float autoThrowRange = 15f;

    public Transform grabPosition;
    public KeyCode grabKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.Q;
    public KeyCode setDownKey = KeyCode.F;

    private GameObject grabbedObject;
    private Rigidbody grabbedObjectRigidbody;

    private void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
        }

        if (Input.GetKeyDown(throwKey))
        {
            if (grabbedObject != null)
            {
                if (EnemyInRange())
                {
                    ThrowObjectTowardsEnemy();
                }
                else
                {
                    ThrowObject();
                }
            }
        }

        if (Input.GetKeyDown(setDownKey))
        {
            if (grabbedObject != null)
            {
                SetDownObject();
            }
        }
    }

    private void TryGrabObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(grabbableTag))
            {
                grabbedObject = collider.gameObject;
                grabbedObjectRigidbody = grabbedObject.GetComponent<Rigidbody>();
                grabbedObjectRigidbody.isKinematic = true;
                grabbedObject.transform.position = grabPosition.position;
                grabbedObject.transform.parent = grabPosition;
                break;
            }
        }
    }

    private void ThrowObject()
    {
        grabbedObjectRigidbody.isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObjectRigidbody.velocity = Camera.main.transform.rotation * Vector3.forward * throwForce;

        grabbedObject = null;
        grabbedObjectRigidbody = null;
    }

    private void SetDownObject()
    {
        grabbedObjectRigidbody.isKinematic = false;
        grabbedObject.transform.parent = null;

        grabbedObject = null;
        grabbedObjectRigidbody = null;
    }

    private bool EnemyInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, autoThrowRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                return true;
            }
        }
        return false;
    }

    private void ThrowObjectTowardsEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, autoThrowRange);
        Vector3 throwDirection = Vector3.zero;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                throwDirection = (collider.transform.position - transform.position).normalized;
                break;
            }
        }

        grabbedObjectRigidbody.isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObjectRigidbody.velocity = throwDirection * throwForce;

        grabbedObject = null;
        grabbedObjectRigidbody = null;
    }
}
