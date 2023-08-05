using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace domiSliceScreen {
    public interface ISlideEvent {
        public void OnSlideOpen();
        public void OnSlideClose();
    }

    [System.Serializable]
    class SlideContent {
        public string name;
        public RectTransform screen;
    }

    public class SlideScreen : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] int defaultScreen;
        [SerializeField] SlideContent[] screens;

        [Header("Resources")]
        [SerializeField] Transform oneSection;
        [SerializeField] Sprite oneImage;
        [SerializeField] TextMeshProUGUI StageText;

        int currentID;

        private void Awake() {
            for (int i = 0; i < screens.Length; i++)
            {
                var obj = new GameObject("icon");
                obj.transform.parent = oneSection;

                var objTransform = obj.AddComponent<RectTransform>();
                objTransform.sizeDelta = Vector2.one * 30;

                var objSprite = obj.AddComponent<Image>();
                objSprite.sprite = oneImage;
                objSprite.color = new(1,1,1, .5f);
            }

            currentID = defaultScreen;
        }

        private void Start() {
            ImmediatelyChange();
        }

        public void ChangeScreen(bool left) {
            int oldID = currentID;
            int Plus = left ? -1 : 1;
            if ((currentID + Plus) < 0 || (currentID + Plus) >= screens.Length) return;

            currentID += Plus;
            print("page change - "+currentID);

            var screen = screens[currentID];
            float width = transform.root.GetComponent<Canvas>().GetComponent<RectTransform>().rect.width + 500;

            screen.screen.offsetMin = new Vector2(-width, 0);
            screen.screen.offsetMax = -new Vector2(width, 0);

            if (!left) { // 반대
                screen.screen.offsetMin *= -1;
                screen.screen.offsetMax *= -1;
            }

            screen.screen.DOAnchorPos(Vector3.zero, .3f);
            ImmediatelyChange(true);
        }

        public void ImmediatelyChange(bool disable = false) {
            var screen = screens[currentID];
            if (!disable)
                screen.screen.anchoredPosition = Vector3.zero;

            screen.screen.transform.SetSiblingIndex(transform.childCount - 1 - 1);
            StageText.text = screen.name;
            UpdateOne();
        }

        void UpdateOne() {
            for (int i = 0; i < oneSection.childCount; i++)
                oneSection.GetChild(i).GetComponent<Image>().color = currentID == i ? new(1,1,1,1) : new(1,1,1, .5f);
        }
    }
}