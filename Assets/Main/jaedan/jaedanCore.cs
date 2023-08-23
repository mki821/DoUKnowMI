using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domi.DB;
using System;

public class jaedanCore : MonoBehaviour
{
    DBstruct _db;
    
    private void Start() {
        _db = DBmanager.GetData();
    }

    public void ClickJaedan() {
        DateTime NowTime = DateTime.Now;
        
        if (NowTime.Day == _db.jeadanTime) { // 날짜 안바뀜
            TimeSpan reamingTime = new DateTime(NowTime.Year, NowTime.Month, NowTime.Day, 23, 59, 59) - NowTime;
            print($"{reamingTime.Hours} : {reamingTime.Minutes}");
            domiAlertSys.Show($"사용할 수 없습니다. 내일 다시 와주세요! 남은시간: {reamingTime.Hours.ToString().PadLeft(2, '0')}시 {reamingTime.Minutes.ToString().PadLeft(2, '0')}분", new Color32(230,100,100, 255));
            return;
        }

        _db.jeadanTime = NowTime.Day;
        HealthManager.Give(5, true);

        DBmanager.Save();
        domiAlertSys.Show("제단을 사용하여 체력 5개를 받았습니다.", new Color(100, 230, 100, 255));
    }
}
