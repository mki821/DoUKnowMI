using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TouchToStart : MonoBehaviour
{
    [SerializeField] CanvasGroup _toutchBar;

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
