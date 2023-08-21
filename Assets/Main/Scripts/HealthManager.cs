using System;
using System.Collections;
using System.Collections.Generic;
using domi.DB;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    static long TIME_LEFT = 6000000000;
    static HealthManager instance;
    public static int health {
        get => instance._db.health;
    }
    DBstruct _db;

    [SerializeField] TextMeshProUGUI _healthT;
    [SerializeField] TextMeshProUGUI _timeT;
    bool activeTimer = false;

    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        _db = DBmanager.GetData();
        UpdateText();

        // test
        Give(5);
        Try(3);
    }

    private void Update() {
        if (health < 5) {
            long diff = (DateTime.Now.Ticks - _db.healthTime) / TIME_LEFT;
            long RemainTime = (TIME_LEFT - (DateTime.Now.Ticks - _db.healthTime)) / 10000000 /* 1초 */;
            RemainTime = Math.Max(RemainTime, 0);

            if (!activeTimer) {
                activeTimer = true;
                _timeT.DOFade(.85f, 0.3f);
            }

            _timeT.text = $"{(RemainTime / 60).ToString().PadLeft(2, '0')}:{(RemainTime % 60).ToString().PadLeft(2, '0')}";

            if (diff >= 1) {
                Give((int)diff);
                _db.healthTime = DateTime.Now.Ticks;
                print(health);
            }
        } else if (activeTimer) {
            activeTimer = false;
            _timeT.DOFade(0, 0.3f);
        }
    }

    public static void Give(int value = 1) {
        instance._db.health = Mathf.Min(instance._db.health + value, 5);
        DBmanager.Save();
        UpdateText();
    }

    public static bool Try(int value = 1) {
        if (instance._db.health - value < 0) return false;

        int lastNum = instance._db.health;
        instance._db.health -= value;

        if (lastNum == 5 && instance._db.health < 5)
            instance._db.healthTime = DateTime.Now.Ticks;

        DBmanager.Save();
        UpdateText();
        return true;
    }

    static void UpdateText() {
        instance._healthT.text = instance._db.health.ToString();
    }
}
