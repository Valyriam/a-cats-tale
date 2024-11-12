using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int pageNumber;
    bool activated;
    CheckpointManager checkpointManager;
    Transform exactPosition;

    private void Start()
    {
        activated = false;
        checkpointManager = GameObject.FindObjectOfType<CheckpointManager>();
        exactPosition = transform.GetChild(0).transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            if (collision.tag == "Character")
            {
                if (checkpointManager.latestCheckpointPosition.x == 0 || checkpointManager.latestCheckpointPosition.x < transform.position.x) //prevent from activating checkpoints that you've already passed
                {
                    checkpointManager.latestCheckpointPosition = exactPosition.position;
                    checkpointManager.latestCheckpointPageNumber = pageNumber;
                    activated = true;
                }
            }
        }
    }
}
