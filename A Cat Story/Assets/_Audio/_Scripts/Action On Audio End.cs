using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnAudioEnd : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private UnityEvent actionOnAudioEnd = new();

    public void TriggerActionsOnAudioEnd() => StartCoroutine(AwaitPlayingToEnd());

    IEnumerator AwaitPlayingToEnd()
    {
        while (audioSource.isPlaying == true)
        {
            yield return null;
        }

        actionOnAudioEnd.Invoke();
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
