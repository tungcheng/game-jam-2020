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

                if (Mathf.Abs(delta.x) > sceneData.dragCardZone.size.x * 0.5f)
                {
                    sceneData.cardCurrent.SetFlipOut();
                }
                else
                {
                    sceneData.cardCurrent.SetReturn();
                }
            }

            if (gameData.isCardDragging && sceneData.cardCurrent.state == CardData.CardState.CanDrag)
            {
                var dragNowPos = GetMousePosition();
                var dragDelta = dragNowPos - gameData.dragLastPos;
                gameData.dragLastPos = dragNowPos;
                sceneData.cardCurrent.SetPosByDrag(dragDelta);
            }
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
