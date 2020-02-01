using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameState state;
    public int cardCount;
    public bool isCardDragging;
    public Vector2 dragLastPos;

    public void SetState(GameState state)
    {
        this.state = state;
    }
}

public enum GameState
{
    Choosing
}