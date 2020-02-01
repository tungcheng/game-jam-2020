using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject
{
    public List<GameEvent> events;
    public List<GameEnding> endings;
    public List<GameCharacter> characters;

    public GameEnding GetEnding(string reason)
    {
        return endings.Find(x => x.name == reason);
    }

    public GameCharacter GetCharacter(string character)
    {
        return characters.Find(x => x.character == character);
    }
}

[System.Serializable]
public class GameCharacter
{
    public string character;
    public Sprite avatar;
}

[System.Serializable]
public class GameEnding
{
    public string name;
    public string header;
    public string textInfo;
    public Sprite sprite;
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