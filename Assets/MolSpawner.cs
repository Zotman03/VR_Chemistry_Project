// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MolSpawner : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }

using UnityEngine;

public class MolSpawner : MonoBehaviour
{
    public GameObject spherePrefab;

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
            for (int i = 0; i < numberOfSpheres; i++)
            {
                Instantiate(spherePrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            }
        }
    }
}
