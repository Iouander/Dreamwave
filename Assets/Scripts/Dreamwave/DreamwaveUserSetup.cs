using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamwaveUserSetup : MonoBehaviour
{
    public static DreamwaveUserSetup instance;

    [SerializeField] private NoteHitbox[] _playerNoteHitbox;

    private void Awake() => instance = this;
    
    private void Start()
    {
        LoadUserSettings();
    }

    public void LoadUserSettings()
    {
        SetPlayerFps();
        SetPlayerGraphicSettings();
        CheckPlayerNoteRenderPreferences();
        ShouldOpponentNotesRender();
        ShouldAllowGhostTapping();
        ShouldAllowFreeAnimation();
        EnableIncomingNoteWarning();
        ShouldAllowAutoPause();
        ShouldAllowNoteSplashes();
        UpdateUserKeybinds();
    }

    private void SetPlayerFps()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("fps");
        Time.fixedDeltaTime = (1.0f / PlayerPrefs.GetFloat("ffps"));
    }

    private void CheckPlayerNoteRenderPreferences()
    {
        if (PlayerPrefs.GetString("chartPos") == "downScroll")
        {
            GameManager.Instance.noteUiSidePlayer.transform.rotation = Quaternion.Euler(180f, 0f, 0);
            GameManager.Instance.noteUiSideOpponent.transform.rotation = Quaternion.Euler(180f, 0f, 0);
        }
        else if(PlayerPrefs.GetString("chartPos") == "upScroll")
        {
            GameManager.Instance.noteUiSidePlayer.transform.rotation = Quaternion.Euler(0f, 0, 0);
            GameManager.Instance.noteUiSideOpponent.transform.rotation = Quaternion.Euler(0f, 0, 0);
        }
    }

    private void SetPlayerGraphicSettings()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
    }

    private void ShouldOpponentNotesRender()
    {
        var _pref = PlayerPrefs.GetInt("opponentEnabled");

        if (_pref == 1) GameManager.Instance.noteUiSideOpponent.SetActive(true);
        else GameManager.Instance.noteUiSideOpponent.SetActive(false);
    }

    private void ShouldAllowGhostTapping()
    {
        var _pref = PlayerPrefs.GetInt("ghostTapping");

        if (_pref == 1) GameManager.Instance.canGhostTap = true;
        else GameManager.Instance.canGhostTap = false;
    }

    private void ShouldAllowFreeAnimation()
    {
        var _pref = PlayerPrefs.GetInt("freeAnimate");

        if (_pref == 1) GameManager.Instance.canFreeAnimate = true;
        else GameManager.Instance.canFreeAnimate = true;
    }

    private void EnableIncomingNoteWarning()
    {
        var _pref = PlayerPrefs.GetInt("incomingNoteWarning");

        if (_pref == 1) GameManager.Instance.shouldDisplayIncomingNoteWarning = true;
        else GameManager.Instance.shouldDisplayIncomingNoteWarning = false;
    }

    private void ShouldAllowAutoPause()
    {
        var _pref = PlayerPrefs.GetInt("autoPause");

        if (_pref == 1) GameManager.Instance.shouldAutoPause = true;
        else GameManager.Instance.shouldAutoPause = false;
    }

    private void ShouldAllowNoteSplashes()
    {
        var _pref = PlayerPrefs.GetInt("noteSplashes");

        if (_pref == 1) GameManager.Instance.shouldDrawNoteSplashes = true;
        else GameManager.Instance.shouldDrawNoteSplashes = false;
    }

    private void UpdateUserKeybinds()
    {
        _playerNoteHitbox[0].keyForSide = GameManager.Instance.left;
        _playerNoteHitbox[1].keyForSide = GameManager.Instance.down;
        _playerNoteHitbox[2].keyForSide = GameManager.Instance.up;
        _playerNoteHitbox[3].keyForSide = GameManager.Instance.right;
    }

    void Update()
    {
        
    }
}
