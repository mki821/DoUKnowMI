using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTest : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentPanel;


    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        float y = scrollRect.transform.InverseTransformPoint(contentPanel.position).y - scrollRect.transform.InverseTransformPoint(target.position).y;

        contentPanel.anchoredPosition = new(0, y);
    }

    private void Update() {
        InvokeRepeating(nameof(testrtest), 5, .1f);
    }

    void testrtest() {
        SnapTo((RectTransform)contentPanel.GetChild(4).GetComponent("RectTransform"));
    }

    public void OnChangeScroll(Vector2 value) {
        print(value);
    }
}
