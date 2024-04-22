using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM : MonoBehaviour
{
    public AudioSource MainBGMSound;

    void Start()
    {
        MainBGMSound.Play();
    }
}
