using System.Collections;
using UnityEngine;

public class ConnectN : MonoBehaviour
{
    public GameObject subGameObjectA;
    public GameObject subGameObjectB; // if exist other chemical
    public int isAlreadyConnected = 0;
    private GameObject connectedObject;
    public float maxDistance = 5.0f;

    public void OnCollisionEnterChild(GameObject child, Collision collision)
    {
        Debug.Log(isAlreadyConnected);
        if (isAlreadyConnected == 2)
        {
            return;
        }

        Connect other = collision.transform.parent.GetComponent<Connect>();
        if (other != null && !other.isAlreadyConnected)
        {
            Bond(other.gameObject, child);
            other.Bonded(this.gameObject, child);
        }
    }
    void Update()
    {
        if ((isAlreadyConnected == 1 || isAlreadyConnected == 2) && connectedObject != null)
        {
            float distance = Vector3.Distance(subGameObjectA.transform.position, connectedObject.transform.position);
            GlobalChemistryData chemistryData = GlobalChemistryData.instance;
            if (distance > 0.5f)
            {
                BreakBond();
                chemistryData.mixedChemicalCombinedAmount -= 1f;
                if (chemistryData.mixedChemicalCombinedAmount == 0f)
                {
                    chemistryData.mixedChemicalCombined = "";
                }
                return;
            }
        }

        if ((isAlreadyConnected == 0 || isAlreadyConnected == 1)&& subGameObjectA != null)
        {
            GameObject closestTarget = null;
            float closestDistance = 0.01f;
            Collider[] hitColliders = Physics.OverlapSphere(subGameObjectA.transform.position, 0.01f);
            GlobalChemistryData chemistryData = GlobalChemistryData.instance;
            chemistryData.mixedChemicalCombined = "NACL";
            chemistryData.mixedChemicalCombinedAmount += 1f;
            foreach (var hitCollider in hitColliders)
            {
                Connect otherConnect = hitCollider.GetComponentInParent<Connect>();
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

            if (closestTarget != null)
            {
                Bond(closestTarget.transform.parent.gameObject, subGameObjectA);
                closestTarget.transform.parent.GetComponent<Connect>().Bonded(gameObject, subGameObjectA);
            }
        }
    }

    public void Bonded(GameObject other, GameObject initiatingChild)
    {
        isAlreadyConnected++;
        connectedObject = other;
    }

    void Bond(GameObject other, GameObject initiatingChild)
    {
        Rigidbody thisRb = initiatingChild.transform.parent.GetComponent<Rigidbody>() ?? initiatingChild.transform.parent.gameObject.AddComponent<Rigidbody>();
        FixedJoint joint = initiatingChild.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();
        Rigidbody otherRb = other.GetComponent<Rigidbody>() ?? other.AddComponent<Rigidbody>();
        FixedJoint otherJoint = other.AddComponent<FixedJoint>();
        otherJoint.connectedBody = thisRb;
        isAlreadyConnected++;
        connectedObject = other;
    }


    public void BreakBond()
    {
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
        foreach (FixedJoint joint in GetComponentsInChildren<FixedJoint>())
        {
            Destroy(joint);
        }
        connectedObject = null;
        isAlreadyConnected = 0;
    }

    private IEnumerator ResetAfterPhysicsUpdate()
    {
        yield return new WaitForFixedUpdate();
        isAlreadyConnected = 0;
    }
}

