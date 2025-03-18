using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Queue<string> tracks;
    private string path;
    public float delayBetweenTracks = 1f;
    private Coroutine playTracksCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tracks = new Queue<string>();
        path = Path.Combine(Application.streamingAssetsPath, "Music", "Settings");

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] wavFiles = directoryInfo.GetFiles("*.wav");

        foreach (FileInfo wavFile in wavFiles)
        {
            tracks.Enqueue("file:///" + wavFile.FullName);
        }

        playTracksCoroutine = StartCoroutine(PlayTracks());
    }

    private void OnDisable()
    {
        if (playTracksCoroutine != null)
        {
            StopCoroutine(playTracksCoroutine);
            playTracksCoroutine = null;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        if (playTracksCoroutine != null)
        {
            StopCoroutine(playTracksCoroutine);
            playTracksCoroutine = null;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    IEnumerator PlayTracks()
    {
        while (true)
        {
            if (tracks.Count == 0)
            {
                yield break;
            }

            string track = tracks.Dequeue();
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(track, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    audioSource.clip = clip;
                    audioSource.Play();

                    yield return new WaitUntil(() => !audioSource.isPlaying);

                    yield return new WaitForSecondsRealtime(delayBetweenTracks);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }

            tracks.Enqueue(track);
        }
    }
}
