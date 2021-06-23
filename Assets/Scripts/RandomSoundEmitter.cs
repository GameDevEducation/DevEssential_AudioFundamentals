using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundEmitter : MonoBehaviour
{
    [SerializeField] List<AudioClip> Sounds;
    [SerializeField] float EmissionBufferTime = 5f;
    [SerializeField] [Range(0f, 1f)] float EmissionBufferVariation = 0.2f;
    [SerializeField] float MinSoundVolume = 0.8f;
    [SerializeField] float MaxSoundVolume = 0.9f;
    [SerializeField] float MinSoundPitch = 0.8f;
    [SerializeField] float MaxSoundPitch = 1.2f;

    AudioSource LinkedSource;
    float NextEmissionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        LinkedSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the next emission time if needed
        if (NextEmissionTime > 0)
            NextEmissionTime -= Time.deltaTime;

        // time to play sound?
        if (NextEmissionTime <= 0)
        {
            AudioClip selectedClip = Sounds[Random.Range(0, Sounds.Count)];

            // randomise the pitch slightly
            LinkedSource.pitch = Random.Range(MinSoundPitch, MaxSoundPitch);

            // play with a randomised volume
            LinkedSource.PlayOneShot(selectedClip, Random.Range(MinSoundVolume, MaxSoundVolume));

            // set up the next emission time
            NextEmissionTime = EmissionBufferTime * (1f + Random.Range(-EmissionBufferVariation, EmissionBufferVariation));
            NextEmissionTime += selectedClip.length;
        }
    }
}
