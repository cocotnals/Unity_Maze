using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;      // 오디오 소스 (BGM용)
    public AudioClip normalBGM;          // 평상시 음악
    public AudioClip chaseBGM;           // 추격(긴박) 음악
    public float fadeTime = 1.0f;        // 페이드 전환 시간

    private Coroutine musicRoutine;

    void Start()
    {
        if (audioSource != null && normalBGM != null)
        {
            audioSource.clip = normalBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // 외부에서 호출 - 추격 BGM
    public void OnChaseStarted()
    {
        SwitchMusic(chaseBGM);
    }

    // 외부에서 호출 - 평상시 BGM
    public void OnChaseStopped()
    {
        SwitchMusic(normalBGM);
    }

    // 페이드 전환
    public void SwitchMusic(AudioClip newClip)
    {
        if (audioSource == null || newClip == null) return;

        if (musicRoutine != null) StopCoroutine(musicRoutine);
        musicRoutine = StartCoroutine(FadeMusic(newClip));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVol = audioSource.volume;

        // 1. 페이드 아웃
        float t = 0;
        while (t < fadeTime)
        {
            audioSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();

        // 2. 새 음악으로 교체
        audioSource.clip = newClip;
        audioSource.Play();

        // 3. 페이드 인
        t = 0;
        while (t < fadeTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVol, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = startVol;
    }
}
