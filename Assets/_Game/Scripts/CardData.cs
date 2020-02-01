using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardData : MonoBehaviour
{
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    public bool isActive
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }

    public enum CardState
    {
        Apear,
        CanDrag,
        FlipOut,
        Return
    }

    public CardState state;

    public void SetState(CardState state)
    {
        this.state = state;
    }

    public SpriteRenderer spriteRenderer;
    public TMP_Text textRight;
    public TMP_Text textLeft;
    public TMP_Text textCharacter;

    private void Start()
    {
        SetState(CardState.Apear);
        var color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
        spriteRenderer.DOFade(1f, 0.3f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                SetState(CardState.CanDrag);
            });

        HideText();
        textCharacter.gameObject.SetActive(true);
    }

    private void Update()
    {
        var cardStart = Pool.Get<SceneData>().cardStartPos;
        if (state == CardState.Return)
        {
            position = Vector3.MoveTowards(position, cardStart.position, 10f * Time.deltaTime);
            SetRotationByDeltaX();

            if (position == cardStart.position)
            {
                rotation = cardStart.rotation;
                SetState(CardState.CanDrag);
            }
        }
        else if (state == CardState.FlipOut)
        {
            SetRotationByDeltaX();
        }
    }

    public void SetPosByDrag(Vector3 dragDelta)
    {
        this.position += new Vector3(dragDelta.x, dragDelta.y);
        SetRotationByDeltaX();
        SetTextChoiceByDeltaX();
    }

    public void SetFlipOut()
    {
        SetState(CardState.FlipOut);
        var sequence = DOTween.Sequence();
        var outPos = position + Mathf.Sign(position.x) * Vector3.right * 10f;
        sequence.Insert(0f, transform.DOMove(outPos, 1f));
        sequence.Insert(0f, spriteRenderer.DOFade(0f, 0.3f));
        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
        HideText();
    }

    public void SetReturn()
    {
        SetState(CardState.Return);
    }

    public void SetRotationByDeltaX()
    {
        var cardStart = Pool.Get<SceneData>().cardStartPos;
        var delta = this.position - cardStart.position;
        var angleZ = -delta.x * 10f;
        angleZ = (angleZ < -120f) ? -120f : angleZ;
        angleZ = (angleZ > 120f) ? 120f : angleZ;
        this.rotation = Quaternion.Euler(0f, 0f, angleZ);
    }

    public void SetTextChoiceByDeltaX()
    {
        var cardStart = Pool.Get<SceneData>().cardStartPos;
        var delta = this.position - cardStart.position;
        textLeft.gameObject.SetActive(delta.x < 0f);
        textRight.gameObject.SetActive(delta.x > 0f);
    }

    public void HideText()
    {
        textLeft.gameObject.SetActive(false);
        textRight.gameObject.SetActive(false);
        textCharacter.gameObject.SetActive(false);
    }
}
