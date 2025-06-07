using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;      // ����� �ҽ� (BGM��)
    public AudioClip normalBGM;          // ���� ����
    public AudioClip chaseBGM;           // �߰�(���) ����
    public float fadeTime = 1.0f;        // ���̵� ��ȯ �ð�

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

    // �ܺο��� ȣ�� - �߰� BGM
    public void OnChaseStarted()
    {
        SwitchMusic(chaseBGM);
    }

    // �ܺο��� ȣ�� - ���� BGM
    public void OnChaseStopped()
    {
        SwitchMusic(normalBGM);
    }

    // ���̵� ��ȯ
    public void SwitchMusic(AudioClip newClip)
    {
        if (audioSource == null || newClip == null) return;

        if (musicRoutine != null) StopCoroutine(musicRoutine);
        musicRoutine = StartCoroutine(FadeMusic(newClip));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVol = audioSource.volume;

        // 1. ���̵� �ƿ�
        float t = 0;
        while (t < fadeTime)
        {
            audioSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();

        // 2. �� �������� ��ü
        audioSource.clip = newClip;
        audioSource.Play();

        // 3. ���̵� ��
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
