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
}
