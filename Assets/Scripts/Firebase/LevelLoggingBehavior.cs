using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.SceneManagement;

public class LevelLoggingBehavior : MonoBehaviour
{
    private int _sceneIndex;
    private string _sceneName;

    private void Start()
    {
        var activeSecene = SceneManager.GetActiveScene();
        _sceneIndex = activeSecene.buildIndex;
        _sceneName = activeSecene.name;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
            new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
    }

    private void onDestroy()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
            new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
    }
}
