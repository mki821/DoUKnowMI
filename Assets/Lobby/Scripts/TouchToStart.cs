using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TouchToStart : MonoBehaviour
{
    [SerializeField] Image _toutchBar;

    private void Start() 
    {
        _toutchBar.DOFade(1, .4f).SetLoops((int)LoopType.Yoyo);
    }
    
    void Update()
    {
        if (Input.touchCount > 0) {
            SceneManager.LoadScene("Main");
        }
    }
}
