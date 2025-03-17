using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public enum TypeOfScrollEvent 
{
    FocusCentre,
    FocusPlayerRight,
    FocusPlayerLeft,
    ZoomOnGirlFriend,
    ChangeSongSpeed,
    SectionCompleteAnimation,
    Cutscene,
    Animation,
    InstantRestart
}

public class ScrollEvents : MonoBehaviour
{
    public TypeOfScrollEvent typeOfScrollEvent;

    [SerializeField] private float scrollSpeedModificationAmount;

    [Header("Animation")]
    [Tooltip("If this event doesn't make use of animations, don't drag one into this variable.")][SerializeField] private Animation eventAnim;

    [Header("Cutscene")]
    [SerializeField] private string _cutscenePath;

    private void FocusCentre()
    {
        GameManager.Instance.focus = Focus.Centre;
    }

    private void FocusLeftPlayer()
    {
        GameManager.Instance.focus = Focus.LeftPlayer;
    }

    private void FocusRightPlayer()
    {
        GameManager.Instance.focus = Focus.RightPlayer;
    }

    private void ChangeSongSpeed()
    {
        GameManager.Instance.scrollManager.scrollSpeedMultiplier = scrollSpeedModificationAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScrollEventTrigger"))
        {
            switch (typeOfScrollEvent)
            {
                case TypeOfScrollEvent.FocusCentre: FocusCentre(); break;
                case TypeOfScrollEvent.FocusPlayerRight: FocusRightPlayer(); break;
                case TypeOfScrollEvent.FocusPlayerLeft: FocusLeftPlayer(); break;
                case TypeOfScrollEvent.ChangeSongSpeed: ChangeSongSpeed(); break;
                case TypeOfScrollEvent.Animation: eventAnim.Play(); break;
                case TypeOfScrollEvent.InstantRestart: SceneManager.LoadScene(SceneManager.GetActiveScene().name); break;
                case TypeOfScrollEvent.Cutscene: Instance.DreamwaveVideoStreamer.InitLoad(_cutscenePath); break;
            }
        }
    }
}
