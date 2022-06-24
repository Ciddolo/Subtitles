using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Subtitles : MonoBehaviour
{
    public List<AudioClip> Clips;
    public List<string> Texts;

    private AudioSource audioSource;
    private TextMeshProUGUI text;

    [SerializeField]
    private int currentIndex = 0;
    [SerializeField]
    private bool isPlaying;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Previous()
    {
        if (--currentIndex < 0)
            currentIndex = 0;

        Stop(currentIndex);
        Play();
    }

    public void Next()
    {
        if (++currentIndex >= Clips.Count)
            currentIndex = Clips.Count - 1;

        Stop(currentIndex);
        Play();
    }

    public void Stop(int index = 0)
    {
        StopAllCoroutines();

        text.text = "";

        audioSource.Stop();
        audioSource.clip = null;

        currentIndex = index;

        isPlaying = false;
    }

    public void Pause()
    {
        if (!isPlaying) return;

        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.Play();
    }

    public void Play()
    {
        if (isPlaying)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            StartCoroutine(PlaySubtitles());
    }

    private IEnumerator PlaySubtitles()
    {
        bool end = false;

        float timer = 0.0f;

        while (!end)
        {
            text.text = Texts[currentIndex];

            audioSource.clip = Clips[currentIndex];
            audioSource.Play();

            isPlaying = true;

            while (timer < audioSource.clip.length - 0.05f)
            {
                if (audioSource.isPlaying)
                    timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            if (currentIndex == Clips.Count - 1)
                end = true;
            else
            {
                currentIndex++;
                timer = 0.0f;
            }
        }

        Stop();
    }
}
