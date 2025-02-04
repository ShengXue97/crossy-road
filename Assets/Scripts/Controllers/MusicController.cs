﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Sprite musicOn;
    public Sprite musicOff;
    public GameObject musicButton;
    public AudioSource audio;
    public bool audioEnabled;

    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
        string audioon = "1";
        if (PlayerPrefs.HasKey("audioon"))
        {
            audioon = PlayerPrefs.GetString("audioon");
        }

        if (audioon == "1")
        {
            audio.Play();
            musicButton.GetComponent<Image>().sprite = musicOn;
            audioEnabled = true;
        }
        else
        {
            audio.Stop();
            musicButton.GetComponent<Image>().sprite = musicOff;
            audioEnabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void toggleMusic()
    {
        if (audio.isPlaying)
        {
            audio.Stop();
            musicButton.GetComponent<Image>().sprite = musicOff;
            audioEnabled = false;
            PlayerPrefs.SetString("audioon", "0");
        }
        else
        {
            audio.Play();
            musicButton.GetComponent<Image>().sprite = musicOn;
            audioEnabled = true;
            PlayerPrefs.SetString("audioon", "1");
        }
    }
}
