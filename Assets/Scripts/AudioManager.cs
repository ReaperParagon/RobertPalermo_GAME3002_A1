using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_AWhistle;

    [SerializeField]
    private AudioClip m_AGoal;

    private AudioSource m_Audio;

    private void Start()
    {
        m_Audio = GetComponent<AudioSource>();

        Assert.IsNotNull(m_Audio, "Problem: AudioSource not attached");
        Assert.IsNotNull(m_AGoal, "Problem: Goal Audio not attached");
        Assert.IsNotNull(m_AWhistle, "Problem: Whistle Audio not attached");
    }

    public void PlayWhistle()
    {
        m_Audio.clip = m_AWhistle;
        m_Audio.Play();
    }

    public void PlayGoal()
    {
        m_Audio.clip = m_AGoal;
        m_Audio.Play();
    }
}
