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
                var delta = sceneData.testCard.position - sceneData.cardStartPos.position;

                if (Mathf.Abs(delta.x) > sceneData.dragCardZone.size.x * 0.5f)
                {
                    // handle drag out
                }
                sceneData.testCard.position = sceneData.cardStartPos.position;
                sceneData.testCard.rotation = sceneData.cardStartPos.rotation;
            }

            if (gameData.isCardDragging)
            {
                var dragNowPos = GetMousePosition();
                var dragDelta = dragNowPos - gameData.dragLastPos;
                gameData.dragLastPos = dragNowPos;
                sceneData.testCard.position += new Vector3(dragDelta.x, dragDelta.y);
                var delta = sceneData.testCard.position - sceneData.cardStartPos.position;
                sceneData.testCard.rotation = Quaternion.Euler(0f, 0f, -delta.x * 10f);
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
