using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public GameConfig config;
    public Camera mainCamera;
    public Transform sceneRoot;
    public GoogleSheetToCSV sheetLoader;

    public Transform cardStartPos;
    public BoxCollider2D dragCardZone;

    public CardData cardPrefab;
    public CardData cardCurrent;

    public ResourceData[] resources;
    public Color colorAdd;
    public Color colorReduce;
    public Color colorNormal;

    public Color GetColorChange(float change)
    {
        if (change == 0) return colorNormal;
        return (change > 0) ? colorAdd : colorReduce;
    }

    public float GetHalfZoneX()
    {
        return dragCardZone.size.x * 0.5f;
    }
}
