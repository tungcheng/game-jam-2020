using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameState state;
    public bool isCardDragging;
    public Vector2 dragLastPos;
}

public enum GameState
{
    Choosing
}