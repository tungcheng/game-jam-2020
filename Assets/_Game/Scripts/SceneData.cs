using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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

    public TMP_Text textEventInfo;
    public TMP_Text timeInfo;

    public Dictionary<string, ResourceData> dictResource;

    public Dictionary<string, ResourceData> DictResources()
    {
        if (dictResource == null)
        {
            dictResource = resources.ToDictionary(x => x.gameObject.name, x => x);
        }
        return dictResource;
    }

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
