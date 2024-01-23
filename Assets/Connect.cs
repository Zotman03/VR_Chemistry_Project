// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class Connect : MonoBehaviour
// // {
// //     public bool isAlreadyConnected = false;  // Flag to track whether this object is already connected
// //     int connect = 0;
// //     private GameObject connectedObject;

// //     void OnCollisionEnter(Collision collision)
// //     {
// //         if (isAlreadyConnected)
// //         {
// //             return;
// //         }

// //         Connect otherSphere = collision.gameObject.GetComponent<Connect>();
// //         NAConnect naSphere = collision.gameObject.GetComponent<NAConnect>();

// //         if (otherSphere && !otherSphere.isAlreadyConnected)
// //         {
// //             Bond(collision.gameObject);
// //             otherSphere.Bonded();
// //             isAlreadyConnected = true;
// //         }
// //         else if (naSphere && !naSphere.isAlreadyConnected)
// //         {
// //             Bond(collision.gameObject);
// //             naSphere.isAlreadyConnected = true;
// //             isAlreadyConnected = true;
// //         }
// //     }

// //     void Bond(GameObject other)
// //     {
// //         FixedJoint joint = gameObject.AddComponent<FixedJoint>();
// //         joint.connectedBody = other.GetComponent<Rigidbody>();
// //         connect++;
// //     }

// //     public void Bonded()
// //     {
// //         isAlreadyConnected = true;
// //     }

// //     void Bond(GameObject other)
// //     {
// //         FixedJoint joint = gameObject.AddComponent<FixedJoint>();
// //         joint.connectedBody = other.GetComponent<Rigidbody>();
// //         connect++;

// //         // Set the connected object
// //         connectedObject = other;
// //     }

// //     public void BreakBond()
// //     {
// //         FixedJoint joint = GetComponent<FixedJoint>();
// //         if (joint)
// //         {
// //             Destroy(joint);
// //             //joint.connectedBody = null;
// //         }
// //         StartCoroutine(ResetAfterPhysicsUpdate());
// //     }
// //     public void Disconnect()
// //     {
// //         BreakBond();
// //         // Inform the other object to break the bond as well
// //         if (connectedObject != null)
// //         {
// //             connectedObject.GetComponent<Connect>().BreakBond();
// //         }
// //     }
// //     private IEnumerator ResetAfterPhysicsUpdate()
// //     {
// //         yield return new WaitForFixedUpdate();
// //         isAlreadyConnected = false;
// //         Rigidbody rb = GetComponent<Rigidbody>();
// //         if (rb != null)
// //         {
// //             rb.isKinematic = false;
// //             rb.velocity = Vector3.zero;
// //             rb.angularVelocity = Vector3.zero;
// //         }

// //         // Optionally, if you need to force the objects to separate, you can apply a small force or change their positions
// //         //rb.AddForce(Vector3.up * 0.1f, ForceMode.VelocityChange); // Example force

// //         // If there's another object that was bonded, reset its state as well
// //         // You'll need a reference to the connected object to do this
// //     }
// // }

using System.Collections;
using UnityEngine;

public class Connect : MonoBehaviour
{
    public bool isAlreadyConnected = false;
    private GameObject connectedObject;
    public float maxDistance = 5.0f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter: Collided with " + collision.gameObject.name);

        if (isAlreadyConnected)
        {
            Debug.Log("Already connected, ignoring collision");
            return;
        }

        Connect otherSphere = collision.gameObject.GetComponent<Connect>();
        //NAConnect naSphere = collision.gameObject.GetComponent<NAConnect>();

        if (otherSphere && !otherSphere.isAlreadyConnected)
        {
            Debug.Log("Bonding with other Connect object: " + collision.gameObject.name);
            Bond(collision.gameObject);
            otherSphere.Bonded(this.gameObject);
            isAlreadyConnected = true;
        }
        // else if (naSphere && !naSphere.isAlreadyConnected)
        // {
        //     Debug.Log("Bonding with NAConnect object: " + collision.gameObject.name);
        //     Bond(collision.gameObject);
        //     naSphere.isAlreadyConnected = true;
        //     isAlreadyConnected = true;
        // }
    }

    void Bond(GameObject other)
    {
        Debug.Log("Creating bond with: " + other.name);
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();

        GameObject parentCL = gameObject.transform.parent.gameObject;
        Debug.Log(parentCL);
        GameObject otherParentCL = other.transform.parent.gameObject;
        Debug.Log(otherParentCL);

        // Check if the parent CL GameObjects have Rigidbodies and are not already connected
        Rigidbody parentRb = parentCL.GetComponent<Rigidbody>();
        Rigidbody otherParentRb = otherParentCL.GetComponent<Rigidbody>();

        if (parentRb != null && otherParentRb != null)
        {
            FixedJoint parentJoint = parentCL.AddComponent<FixedJoint>();
            parentJoint.connectedBody = otherParentRb;

            gameObject.GetComponent<Connect>().isAlreadyConnected = true;
            other.GetComponent<Connect>().isAlreadyConnected = true;
        }
        connectedObject = other;
    }

    public void Bonded(GameObject other)
    {
        Debug.Log("Bonded called by: " + other.name);
        isAlreadyConnected = true;
        connectedObject = other;
    }

    public void BreakBond()
    {
        Debug.Log("Breaking bond");
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint)
        {
            Destroy(joint);
        }
        if (connectedObject != null)
        {
            Connect otherConnect = connectedObject.GetComponent<Connect>();
            if (otherConnect != null)
            {
                otherConnect.BreakBondFromOther();
            }
            connectedObject = null;
        }
        StartCoroutine(ResetAfterPhysicsUpdate());
    }

    public void BreakBondFromOther()
    {
        Debug.Log("BreakBondFromOther called");
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint)
        {
            Destroy(joint);
        }
        connectedObject = null;
        isAlreadyConnected = false;
    }

    private IEnumerator ResetAfterPhysicsUpdate()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("Physics update reset");
        isAlreadyConnected = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    void Update()
    {
        if (isAlreadyConnected && connectedObject != null)
        {
            float distance = Vector3.Distance(transform.position, connectedObject.transform.position);

            if (distance > maxDistance)
            {
                BreakBond();
                Connect otherConnect = connectedObject.GetComponent<Connect>();
                if (otherConnect != null)
                {
                    otherConnect.BreakBondFromOther();
                }
            }
        }
    }
}

