using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    [SerializeField] private float distance;

    private RectTransform _ractTransform;

    private void Awake() {
        _ractTransform = (RectTransform)GetComponent("RectTransform");
    }

    public void ButtonUp() {
        _ractTransform.position += -Vector3.up * distance;
    }

    public void ButtonDown() {
        _ractTransform.position += Vector3.up * distance;
    }
}
