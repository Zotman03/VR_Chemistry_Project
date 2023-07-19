using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBondScene : MonoBehaviour
{
    public string new_Scene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        // GameStateManager.Victory();
        // StartCoroutine(SetFade());
        SceneManager.LoadScene(new_Scene);
    }

    // IEnumerator SetFade()
    // {
    //     if (complete)
    //     {
    //         if (PlayerPrefs.GetInt("LevelsCompleted") < level)
    //         {
    //             PlayerPrefs.SetInt("LevelsCompleted", level);
    //         }
    //     }

    //     float f = 0;

    //     while (f < 1)
    //     {
    //         f += Time.unscaledDeltaTime;

    //         if (f > 1)
    //         {
    //             f = 1;
    //         }

    //         Fade.SetFade(f);
    //         yield return new WaitForEndOfFrame();
    //     }

    //     yield return new WaitForEndOfFrame();

    //     SceneManager.LoadScene(new_Scene);
    // }
}
