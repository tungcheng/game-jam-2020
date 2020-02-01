using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerData playerData;
    public GameData gameData;
    public SceneData sceneData;

    // Start is called before the first frame update
    void Start()
    {
        Pool.Set(playerData);
        Pool.Set(gameData);
        Pool.Set(sceneData);

        sceneData.cardPrefab.isActive = false;
        CreateNewCard();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.state == GameState.Choosing)
        {
            if (IsMouseDown() && !gameData.isCardDragging)
            {
                gameData.isCardDragging = true;
                gameData.dragLastPos = GetMousePosition();
            }

            if (IsMouseUp())
            {
                gameData.isCardDragging = false;
                var delta = sceneData.cardCurrent.position - sceneData.cardStartPos.position;

                if (Mathf.Abs(delta.x) > sceneData.GetHalfZoneX())
                {
                    sceneData.cardCurrent.SetFlipOut();

                    foreach (var res in sceneData.resources)
                    {
                        var change = (delta.x > 0f) ? res.changeOnRight : res.changeOnLeft;
                        res.SetChange(change);
                    }

                    CreateNewCard();
                }
                else
                {
                    sceneData.cardCurrent.SetReturn();

                    foreach (var res in sceneData.resources)
                    {
                        res.HideHint();
                    }
                }
            }

            if (gameData.isCardDragging && sceneData.cardCurrent.state == CardData.CardState.CanDrag)
            {
                var dragNowPos = GetMousePosition();
                var dragDelta = dragNowPos - gameData.dragLastPos;

                var delta = sceneData.cardCurrent.position - sceneData.cardStartPos.position;
                var delta01 = Mathf.Sign(delta.x) * Mathf.Clamp01(Mathf.Abs(delta.x) / sceneData.GetHalfZoneX());

                gameData.dragLastPos = dragNowPos;
                sceneData.cardCurrent.SetPosByDrag(dragDelta);

                foreach (var res in sceneData.resources)
                {
                    var change = (delta.x > 0f) ? res.changeOnRight : res.changeOnLeft;
                    res.ShowHint(change, delta01);
                }
            }
        }
    }

    void CreateNewCard()
    {
        sceneData.cardCurrent = Instantiate(sceneData.cardPrefab, sceneData.sceneRoot);
        sceneData.cardCurrent.isActive = true;

        foreach(var res in sceneData.resources)
        {
            res.changeOnLeft = Random.Range(-4f, 4f);
            res.changeOnRight = Random.Range(-4f, 4f);
        }
    }

    bool IsMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    bool IsMouseUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    Vector2 GetMousePosition()
    {
        return sceneData.mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
