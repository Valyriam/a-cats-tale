using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PuzzleManager puzzleManager;
    CheckpointManager checkpointManager;
    private void Start()
    {
        checkpointManager = GameObject.FindObjectOfType<CheckpointManager>();
        puzzleManager = GameObject.FindObjectOfType<PuzzleManager>();
        puzzleManager.ClearPuzzles();   
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //if (Input.GetKeyDown(KeyCode.Z)) Application.Quit();
    }

    public void Reload()
    {
        checkpointManager.ResetPlayer();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
