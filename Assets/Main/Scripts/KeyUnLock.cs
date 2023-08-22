using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domi.DB;

namespace MainScroll {
    public class KeyUnLock : MonoBehaviour
    {
        ScrollEvent _event;
        DBstruct _db;
        public static int NEED_KEY = 5;
        public static KeyUnLock instance;

        private void Awake() {
            if (instance == null)
                instance = this;

            _event = GetComponent<ScrollEvent>();
        }

        private void Start() {
            _db = DBmanager.GetData();
            // DBmanager.Reset(true);
            // _db.keyAmount = 8; // test
        }

        public void TryUnlock(Transform _transform, int stage, MapTheme theme) {
            if ((stage / 20) != _db.keyUnlock + 1) {
                domiAlertSys.Show("아직 전 스테이지에서 잠금이 풀리지 않았습니다.", new Color32(230,100,100, 255));
                return;
            }

            if (_db.keyAmount - NEED_KEY < 0) {
                domiAlertSys.Show("열쇠 조각이 부족합니다. 필요한 열쇠조각: "+ (NEED_KEY - _db.keyAmount) + "개", new Color32(230,100,100, 255));
                return;
            }

            _db.keyAmount -= NEED_KEY;
            _db.keyUnlock ++;
            DBmanager.Save();

            domiAlertSys.Show("잠금 해제! 남은 열쇠조각: "+ _db.keyAmount + "개", new Color(100, 230, 100, 255));
            _event.OnCreateStage(_transform, stage, 1, theme);
        }
    }
}
