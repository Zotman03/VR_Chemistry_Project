using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : MonoBehaviour{
    public bool isAlreadyConnected = false;  // Flag to track whether this object is already connected
    int connect = 0;

    void OnCollisionEnter(Collision collision){
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
        else if(naSphere && !naSphere.isAlreadyConnected){
            Bond(collision.gameObject);
            naSphere.isAlreadyConnected = true;
            isAlreadyConnected = true;
        }
    }

    void Bond(GameObject other){
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();
        connect++;
    }

    public void Bonded(){
        isAlreadyConnected = true;
    }

    public void BreakBond(){
        FixedJoint joint = GetComponent<FixedJoint>();
        if(joint){
            Destroy(joint);
            isAlreadyConnected = false;  // Reset the connection status
        }
    }
}
