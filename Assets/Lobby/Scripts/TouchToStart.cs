using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TouchToStart : MonoBehaviour
{
    [SerializeField] CanvasGroup _toutchBar;

    private void Awake() {
        #if UNITY_IOS || UNITY_ANDROID
            Application.targetFrameRate = 144;
        #else
            QualitySettings.vSyncCount = 1;
        #endif
    }

    private void Start() 
    {
        _toutchBar.DOFade(0, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    
    void Update()
    {
        if (Input.touchCount > 0) {
            SceneManager.LoadScene("Main");
        }
    }
}
