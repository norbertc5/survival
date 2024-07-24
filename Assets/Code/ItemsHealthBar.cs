using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ItemsHealthBar : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Image bar;
    CanvasGroup canvasGroup;

    [SerializeField] float timeToDssolve = 3;
    [SerializeField] float dissolveSpeed = 1;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        transform.LookAt(player);
    }

    public void SetHealthBar(Vector3 pos, float fill)
    {
        transform.position = pos + Vector3.up;
        bar.fillAmount = fill;
        StopAllCoroutines();
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(timeToDssolve);

        while(canvasGroup.alpha > 0)
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
}
