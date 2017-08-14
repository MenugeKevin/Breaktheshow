using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpectatorFeedback : MonoBehaviour {

    public List<AudioClip> badFeedback = new List<AudioClip>();
    public List<AudioClip> goodFeedback = new List<AudioClip>();

    private AudioSource m_audioSource;

    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void playGoodFeedback()
    {
        if (goodFeedback.Count > 0)
        {
            AudioClip clip = goodFeedback[Random.Range(0, goodFeedback.Count)];
            m_audioSource.clip = clip;
            m_audioSource.Play();
        }
    }

    public void playBadFeedback()
    {
        if (badFeedback.Count > 0)
        {
            AudioClip clip = badFeedback[Random.Range(0, goodFeedback.Count)];
            m_audioSource.clip = clip;
            m_audioSource.Play();
        }
    }

}
