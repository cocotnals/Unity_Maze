using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalBGM;
    public AudioClip chaseBGM;

    public void PlayNormalMusic()
    {
        if (audioSource.clip != normalBGM)
        {
            audioSource.clip = normalBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayChaseMusic()
    {
        if (audioSource.clip != chaseBGM)
        {
            audioSource.clip = chaseBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
