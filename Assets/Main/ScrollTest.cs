using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTest : MonoBehaviour, domiSliceScreen.ISlideEvent
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentPanel;

    private void Update() {
        // InvokeRepeating(nameof(testrtest), 5, .1f);
    }

    void testrtest() {
        // SnapTo((RectTransform)contentPanel.GetChild(4).GetComponent("RectTransform"));
    }

    public void OnChangeScroll(Vector2 value) {
        print(value);
    }

    public void OnSlideOpen()
    {
        print("열렸다");
    }

    public void OnSlideClose()
    {
        print("오 닫혔는데?");
    }
}
