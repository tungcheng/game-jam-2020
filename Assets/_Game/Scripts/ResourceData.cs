using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourceData : MonoBehaviour
{
    public SpriteRenderer fillRender;
    public GameObject hint;

    public float amountMax = 100f;
    public float amount = 100f;

    public float changeOnLeft;
    public float changeOnRight;

    // Start is called before the first frame update
    void Start()
    {
        SetFill();
        HideHint();
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }

    public void ShowHint(float change, float decideDelta)
    {
        if (change == 0) return;
        hint.SetActive(true);
        var hintRender = hint.GetComponent<SpriteRenderer>();
        var color = Color.white;
        color.a = Mathf.Abs(decideDelta);
        hintRender.color = color;

        hint.transform.localScale = ((change > 2) ? 0.22f : 0.12f) * Vector3.one;
    }

    public void SetChange(float change)
    {
        amount += change;
        amount = Mathf.Clamp(amount, 0f, 100f);
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
