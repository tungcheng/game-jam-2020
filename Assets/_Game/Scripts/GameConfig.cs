using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject
{
    public List<GameEvent> events;
}

[System.Serializable]
public class GameEvent
{
    public string character;
    public string eventInfo;
    public EventAnswer answerRight;
    public EventAnswer answerLeft;
}

[System.Serializable]
public class EventAnswer
{
    public string swipeSide;
    public string textAnswer;

    public List<ResourceResult> results;
}

[System.Serializable]
public class ResourceResult
{
    public string resource;
    public float min;
    public float max;
}