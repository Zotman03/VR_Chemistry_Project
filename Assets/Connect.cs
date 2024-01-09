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
// using System.Collections;
// using UnityEngine;

// public class Connect : MonoBehaviour
// {
//     public bool isAlreadyConnected = false;
//     private GameObject connectedObject;

//     void OnCollisionEnter(Collision collision)
//     {
//         if (isAlreadyConnected)
//         {
//             return;
//         }

//         Connect otherSphere = collision.gameObject.GetComponent<Connect>();
//         //NAConnect naSphere = collision.gameObject.GetComponent<NAConnect>();

//         if (otherSphere && !otherSphere.isAlreadyConnected)
//         {
//             Bond(collision.gameObject);
//             otherSphere.Bonded(this.gameObject); // Pass this gameObject as the one it's connected to
//             isAlreadyConnected = true;
//         }
//         // else if (naSphere && !naSphere.isAlreadyConnected)
//         // {
//         //     // Presuming NAConnect has a similar structure to Connect
//         //     Bond(collision.gameObject);
//         //     //naSphere.Bonded(this.gameObject);
//         //     isAlreadyConnected = true;
//         // }
//     }

//     void Bond(GameObject other)
//     {
//         FixedJoint joint = gameObject.AddComponent<FixedJoint>();
//         joint.connectedBody = other.GetComponent<Rigidbody>();
//         connectedObject = other;
//     }

//     public void Bonded(GameObject other)
//     {
//         isAlreadyConnected = true;
//         connectedObject = other;
//     }

//     public void BreakBond()
//     {
//         FixedJoint joint = GetComponent<FixedJoint>();
//         if (joint)
//         {
//             Destroy(joint);
//         }
//         if (connectedObject != null)
//         {
//             // Call a method on the connected object to break the bond from its side
//             Connect otherConnect = connectedObject.GetComponent<Connect>();
//             if (otherConnect != null)
//             {
//                 otherConnect.BreakBondFromOther(); // This should be a method that only breaks the bond without calling Disconnect
//             }
//             connectedObject = null;
//         }
//         StartCoroutine(ResetAfterPhysicsUpdate());
//     }

//     // This method gets called by the other object when it breaks the bond
//     public void BreakBondFromOther()
//     {
//         FixedJoint joint = GetComponent<FixedJoint>();
//         if (joint)
//         {
//             Destroy(joint);
//         }
//         connectedObject = null;
//         isAlreadyConnected = false;
//         // No need for the coroutine here since it's called by BreakBond
//     }

//     private IEnumerator ResetAfterPhysicsUpdate()
//     {
//         yield return new WaitForFixedUpdate();
//         isAlreadyConnected = false;
//         Rigidbody rb = GetComponent<Rigidbody>();
//         if (rb != null)
//         {
//             rb.isKinematic = false;
//             rb.velocity = Vector3.zero;
//             rb.angularVelocity = Vector3.zero;
//         }
//     }
// }

using System.Collections;
using UnityEngine;

public class Connect : MonoBehaviour
{
    public bool isAlreadyConnected = false;
    private GameObject connectedObject;

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
}

