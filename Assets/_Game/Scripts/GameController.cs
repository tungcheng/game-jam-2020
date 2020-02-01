using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        sceneData.btnRestart.onClick.AddListener(StartNewGame);
        StartNewGame();
    }

    public void StartNewGame()
    {
        sceneData.cardPrefab.isActive = false;
        sceneData.endingInfo.gameObject.SetActive(false);
        if (sceneData.cardCurrent != null)
        {
            Destroy(sceneData.cardCurrent.gameObject);
            gameData.cardCount = 0;
        }
        foreach (var res in sceneData.resources)
        {
            res.SetStartNew();
        }
        gameData.SetState(GameState.Choosing);
        CreateNewCard();
    }

    [NaughtyAttributes.Button]
    public void LoadGameConfig()
    {
        sceneData.sheetLoader.DownLoadCsv("1JVavEENVVWvR2J86RQTvHFUlV8-KjbnWRMdoizeQihU", "0", true, data =>
        {
            //data.ForEach(x => x.ForEach(print));

            Dictionary<int, string> tempString = new Dictionary<int, string>();
            Dictionary<string, GameEvent> tempEvents = new Dictionary<string, GameEvent>();

            int i = 1;
            foreach (var row in data)
            {
                i++;
                row.ForEach(print);
                var resource = row[5].Trim();
                if (!string.IsNullOrWhiteSpace(resource))
                {
                    var character = GetColumnValue(row, 1, tempString);
                    var eventName = GetColumnValue(row, 2, tempString);
                    var answer = GetColumnValue(row, 3, tempString);
                    var textAnswer = GetColumnValue(row, 4, tempString);
                    float min;
                    min = float.Parse(row[6]);
                    if (!float.TryParse(row[7], out float max))
                    {
                        max = min;
                    }

                    if (!tempEvents.TryGetValue(eventName, out GameEvent tempEvt))
                    {
                        tempEvt = new GameEvent()
                        {
                            eventInfo = eventName,
                            character = character
                        };
                        tempEvt.answerRight = new EventAnswer()
                        {
                            swipeSide = "Right",
                            textAnswer = "",
                            results = new List<ResourceResult>()
                        };
                        tempEvt.answerLeft = new EventAnswer()
                        {
                            swipeSide = "Left",
                            textAnswer = "",
                            results = new List<ResourceResult>()
                        };
                        tempEvents[eventName] = tempEvt;
                    }

                    var eventAnswer = tempEvt.answerRight;
                    switch (answer)
                    {
                        case "Right":
                            break;
                        case "Left":
                            eventAnswer = tempEvt.answerLeft;
                            break;
                    }
                    eventAnswer.textAnswer = textAnswer;

                    var result = new ResourceResult()
                    {
                        resource = resource,
                        min = min,
                        max = max
                    };
                    eventAnswer.results.Add(result);
                }
            }
            sceneData.config.events = tempEvents.Select(x => x.Value).ToList();
            UnityEditorHelper.SetDirtyAndSave(sceneData.config);
        },
        Debug.LogError);
    }

    string GetColumnValue(List<string> row, int col, Dictionary<int, string> tempValue)
    {
        var value = row[col].Trim();
        value = string.IsNullOrWhiteSpace(value) ? tempValue[col] : value;
        tempValue[col] = value;
        return value;
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
        var gameEvent = sceneData.config.events.ElementAt(Random.Range(0, sceneData.config.events.Count));
        sceneData.cardCurrent = Instantiate(sceneData.cardPrefab, sceneData.sceneRoot);
        sceneData.cardCurrent.isActive = true;
        sceneData.cardCurrent.SetGameEvent(gameEvent);
        sceneData.textEventInfo.text = gameEvent.eventInfo;

        gameData.cardCount += 1;
        sceneData.timeInfo.text = "Day : " + gameData.cardCount.ToString();

        var iconPos = sceneData.researchIcon.position;
        iconPos.x = sceneData.research.GetRatio() * 4 - 2f;
        sceneData.researchIcon.position = iconPos;

        var endingReason = "";
        foreach (var res in sceneData.resources)
        {
            res.changeOnLeft = 0;
            res.changeOnRight = 0;
            if (res.isMakeEnding && res.IsEmpty())
            {
                endingReason = res.gameObject.name;
                gameData.SetState(GameState.GameOver);
            }
        }

        if (sceneData.research.IsFull())
        {
            endingReason = sceneData.research.gameObject.name;
            endingReason += (Random.value > 0.1f) ? ".Good" : ".Bad";
            gameData.SetState(GameState.GameOver);
        }

        if (gameData.state == GameState.GameOver)
        {
            sceneData.textEventInfo.text = "GAME OVER";
            sceneData.endingInfo.gameObject.SetActive(true);
            var ending = sceneData.config.GetEnding(endingReason);
            if (ending != null)
            {
                sceneData.endingInfo.text = ending.textInfo;
                sceneData.cardCurrent.ShowEnding(ending);
            }
        }

        foreach (var result in gameEvent.answerLeft.results)
        {
            if (!sceneData.DictResources().ContainsKey(result.resource)) continue;
            var res = sceneData.DictResources()[result.resource];
            res.changeOnLeft = Random.Range(result.min, result.max);
        }

        foreach (var result in gameEvent.answerRight.results)
        {
            if (!sceneData.DictResources().ContainsKey(result.resource)) continue;
            var res = sceneData.DictResources()[result.resource];
            res.changeOnRight = Random.Range(result.min, result.max);
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
