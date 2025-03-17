using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class DreamwaveVideoStreamer : MonoBehaviour
{
    public VideoPlayer _videoPlayer;

    public void InitLoad(string path)
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, path);

#if UNITY_ANDROID
        StartCoroutine(LoadAndroidVideo(videoPath));
#else
        _videoPlayer.url = videoPath;
        _videoPlayer.Play();
#endif
    }

    IEnumerator LoadAndroidVideo(string path)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string tempPath = Path.Combine(Application.streamingAssetsPath, path);
                File.WriteAllBytes(tempPath, request.downloadHandler.data);
                _videoPlayer.url = tempPath;
                _videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Failed to load video: " + request.error);
            }
        }
    }
}
