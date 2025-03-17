using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float smoothing = 2f;

    [Header("Instancing")]
    [SerializeField] public static CameraSystem cameraSystem;

    [Header("Positions")]
    [SerializeField] private Vector3 centrePos;
    [SerializeField] private Transform leftPlayer;
    [SerializeField] private Transform rightPlayer;
    [SerializeField] private float _zAxis = -2.98f;
    /*[SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;
    [SerializeField] private Vector3 leftPosMob;
    [SerializeField] private Vector3 rightPosMob;*/

    private void Awake()
    {
        cameraSystem = this;
    }

    private void OnEnable()
    {
        TempoManager.OnStep += ZoomEffect;
    }

    private void OnDestroy()
    {
        TempoManager.OnStep -= ZoomEffect;
    }


    void LateUpdate()
    {
        SwitchFocus();

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoom, 15f * Time.deltaTime);
    }

    private void SwitchFocus() // originally had seperate positions based on platform but this has been changed - make code efficient later down the line
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            switch (GameManager.Instance.focus)
            {
                case Focus.LeftPlayer:
                    transform.position = Vector3.LerpUnclamped(transform.position, new(leftPlayer.position.x, leftPlayer.position.y, _zAxis), smoothing * Time.deltaTime);
                    break;
                case Focus.RightPlayer:
                    transform.position = Vector3.LerpUnclamped(transform.position, new(rightPlayer.position.x, rightPlayer.position.y, _zAxis), smoothing * Time.deltaTime);
                    break;
                case Focus.Centre:
                    transform.position = Vector3.LerpUnclamped(transform.position, new(leftPlayer.position.x, leftPlayer.position.y, _zAxis), smoothing * Time.deltaTime);
                    break;
            }
        }
        else
        {
            switch (GameManager.Instance.focus)
            {
                case Focus.LeftPlayer:
                    transform.position = Vector3.LerpUnclamped(transform.position, new(leftPlayer.position.x, leftPlayer.position.y, _zAxis), smoothing * Time.deltaTime);
                    break;
                case Focus.RightPlayer:
                    transform.position = Vector3.LerpUnclamped(transform.position, new(rightPlayer.position.x, rightPlayer.position.y, _zAxis), smoothing * Time.deltaTime);
                    break;
                case Focus.Centre:
                    transform.position = Vector3.LerpUnclamped(transform.position, centrePos, smoothing * Time.deltaTime);
                    break;
            }
        }
    }

    float zoom = 60f;
    private void ZoomEffect(int step)
    {
        if (step == 1) { zoom = 50f; StartCoroutine(UnZoom()); }
    }

    private IEnumerator UnZoom()
    {
        yield return new WaitForSeconds(0.05f);
        zoom = 60f;
        StopCoroutine(UnZoom());
    }
}
