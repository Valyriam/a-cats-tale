using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHazard : MonoBehaviour
{
    [SerializeField] float attackSpeed;
    float attackTimer = 0;
    [SerializeField] GameObject contractTrigger;
    [SerializeField] LayerMask playerLayer;
    Animator animator;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= attackSpeed)
        {
            animator.enabled = true;
            attackTimer = 0;
        }

        //if collider hits player with player
        if(Physics2D.OverlapCircle(contractTrigger.transform.position, 1f, playerLayer) != null)
        {
            gameManager.Reload();
        }
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }
}
