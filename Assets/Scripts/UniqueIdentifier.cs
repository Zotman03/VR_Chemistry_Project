//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UniqueIdentifier : MonoBehaviour
//{
//    public int UniqueID { get; private set; }

//    void Awake()
//    {
//        if (UniqueID == 0)
//        {
//            UniqueID = GetInstanceID();
//            Liquid liquid = GetComponent<Liquid>();
//            if (liquid != null)
//            {
//                liquid.uniqueID = GetInstanceID();
//            }
//        }
//    }
//}
