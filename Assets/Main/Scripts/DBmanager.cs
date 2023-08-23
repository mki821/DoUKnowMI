using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace domi.DB {
    public class DBstruct {
        public int health = 20;
        public int clearStage = 0;
        public string healthTime = "0";
        public int jeadanTime = 0;
        public int keyAmount = 0;
        public int keyUnlock = 0;
        public List<int> takenKeyStage = new List<int>();
    }

    public class DBmanager : MonoBehaviour
    {
        static readonly string SAVE_KEY = "domiSaveData";
        private static DBstruct SData;
        
        private void Awake() {
            string deviceData = PlayerPrefs.GetString(SAVE_KEY);
            print(deviceData);
            if (deviceData.Length > 0) {
                SData = JsonMapper.ToObject<DBstruct>(deviceData);
            } else {
                SData = new();
            }
        }

        private void OnDestroy() {
            SData = null;
        }

        static public DBstruct GetData() {
            return SData;
        }

        static public void Save() {
            PlayerPrefs.SetString(SAVE_KEY, JsonMapper.ToJson(SData));
        }

        static public void Reset(bool sure) {
            if (!sure) throw new System.Exception("[DB] 초기화 하려면 1 인수가 동의되어야 합니다.");
            PlayerPrefs.DeleteKey(SAVE_KEY);
            SData = new();
        }
    }
}

