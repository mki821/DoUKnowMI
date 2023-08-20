using System;
using System.Collections;
using System.Collections.Generic;
using domi.DB;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    static HealthManager instance;
    public static int health {
        get => instance._db.health;
    }
    DBstruct _db;

    [SerializeField] TextMeshProUGUI _healthT;

    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        _db = DBmanager.GetData();
        UpdateText();

        // test
        Give(5);
        Try(1);
    }

    private void Update() {
        if (health < 5) {
            print(DateTime.Now.Ticks - _db.healthTime);
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
