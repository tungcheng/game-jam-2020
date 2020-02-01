using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourceData : MonoBehaviour
{
    public SpriteRenderer fillRender;
    public bool isHaveHint;
    public bool isMakeEnding;
    public GameObject hint;

    public float amountMax = 100f;
    public float amountStart = 50f;
    public float amount;

    public float changeOnLeft;
    public float changeOnRight;

    // Start is called before the first frame update
    public void SetStartNew()
    {
        amount = amountStart;
        changeOnLeft = 0f;
        changeOnRight = 0f;
        SetFill();
        HideHint();
    }

    public bool IsFull()
    {
        return (amount >= amountMax);
    }

    public bool IsEmpty()
    {
        return (amount <= 0);
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }

    public void ShowHint(float change, float decideDelta)
    {
        if (change == 0)
        {
            hint.SetActive(false);
            return;
        }
        hint.SetActive(true && isHaveHint);
        var hintRender = hint.GetComponent<SpriteRenderer>();
        var color = Color.white;
        color.a = Mathf.Abs(decideDelta);
        hintRender.color = color;

        hint.transform.localScale = ((change > 2) ? 0.22f : 0.12f) * Vector3.one;
    }

    public void SetChange(float change)
    {
        amount += change;
        amount = Mathf.Clamp(amount, 0, amountMax);
        SetFill();

        var hintRender = hint.GetComponent<SpriteRenderer>();
        var color = Pool.Get<SceneData>().GetColorChange(change);
        hintRender.color = Color.white;
        hintRender.DOColor(color, 0.4f)
            .OnComplete(() =>
            {
                HideHint();
            });
        amount += change;
    }

    public void SetFill()
    {
        var fill = Mathf.Clamp01(amount / amountMax);
        fillRender.sharedMaterial.SetFloat("_FillY", fill);
    }
}
