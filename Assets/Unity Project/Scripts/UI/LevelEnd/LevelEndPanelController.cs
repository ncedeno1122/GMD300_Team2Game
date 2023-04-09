using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndPanelController : MonoBehaviour
{
    private bool m_IsOpen = false;
    public bool IsOpen { get => m_IsOpen; }

    //private bool m_IsBlackScreenShowing = false;

    public float FadeTimeSeconds = 1f;

    // TODO: Should this script REALLY manhandle the GUIManager & MMFader?
    private CanvasGroup m_CanvasGroup;
    private GameObject m_ButtonPanel;
    public MMFader BlackScreenFader;
    public GUIManager PlayerGUIManager;

    private IEnumerator FadeLevelEndUICRT;
    
    public void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_ButtonPanel = transform.Find("ButtonPanel").gameObject;

        // Initialize
        m_CanvasGroup.alpha = 0f;
        m_ButtonPanel.SetActive(false);
    }

    // + + + + | Functions | + + + + 


    public void OnLevelEnd()
    {
        if (m_IsOpen)
        {
            HandleClose();
        }
        else
        {
            HandleOpen();
        }
    }

    private void HandleOpen()
    {
        MMDebug.DebugLogTime($"{this.GetType().Name} - Handling Opening the LevelEndUI!");

        // 1. Fade out PlayerGUI, show blackscreen
        PlayerGUIManager.SetHUDActive(false);
        BlackScreenFader.ActiveAlpha = 1f;
        MMFadeInEvent.Trigger(0.5f, BlackScreenFader.DefaultTween, id: BlackScreenFader.ID, true, worldPosition: Vector3.zero);

        // 2. Fade in this LevelEndUI
        
        if (FadeLevelEndUICRT != null) StopCoroutine(FadeLevelEndUICRT);
        FadeLevelEndUICRT = FadeLevelEndUI(false, FadeTimeSeconds);
        StartCoroutine(FadeLevelEndUICRT);
        m_ButtonPanel.SetActive(true);
    }
    private void HandleClose()
    {
        MMDebug.DebugLogTime($"{this.GetType().Name} - Handling Closing the LevelEndUI!");

        if (FadeLevelEndUICRT != null) StopCoroutine(FadeLevelEndUICRT);
        FadeLevelEndUICRT = FadeLevelEndUI(true, FadeTimeSeconds);
        StartCoroutine(FadeLevelEndUICRT);
        m_ButtonPanel.SetActive(false);
    }

    /// <summary>
    /// Fades this LevelEndUI according to user specs.
    /// </summary>
    /// <param name="fadeOut"></param>
    /// <param name="fadeTimeSeconds"></param>
    /// <returns></returns>
    private IEnumerator FadeLevelEndUI(bool fadeOut, float fadeTimeSeconds)
    {
        m_IsOpen = fadeOut; // Current status at time of request

        // We're fading THIS LevelEndPanel in/out
        float initValue = fadeOut ? 1f : 0f;
        float finalValue = fadeOut ? 0f : 1f;
        float deltaTimeHelper = 0f;

        // TODO: This seems over-engineered :)
        while (deltaTimeHelper <= fadeTimeSeconds)
        {
            deltaTimeHelper += Time.deltaTime;
            m_CanvasGroup.alpha = Mathf.Lerp(initValue, finalValue, deltaTimeHelper / fadeTimeSeconds);
            yield return new WaitForEndOfFrame();
        }

        m_IsOpen = !fadeOut;
    }

    // + + + + | Button Functions | + + + +

    public void OnNextLevelButtonPressed()
    {
        // TODO: Doesn't do anything for now...
    }

    public void OnReplayLevelButtonPressed()
    {
        // Loads this scene again, avoids using Corgi's LevelManager
        MMDebug.DebugLogTime($"{this.GetType().Name} - Replay Level Button Pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonPressed()
    {
        // Loads the main menu scene, avoids using Corgi's LevelManager
        SceneManager.LoadScene((int)SceneIndexes.MAIN_MENU);
    }
}
