using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Name - SFX Object", menuName = "SFX Object")]
public class SFXObject : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    public bool loop;

    [Range(0.0f, 1.0f)]
    public float volume;

    [Header("Pitch")]
    public bool randomizePitch;

    [Range(0.0f, 1.0f)]
    public float pitchRangeLowerValue;
    [Range(0.0f, 1.0f)]
    public float pitchRangeHigherValue;

    [Header("Randomize Clips")]
    public bool randomizeClips;

    public List<AudioClip> audioClips = new List<AudioClip>();
}
