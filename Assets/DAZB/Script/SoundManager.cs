using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    [SerializeField] private AudioClip Eat;
    [SerializeField] private AudioClip FirstDash;
    [SerializeField] private AudioClip GetKey;
    [SerializeField] private AudioClip TouchToStart;
    [SerializeField] private AudioClip CantGoStage;
    [SerializeField] private AudioClip BntClick;

    new private AudioSource audio;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        audio = GetComponent<AudioSource>();
    }

    public void EatSound() {
        audio.clip = Eat;
        audio.Play();
    }

    public void FirstDashSound() {
        audio.clip = FirstDash;
        audio.Play();
    }

    public void GetKeySound() {
        audio.clip = GetKey;
        audio.Play();
    }

    public void TouchToStartSound() {
        audio.clip = TouchToStart;
        audio.Play();
    }

    public void CantGoStageSound() {
        audio.clip = CantGoStage;
        audio.Play();
    }

    public void BntClickSound() {
        audio.clip = BntClick;
        audio.Play();
    }
}
