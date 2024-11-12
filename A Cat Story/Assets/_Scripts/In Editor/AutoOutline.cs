using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoOutline : MonoBehaviour
{
    List<ParticleSystem> particleSystemsTopBottom = new List<ParticleSystem>();
    List<ParticleSystem> particleSystemsLeftRight = new List<ParticleSystem>();

#if UNITY_EDITOR
    private void Start()
    {
        CollectParticleSystems();
    }

    public void CollectParticleSystems()
    {
        particleSystemsTopBottom.Add(transform.GetChild(0).GetComponent<ParticleSystem>());
        particleSystemsTopBottom.Add(transform.GetChild(1).GetComponent<ParticleSystem>());

        particleSystemsLeftRight.Add(transform.GetChild(2).GetComponent<ParticleSystem>());
        particleSystemsLeftRight.Add(transform.GetChild(3).GetComponent<ParticleSystem>());
    }

    public void ResizeOutlineParticles()
    {
        float topBottomScale = transform.parent.transform.localScale.x;
        float leftRightScale = transform.parent.transform.localScale.y;

        //top and bottom
        foreach (ParticleSystem targetSystem in particleSystemsTopBottom)
        {
            var shape = targetSystem.shape;
            shape.scale = new Vector3(topBottomScale, shape.scale.y, shape.scale.z);
        }

        //left and right
        foreach (ParticleSystem targetSystem in particleSystemsLeftRight)
        {
            var shape = targetSystem.shape;
            shape.scale = new Vector3(leftRightScale, shape.scale.y, shape.scale.z);
        }
    }

#endif
}
