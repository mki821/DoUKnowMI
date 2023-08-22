using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class domiAlertSys : MonoBehaviour
{
    [SerializeField] CanvasGroup _window;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _color;

    Sequence _sequence;
    
    static domiAlertSys instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    public static void Show(string text, Color color) {
        instance._text.text = text;
        instance._color.color = color;

        if (instance._sequence != null)
            instance._sequence.Kill();

        instance._sequence = DOTween.Sequence();
        instance._window.gameObject.SetActive(true);
        instance._sequence.Append(instance._window.DOFade(1, 0.3f));
        instance._sequence.Append(instance._window.DOFade(0, 0.3f).SetDelay(5f).OnComplete(() => instance._window.gameObject.SetActive(false)));
    }
}
