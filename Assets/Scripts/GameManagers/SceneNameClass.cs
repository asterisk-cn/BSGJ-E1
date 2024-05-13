using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNameClass : MonoBehaviour
{
    public enum SceneName
    {
        Title,
        Main,
        Fight
    }

    public static Dictionary<SceneName, string> SceneNameToString
        = new Dictionary<SceneName, string>()
    {
        {SceneName.Title, "Title" },
        {SceneName.Main, "Main" },
        {SceneName.Fight, "Fight" }
    };
}
