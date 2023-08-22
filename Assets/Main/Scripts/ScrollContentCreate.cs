using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MainScroll
{
    public enum MapTheme {
        forest,
        sea,
        volcano,
        snow
    }

    [System.Serializable]
    public class StageMap {
        public GameObject prefab;
        public MapTheme theme;
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

        ScrollEvent _event;
        RectTransform _canvas;

        int nowIndex = 0;
        int nowStage = 0;

        float phone_hieght;
        float phone_weight;

        private void Awake() {
            _canvas = (RectTransform)transform.parent.GetComponent("RectTransform");
            _event = GetComponent<ScrollEvent>();
        }

        private void Start() {
            Canvas.ForceUpdateCanvases(); // 캔버스 업뎃 시키고
            float now_height;
            phone_hieght = _canvas.rect.height; // 그 다음 높이 구함
            phone_weight = _canvas.rect.width; // 이건 넓이 구함
            
            _event.OnInit(this);
            do {
                AddStage(); // 추가 하고
                Canvas.ForceUpdateCanvases(); // 업뎃
                now_height = contentBox.rect.height;
            } while(now_height < phone_hieght);
        }

        internal bool AddStage() {
            if (nowStage > maxStage) return false;

            StageMap map = _stageMaps[nowIndex % _stageMaps.Length];
            var clone = Instantiate(map.prefab, contentBox.transform).transform;
            var cloneT = (RectTransform)clone.GetComponent("RectTransform");

            // 리사이징
            float zoom = phone_weight / cloneT.rect.width;
            cloneT.sizeDelta = new(phone_weight, phone_weight * cloneT.rect.height / cloneT.rect.width);

            // List<Transform> stageButtons = new();
            for (int i = 0; i < clone.childCount; i++)
            {
                var child = clone.GetChild(i);
                if (child.tag != "UIstageMap") continue;
                nowStage ++;
                if (nowStage > maxStage) {
                    Destroy(child.gameObject);
                    continue;
                }
                
                _event.OnCreateStage(child, nowStage, zoom, map.theme);
            }

            if (nowStage > maxStage) { // 더이상 소환 불가
                var paddingT = new GameObject("Padding").AddComponent<RectTransform>();
                paddingT.transform.SetParent(contentBox.transform);
                paddingT.sizeDelta = new Vector2(0,_padding.bottom);
            }

            nowIndex ++;
            return true;
        }

        // 0: StageIndex 1: ChildIndex
        internal int[] GetMapIndexToStage(int stage) {
            int index = 0;
            int stageCount = 0;
            
            while (true) {
                int levelAmount = GetStageAmountToMap(index % _stageMaps.Length);
                for (int i = 0; i < levelAmount; i++) {
                    stageCount ++;
                    if (stageCount == stage) {
                        return new int[] { index + 1 /* padding 블럭이 있기 때문에 padding index 건너뜀 */, i };
                    }
                }
                index ++;
            }
        }

        internal Vector2 SnapTo(RectTransform target, float plus = 0) {
            Canvas.ForceUpdateCanvases();
            float y = transform.InverseTransformPoint(contentBox.position).y - transform.InverseTransformPoint(target.position).y;
            return contentBox.anchoredPosition = new(0, y + plus);
        }
        internal Vector2 SnapTo(int childIndex, int ofChild, float plus)
        {
            RectTransform target = contentBox.GetChild(childIndex).GetChild(ofChild).GetComponent<RectTransform>();
            return SnapTo(target, plus);
        }
        internal int GetStageAmountToMap(int index) {
            int k = 0;
            for (int i = 0; i < _stageMaps[index].prefab.transform.childCount; i++)
                if (_stageMaps[index].prefab.transform.GetChild(i).CompareTag("UIstageMap"))
                    k ++;
            
            return k;
        }

        public void checkScroll(Vector2 pos) {
            if (pos.y <= 0) {
                AddStage();
                Canvas.ForceUpdateCanvases();
            }
        }
    }
}
