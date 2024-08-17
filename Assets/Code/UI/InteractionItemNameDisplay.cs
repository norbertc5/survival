using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionItemNameDisplay : MonoBehaviour
{
    TextMeshProUGUI itemNameText;
    CanvasGroup canvasGroup;

    void Start()
    {
        itemNameText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void ShowAndSet(string itemName)
    {
        canvasGroup.alpha = 1;
        itemNameText.text = itemName;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
