using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    [SerializeField] private float distance;

    private RectTransform _ractTransform;

    public void ButtonUp() {
        _ractTransform.position += -Vector3.up * distance;
    }

    public void ButtonDown() {
        _ractTransform.position += Vector3.up * distance;
    }
}
