using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MainScroll
{
    [System.Serializable]
    public class StageMap {
        public GameObject prefab;
    }
    
    [System.Serializable]
    public class padding {
        public float top;
        public float bottom;
    }
    public class ScrollContentCreate : MonoBehaviour
    {
        [SerializeField] StageMap[] _stageMaps;
        [SerializeField] RectTransform contentBox;
        [SerializeField] int maxStage = 999;
        [SerializeField] padding _padding;
        
        RectTransform _canvas;

        int nowIndex = 0;
        int nowStage = 0;

        private void Awake() {
            _canvas = (RectTransform)transform.parent.GetComponent("RectTransform");
        }

        private void Start() {
            Canvas.ForceUpdateCanvases(); // 캔버스 업뎃 시키고
            float now_height;
            float phone_hieght = _canvas.rect.height; // 그 다음 높이 구함
            
            do {
                AddStage(); // 추가 하고
                Canvas.ForceUpdateCanvases(); // 업뎃
                now_height = contentBox.rect.height;
            } while(now_height < phone_hieght);
        }

        void AddStage() {
            if (nowStage > maxStage) return;

            StageMap map = _stageMaps[nowIndex % _stageMaps.Length];
            var clone = Instantiate(map.prefab, contentBox.transform).transform;

            // List<Transform> stageButtons = new();
            for (int i = 0; i < clone.childCount; i++)
            {
                var child = clone.GetChild(i);
                if (child.tag != "UIstageMap") continue;
                nowStage ++;
                if (nowStage > maxStage) {
                    Destroy(child.gameObject);
                    break;
                }

                child.GetComponentInChildren<TextMeshProUGUI>().text = nowStage.ToString();
            }

            if (nowStage > maxStage) { // 더이상 소환 불가
                var paddingT = new GameObject("Padding").AddComponent<RectTransform>();
                paddingT.transform.SetParent(contentBox.transform);
                paddingT.sizeDelta = new Vector2(0,_padding.bottom);
            }

            nowIndex ++;
        }
    
        public void checkScroll(Vector2 pos) {
            if (pos.y <= 0) {
                AddStage();
                Canvas.ForceUpdateCanvases();
            }
        }
    }
}
