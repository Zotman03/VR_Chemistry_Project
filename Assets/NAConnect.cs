using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAConnect : MonoBehaviour
{
    int connect = 0;
    public bool isAlreadyConnected = false;  // Flag to track whether this object is already connected

    void OnCollisionEnter(Collision collision)
    {
        if(isAlreadyConnected){
            RepelObject(collision);
            return;
        }

        Connect otherSphere = collision.gameObject.GetComponent<Connect>();
        NAConnect naSphere = collision.gameObject.GetComponent<NAConnect>();

        if(otherSphere && !otherSphere.isAlreadyConnected){
            Bond(collision.gameObject);
            otherSphere.Bonded();
            isAlreadyConnected = true;
        }
    }

    void Bond(GameObject other){
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();
        connect++;
    }
    
    public void BreakBond(){
        FixedJoint joint = GetComponent<FixedJoint>();
        if(joint){
            Destroy(joint);
            isAlreadyConnected = false;  // Reset the connection status
        }
    }

    void RepelObject(Collision collision){
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        Rigidbody thisRb = this.gameObject.GetComponent<Rigidbody>();

        if(rb && thisRb){
            Vector3 repulsionDir = collision.transform.position - transform.position;
            float force = 10.0f;
            rb.AddForce(repulsionDir.normalized * force, ForceMode.Impulse);
        }
    }
}
