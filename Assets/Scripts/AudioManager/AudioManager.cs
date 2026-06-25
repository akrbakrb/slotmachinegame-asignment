using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip spinningSound;
    public AudioClip coinSound;
    public AudioClip jackpotSound;

    private AudioSource spinningSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClick);
    }

    public void PlayCoinSound()
    {
        sfxSource.PlayOneShot(coinSound);
    }

    public void PlayJackpotSound()
    {
        sfxSource.PlayOneShot(jackpotSound);
    }

    public void StartSpinningSound()
    {
        if (spinningSound == null)
            return;

        spinningSource = gameObject.AddComponent<AudioSource>();
        spinningSource.clip = spinningSound;
        spinningSource.loop = true;
        spinningSource.Play();
    }

    public void StopSpinningSound()
    {
        if (spinningSource != null)
        {
            spinningSource.Stop();
            Destroy(spinningSource);
        }
    }
}