using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domi.DB;
using TMPro;
using UnityEngine.UI;

namespace MainScroll {
    public class ScrollEvent : MonoBehaviour
    {
        [SerializeField] RectTransform ContentBox;
        RectTransform _canvas;
        DBstruct _db;

        private void Awake() {
            _canvas = (RectTransform)transform.parent.GetComponent("RectTransform");
        }

        internal void OnInit(ScrollContentCreate _base) {
            _db = DBmanager.GetData();
            // _db.clearStage = 998; // TEST

            int[] index_Conf = _base.GetMapIndexToStage(_db.clearStage + 1);

            for (int i = 0; i < index_Conf[0]; i++)
            {
                _base.AddStage();
            }

            float ScrollY = _base.SnapTo(index_Conf[0], index_Conf[1], -500).y;
            Canvas.ForceUpdateCanvases();

            StartCoroutine(Wiatcorrection(_base, ScrollY));
        }

        IEnumerator Wiatcorrection(ScrollContentCreate _base, float ScrollY) {
            yield return null;
            while (ScrollY - ContentBox.anchoredPosition.y > 5) {
                if (!_base.AddStage()) break;

                ContentBox.anchoredPosition = new Vector2(0, ScrollY);
                Canvas.ForceUpdateCanvases();
                yield return null;
            }
        }
        internal void OnCreateStage(Transform _transform, int stage, float scale, MapTheme theme) {
            // 스케일 수정
            var rect = _transform.GetComponent<RectTransform>();
            rect.anchoredPosition *= scale;
            rect.sizeDelta *= scale;

            if (stage != 1 && (stage % 20) == 1) {
                _transform.Find("Lock").gameObject.SetActive(true);
            }

            var textMesh = _transform.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = stage.ToString();

            if ( stage > _db.clearStage + 1 ) {
                textMesh.color = new Color32(255, 255, 255, 150);
                _transform.GetComponent<Image>().color = new Color32(50,50,50, 150);
                return;
            }

            textMesh.color = new Color32(0, 0, 0, 230);
            if (stage == _db.clearStage + 1) {
                _transform.GetComponent<Image>().color = new Color32(243, 240, 102, 255);
            } else {
                _transform.GetComponent<Image>().color = new Color32(111, 212, 43, 255);
            }

            _transform.gameObject.AddComponent<Button>().onClick.AddListener(() => {
                CreateBlock.stageInfo = stage;
                CreateBlock.stageTileType = (int)theme;
                // theme 어디에다가 넣지? (⊙_⊙)？
                UnityEngine.SceneManagement.SceneManager.LoadScene("DAZB3");
            });
        }
    }
}
