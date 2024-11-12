using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndingAbility : MonoBehaviour
{
    Animator playerVisualAnimator;

    public UnityEvent onAnimationEvent = new();

    // Start is called before the first frame update
    void Start()
    {
        playerVisualAnimator = GetComponent<Animator>();
    }

    public void StopAbilityAnimation()
    {
        playerVisualAnimator.SetBool("isAbility", false);
        playerVisualAnimator.SetBool("IsScalingDown", false);
        playerVisualAnimator.SetBool("IsScalingUp", false);
    }

    public void TriggerAnimationEvent()
    {
        onAnimationEvent.Invoke();
    }
}
