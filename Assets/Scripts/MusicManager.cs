using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> Tracks;
    [SerializeField] float FadeInTime = 0f;
    [SerializeField] float FadeOutTime = 0f;

    AudioSource LinkedSource;
    float NextTrackStartTime = 0f;
    int NextTrackIndex = -1;

    float FadeTime = 0f;
    float FadeProgress = 0f;
    bool IsFadeIn = true;

    // Start is called before the first frame update
    void Start()
    {
        LinkedSource = GetComponent<AudioSource>();

        if (Tracks.Count == 0)
        {
            Debug.LogError("No audio tracks are setup");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // no audio tracks configured
        if (Tracks.Count == 0)
            return;

        if (NextTrackStartTime > 0)
            NextTrackStartTime -= Time.deltaTime;

        if (NextTrackStartTime <= 0)
        {
            // use modulus (%) to have the track index wrap back to the start automatically
            NextTrackIndex = (NextTrackIndex + 1) % Tracks.Count;

            // select the clip
            LinkedSource.clip = Tracks[NextTrackIndex];
            LinkedSource.Play();

            // set the next track's start time
            NextTrackStartTime = LinkedSource.clip.length;

            // setup the fade in
            FadeTime = FadeInTime;
            FadeProgress = 0f;
            IsFadeIn = true;
        }

        // within FadeOutTime of the end and not currently fading volume
        if (NextTrackStartTime <= FadeOutTime && FadeTime == 0)
        {
            FadeTime = FadeOutTime;
            FadeProgress = 0f;
            IsFadeIn = false;
        }

        // is a fade in progress?
        if (FadeTime > 0)
        {
            // update progress
            FadeProgress += Time.deltaTime / FadeTime;

            // update volume
            LinkedSource.volume = IsFadeIn ? FadeProgress : (1f - FadeProgress); 

            // fade completed?
            if (FadeProgress >= 1f)
                FadeTime = 0f;
        }
    }
}
