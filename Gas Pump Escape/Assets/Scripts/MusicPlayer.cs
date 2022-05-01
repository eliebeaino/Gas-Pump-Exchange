using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip startGame;
    [SerializeField] AudioClip gameOver;

    // singleton pattern for music player
    private void Awake()
    {
        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayers > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }

    public void MusicPlay()
    {
        if (SceneManager.GetActiveScene().name == "GameOver" && audioSource.clip != gameOver)
        {
            audioSource.clip = gameOver;
            audioSource.Play();
        }
        else if (audioSource.clip != startGame)
        {
            audioSource.clip = startGame;
            audioSource.Play();
        }
    }

    public void SetVolume (float volume)
    {
        audioSource.volume = volume;
    }
}
