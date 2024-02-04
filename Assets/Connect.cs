using System.Collections;
using UnityEngine;

public class Connect : MonoBehaviour
{
    public GameObject subGameObjectA; // Assign in the inspector, e.g., A or C
    //public GameObject subGameObjectB; // Assign in the inspector, e.g., B or D
    public bool isAlreadyConnected = false;
    private GameObject connectedObject; // This will reference the other parent GameObject we're connected to
    public float maxDistance = 5.0f;

    // This method is now public and meant to be called from the child GameObjects
    public void OnCollisionEnterChild(GameObject child, Collision collision)
    {
        Debug.Log($"{child.name} collided with {collision.gameObject.name}");

        if (isAlreadyConnected)
        {
            Debug.Log("Already connected, ignoring collision");
            return;
        }

        Connect other = collision.transform.parent.GetComponent<Connect>();
        if (other != null && !other.isAlreadyConnected)
        {
            Debug.Log("Bonding initiated by child");
            Bond(other.gameObject, child);
            other.Bonded(this.gameObject, child); // Assuming other.Bonded() is adjusted accordingly
        }
    }
    void Update()
    {
        if (isAlreadyConnected && connectedObject != null)
        {
            float distance = Vector3.Distance(subGameObjectA.transform.position, connectedObject.transform.position);
            if (distance > 0.5f) // 3 feet in meters
            {
                BreakBond();
                return; // Exit the method to prevent trying to reconnect in the same frame
            }
        }
        // Only proceed if not already connected and the subGameObjectA is assigned
        if (!isAlreadyConnected && subGameObjectA != null)
        {
            GameObject closestTarget = null;
            float closestDistance = 0.01f; // Use maxDistance as the initial comparison base

            // Find all colliders within maxDistance from subGameObjectA
            Collider[] hitColliders = Physics.OverlapSphere(subGameObjectA.transform.position, 0.01f);
            foreach (var hitCollider in hitColliders)
            {
                Connect otherConnect = hitCollider.GetComponentInParent<Connect>();
                // Check if the hitCollider's parent GameObject is a valid connection target
                if (otherConnect != null && otherConnect != this && !otherConnect.isAlreadyConnected)
                {
                    float distance = Vector3.Distance(subGameObjectA.transform.position, hitCollider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = hitCollider.gameObject;
                    }
                }
            }

            // If a closest target is found, attempt to bond
            if (closestTarget != null)
            {
                Debug.Log("Attempting to bond with target based on proximity");
                Bond(closestTarget.transform.parent.gameObject, subGameObjectA);
                closestTarget.transform.parent.GetComponent<Connect>().Bonded(gameObject, subGameObjectA);
            }
        }
    }

    public void Bonded(GameObject other, GameObject initiatingChild)
    {
        Debug.Log("Bonded called by: " + other.name);
        isAlreadyConnected = true;
        connectedObject = other;
    }

    void Bond(GameObject other, GameObject initiatingChild)
    {
        Debug.Log($"Creating bond with: {other.name}, initiated by {initiatingChild.name}");

        // Ensure this object has a Rigidbody
        Rigidbody thisRb = initiatingChild.transform.parent.GetComponent<Rigidbody>() ?? initiatingChild.transform.parent.gameObject.AddComponent<Rigidbody>();

        // Add or get FixedJoint on initiating child
        FixedJoint joint = initiatingChild.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();

        // Optionally, ensure the other object is prepared to maintain connection
        // This step depends on your specific requirements and setup
        Rigidbody otherRb = other.GetComponent<Rigidbody>() ?? other.AddComponent<Rigidbody>();
        FixedJoint otherJoint = other.AddComponent<FixedJoint>();
        otherJoint.connectedBody = thisRb;

        isAlreadyConnected = true;
        connectedObject = other;
    }


    public void BreakBond()
    {
        Debug.Log("Breaking bond");
        // Breaking the bond now should consider all FixedJoints in children
        foreach (FixedJoint joint in GetComponentsInChildren<FixedJoint>())
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
        foreach (FixedJoint joint in GetComponentsInChildren<FixedJoint>())
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
    }
}

