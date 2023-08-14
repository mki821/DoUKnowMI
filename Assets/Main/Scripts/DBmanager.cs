using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace domi.DB {
    public class DBstruct {
        public int health = 5;
    }

    public class DBmanager : MonoBehaviour
    {
        static readonly string SAVE_KEY = "domiSaveData";
        private static DBstruct SData;
        
        private void Awake() {
            string deviceData = PlayerPrefs.GetString(SAVE_KEY);
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

