using System.Collections;
using System.Collections.Generic;
using domi.DB;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    static HealthManager instance;
    DBstruct _db;

    [SerializeField] TextMeshProUGUI _healthT;

    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        _db = DBmanager.GetData();
        UpdateText();
    }

    public static void Give(int value = 1) {
        instance._db.health = Mathf.Min(instance._db.health + value, 5);
        DBmanager.Save();
        UpdateText();
    }

    public static bool Try(int value = 1) {
        if (instance._db.health - value < 0) return false;

        instance._db.health -= value;
        DBmanager.Save();
        UpdateText();
        return true;
    }

    static void UpdateText() {
        instance._healthT.text = instance._db.health.ToString();
    }
}
