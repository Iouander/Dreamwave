using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Variables

    [Header("Instancing")]
    [SerializeField] public static PauseMenu instance;

    [Header("States")]
    [SerializeField] public bool _isPaused = false;
    [SerializeField] public bool _inSettings = false;
    [SerializeField] public bool _inModEditor = false;

    [Header("GameObjects")]
    [SerializeField] private GameObject _pauseObj;
    [SerializeField] private GameObject _settingsObj;

    [Header("Scripts")]
    [SerializeField] private Settings _settings;

    [Header("Animation")]
    [SerializeField] private Animator _pauseAnim;

    [Header("Audio")]
    [SerializeField] private AudioSource _pauseAudioSource;

    public delegate void PauseCallback(bool pausedState);
    public static event PauseCallback Pause;

    #endregion

    #region Processes

    private void Awake() => instance = this;
    private void OnDisable() => instance = null;
    private void OnDestroy() => instance = null;

    private void Start() => _pauseAudioSource.ignoreListenerPause = true;

    void Update()
    {
        if (Input.GetKeyDown(_settings.back))
        {
            if (_inSettings)
            {
                CloseSettings();
            }
            else if (_inModEditor)
            {
                CloseModEditor();
            }
            else
            {
                PauseInput();
            }
        }
    }

    #endregion

    #region Pause Input and Applying certain states

    private void PauseInput()
    {
        switch (_isPaused)
        {
            case false:
                PauseGame();
                break;
            case true:
                UnpauseGame();
                break;
        }
    }

    #endregion

    #region Pausing and Unpausing game

    private void PauseGame()
    {
        if (!GameManager.Instance.canSongStart) return;

        AudioListener.pause = true;
        _pauseAnim.CrossFade("Paused", 0.1f);
        _isPaused = true;
        _pauseObj.SetActive(true);
        _settingsObj.SetActive(false);
        Time.timeScale = 0f;
        Pause(true);
    }

    public void UnpauseGame()
    {
        AudioListener.pause = false;
        _pauseAnim.CrossFade("Unpaused", 0.1f);
        _isPaused = false;
        _pauseObj.SetActive(false);
        _settingsObj.SetActive(false);
        Time.timeScale = 1f;
        Pause(false);
    }

    #endregion

    #region Opening and Closing menus

    public void OpenSettings()
    {
        if (!_isPaused) return;

        _pauseAnim.CrossFade("OpenSettings", 0.1f);
        _inSettings = true;
    }

    public void CloseSettings()
    {
        _pauseAnim.CrossFade("CloseSettings", 0.1f);
        _inSettings = false;
    }

    public void OpenModEditor()
    {
        _pauseAnim.CrossFade("OpenModEditor", 0.1f);
        _inModEditor = true;
    }

    public void CloseModEditor()
    {
        _pauseAnim.CrossFade("CloseModEditor", 0.1f);
        _inModEditor = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync("Menu");
    }

    #endregion
}
