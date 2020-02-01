using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public Camera mainCamera;
    public Transform sceneRoot;
    
    public Transform cardStartPos;
    public BoxCollider2D dragCardZone;

    public CardData cardPrefab;
    public CardData cardCurrent;
}
