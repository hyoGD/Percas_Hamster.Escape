using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class FirebaseMananger : MonoBehaviour
{
    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
    public static void LogEventShowBanner(string _mess)
    {
        FirebaseAnalytics.LogEvent(_mess);

        //Kochava.Event myEvent = new Kochava.Event(_mess);
        //Kochava.Tracker.SendEvent(myEvent);
    }
    public static void LogEventShowInter(string _mess)
    {
        FirebaseAnalytics.LogEvent(_mess);

        //Kochava.Event myEvent = new Kochava.Event(_mess);
        //Kochava.Tracker.SendEvent(myEvent);
    }

    public static void LogEventLevelComplete(int level)
    {
        Parameter[] _pamLevelComplete = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, level+1)
                };
        FirebaseAnalytics.LogEvent("Level_complete", _pamLevelComplete);
     //   Debug.Log("wwin");
        //Kochava.Event myEvent = new Kochava.Event(Kochava.EventType.LevelComplete);
        //myEvent.name = "level_" + level;
        //Kochava.Tracker.SendEvent(myEvent);
    }
    public static void LogEventLevelLose(int level)
    {
        Parameter[] _pamLevelLose = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, level+1)
                };
        FirebaseAnalytics.LogEvent("Level_lose", _pamLevelLose);
     //   Debug.Log("lose");
        //Kochava.Event myEvent = new Kochava.Event("level_lose");
        //myEvent.name = "level_" + level;
        //Kochava.Tracker.SendEvent(myEvent);
    }

    public static void LogEventPlayLevel(int level)
    {
        Parameter[] _pamPlayLevel = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, level+1)
                };
        FirebaseAnalytics.LogEvent("Play_Level", _pamPlayLevel);
        
        //Kochava.Event myEvent = new Kochava.Event("level_lose");
        //myEvent.name = "level_" + level;
        //Kochava.Tracker.SendEvent(myEvent);
    }

}

