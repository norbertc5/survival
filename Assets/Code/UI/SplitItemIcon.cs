using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SplitItemIcon : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] StackSplit stackSplit;
    [SerializeField] InputAction mousePos;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForClickAndDrop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator WaitForClickAndDrop()
    {
        yield return new WaitForSeconds(.1f);  // delay to avoid drop instantly
        
        // if clicked on empty space, drop separated slot
        while(true)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Inventory.Drop(StackSplit.separatedSlot);
                StackSplit.ToggleSplitIcon(false);
            }
            yield return null;
        }
    }

    public void Update()
    {
        // moving this icon with mouse
        // it's working differently to standard inventory icons
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            mousePos.ReadValue<Vector2>(), canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }
}
