using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using echo17.EndlessBook;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] BookItem menuBook;
    [SerializeField] BookItem playableBook;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera menuBookCam;
    [SerializeField] CinemachineVirtualCamera playingBookCam;

    [Header("Player")]
    [SerializeField] PlayerInput playerInputRef;
    [SerializeField] string previousPlayerActionMap;
    [SerializeField] int previousPageNumber;

    [Header("Screen Containers")]
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject inGameUIScreen;
    [SerializeField] GameObject pageSelectScreen;
    [SerializeField] GameObject mainMenuPlayButton;
    [SerializeField] GameObject pageSelect10Button;

    [Header("Screen Containers")]
    [SerializeField] DoublePageSegment mainMenuDPSegment;
    [SerializeField] DoublePageSegment settingsDPSegment;
    [SerializeField] DoublePageSegment pauseDPSegment;

    [Header("Pausing")]
    [SerializeField] AudioClip pauseMusicToFadeInto;
    [SerializeField] AudioSourceController audioSourceController;
    public bool transitionAlreadyOccurring;

    [Header("Functionality")]
    public GameObject preparedScreenContainer;
    public float activationWaitTime;
    public bool gameInProgress;    

    EventSystem eventSystem;
    

    private void Awake()
    {
        preparedScreenContainer = mainMenuScreen;
        previousPlayerActionMap = "Default Cat";
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    public void PlayGame()
    {
        MoveToPlayableBook();

        if (previousPageNumber == 0)
            playableBook.DelayedBookOpenF(2);

        else
        {
            playableBook.DelayedBookOpenMid(2);
            playableBook.DelayedPageTurn(2.5f, previousPageNumber, 0.1f);
        }

        gameInProgress = true;
    }

    public void MoveToPlayableBook()
    {
        menuBookCam.Priority = 10;
        playingBookCam.Priority = 31;
        SwapToPlayingActionMap();
    }

    public void MoveToMenuBook(int targetPageNumber)
    {
        menuBookCam.Priority = 31;
        playingBookCam.Priority = 10;

        SwapToUIActionMap();

        menuBook.GetComponent<EndlessBook>().SetPageNumber(targetPageNumber);
        menuBook.DelayedBookOpenMid(2);

        eventSystem.SetSelectedGameObject(mainMenuPlayButton);
    }

    public void CollectPageNumber()
    {
        if (playableBook.thisBook.CurrentState == echo17.EndlessBook.EndlessBook.StateEnum.OpenFront)
            previousPageNumber = 0;

        else
            previousPageNumber = playableBook.thisBook.CurrentPageNumber;
    }

    public void SetPreviousPageNumber(int pageNumber)
    {
        previousPageNumber = pageNumber;
    }

    #region Action Maps
    void SwapToUIActionMap() => playerInputRef.SwitchCurrentActionMap("UI");
    void SwapToPlayingActionMap() => playerInputRef.SwitchCurrentActionMap(previousPlayerActionMap);

    public void StorePreviousPlayerActionMapString()
    {
        previousPlayerActionMap = playerInputRef.currentActionMap.name;
    }
    #endregion

    #region Screen Management
    public void EnablePreparedScreen() => preparedScreenContainer.SetActive(true);
    public void EnablePreparedScreenAfterWaitCoroutine(int requiredWaitTime) => StartCoroutine(EnablePreparedScreenAfterWaitTime(requiredWaitTime));

    public IEnumerator EnablePreparedScreenAfterWaitTime(float waitTime)
    {
        transitionAlreadyOccurring = true;

        yield return new WaitForSeconds(waitTime);

        preparedScreenContainer.SetActive(true);
        transitionAlreadyOccurring = false;
    }

    public void ActivateMainMenuScreen()
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = mainMenuScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(activationWaitTime));
        }

        //mainMenuScreen.SetActive(true);
    }

    public void ActivateMainMenuScreen(int waitTime)
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = mainMenuScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(waitTime));
        }

        //mainMenuScreen.SetActive(true);
    }

    public void ActivatePauseScreen()
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = pauseScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(activationWaitTime));
            pauseDPSegment.SetSegmentStates(true);
        }

        //pauseScreen.SetActive(true);
    }

    public void ActivatePauseScreen(int waitTime)
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = pauseScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(waitTime));
        }

        //pauseScreen.SetActive(true);
    }

    public void ActivateInGameUI()
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = inGameUIScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(activationWaitTime));
        }

        //inGameUIScreen.SetActive(true);
    }

    public void ActivateSettingsScreen(int waitTime)
    {
        if (!transitionAlreadyOccurring)
        {
            DisableAllScreens();
            preparedScreenContainer = settingsScreen;
            StartCoroutine(EnablePreparedScreenAfterWaitTime(waitTime));
        }

        //settingsScreen.SetActive(true);
    }

    void DisableAllScreens()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(false);
        inGameUIScreen.SetActive(false);
        pageSelectScreen.SetActive(false);
    }

    public void ExitSettingsScreen()
    {
        //if we've started a game, go back to pause screen, otherwise go to main menu screen
        if (gameInProgress)
        {
            ActivatePauseScreen(1);
            menuBook.PageRight();
            pauseDPSegment.SetSegmentStates(true);
        }

        else
        {
            ActivateMainMenuScreen(1);
            menuBook.PageLeft();
            mainMenuDPSegment.SetSegmentStates(true);
        }
    }

    public void ActivatePageSelectScreen()
    {
        mainMenuScreen.SetActive(false);
        pageSelectScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(pageSelect10Button);
    }

    public void ExitPageSelectScreen()
    {
        pageSelectScreen.SetActive(false);
        mainMenuScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(mainMenuPlayButton);
    }

    #endregion

    public void PauseGame()
    {
        if (!transitionAlreadyOccurring)
        {
            ActivatePauseScreen();
            MoveToMenuBook(5);
            playableBook.CloseBookF();
            CollectPageNumber();
            audioSourceController.PresetFadeIntoNewClip(pauseMusicToFadeInto);
        }
    }
}
