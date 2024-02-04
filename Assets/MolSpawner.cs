using UnityEngine;

public class MolSpawner : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject otherspherePrefab;

    void Start()
    {
        SpawnSpheres();
    }

    void SpawnSpheres()
    {
        GlobalChemistryData chemistryData = GlobalChemistryData.instance;
        if (chemistryData != null)
        {
            int numberOfSpheres = Mathf.FloorToInt(chemistryData.molesOfSubstanceOne);
            int numberOfSpheres1 = Mathf.FloorToInt(chemistryData.molesOfSubstanceTwo);
            for (int i = 0; i < numberOfSpheres; i++)
            {
                Instantiate(spherePrefab, new Vector3(i * 2.0f, 1, 0), Quaternion.identity);
            }
            for (int i = 0; i < numberOfSpheres1; i++)
            {
                Instantiate(otherspherePrefab, new Vector3(i * 4.0f, 1, 5), Quaternion.identity);
            }
        }
    }
}
