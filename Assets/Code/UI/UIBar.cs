using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

[RequireComponent(typeof(CanvasGroup))]
public class UIBar : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Image bar;
    Image background;
    Coroutine backgroundBlinkRoutine;
    Coroutine dissolveRoutine;
    TextMeshProUGUI valueText;

    [SerializeField] float timeToDssolve = 3;
    [SerializeField] float dissolveSpeed = 1;
    Color32 defaultBackgroundColor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        bar = transform.Find("Bar").GetComponent<Image>();
        background = transform.Find("Bg").GetComponent<Image>();
        valueText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        defaultBackgroundColor = background.color;
    }

    public virtual void SetBar(float fill)
    {
        HandleSetBar(fill);
    }

    public virtual void SetBar(float fill, Vector3 pos)
    {
        // overload is to make possiblity to override in inherit classes
        HandleSetBar(fill);
    }

    void HandleSetBar(float fill)
    {
        bar.fillAmount = fill;
        if (valueText != null)
        {
            valueText.text = (fill*10).ToString();
        }
    }

    public void Show()
    {
        if (dissolveRoutine != null) StopCoroutine(dissolveRoutine);
        dissolveRoutine = StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(timeToDssolve);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= dissolveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// Transform actulal value and max value of something into bar fill
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static float ValueToBarFill(int value, int maxValue)
    {
        return (float)value / maxValue;
    }

    /// <summary>
    /// Make background of the bar blinking.
    /// </summary>
    /// <param name="blinkColor"></param>
    /// <param name="amount"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public void BackgroundBlink(Color32 blinkColor, int amount, float duration)
    {
        if (backgroundBlinkRoutine != null) StopCoroutine(backgroundBlinkRoutine);
        backgroundBlinkRoutine = StartCoroutine(BackgroundBlinkRoutine(blinkColor, amount, duration));
    }

    IEnumerator BackgroundBlinkRoutine(Color32 blinkColor, int amount, float duration)
    {
        blinkColor.a = defaultBackgroundColor.a;

        for (int i = 0; i < amount; i++)
        {
            background.color = blinkColor;
            yield return new WaitForSeconds(duration);
            background.color = defaultBackgroundColor;
            yield return new WaitForSeconds(duration);
        }
    }
}
