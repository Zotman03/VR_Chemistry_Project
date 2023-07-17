using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBondScene : MonoBehaviour
{
    public string nextSceneName; // The name of the scene to transition to
    
    private bool hasTransitioned = false; // Flag to ensure the transition happens only once
    
    private void Update()
    {
        // Check if the player's head (or other VR object) is on the plane
        if (!hasTransitioned && IsPlayerOnPlane())
        {
            hasTransitioned = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private bool IsPlayerOnPlane()
    {
        // Cast a ray from the player's head (or other VR object) to check for collisions with the plane
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class ToBondScene : MonoBehaviour
// {
//     public string new_Scene;
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void OnTriggerEnter()
//     {
//         // GameStateManager.Victory();
//         // StartCoroutine(SetFade());
//         SceneManager.LoadScene(new_Scene);
//     }

//     // IEnumerator SetFade()
//     // {
//     //     if (complete)
//     //     {
//     //         if (PlayerPrefs.GetInt("LevelsCompleted") < level)
//     //         {
//     //             PlayerPrefs.SetInt("LevelsCompleted", level);
//     //         }
//     //     }

//     //     float f = 0;

//     //     while (f < 1)
//     //     {
//     //         f += Time.unscaledDeltaTime;

//     //         if (f > 1)
//     //         {
//     //             f = 1;
//     //         }

//     //         Fade.SetFade(f);
//     //         yield return new WaitForEndOfFrame();
//     //     }

//     //     yield return new WaitForEndOfFrame();

//     //     SceneManager.LoadScene(new_Scene);
//     // }
// }
