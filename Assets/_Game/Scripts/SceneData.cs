using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public Camera mainCamera;
    public CardData cardCurrent;
    public Transform cardStartPos;
    public BoxCollider2D dragCardZone;
}
