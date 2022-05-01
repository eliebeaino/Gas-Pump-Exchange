using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float fadeDelay = 1.5f;

    // paramaters for scene counting
    int totalSceneCount;
    int currentScene;
    int lastLvl;

    private void Start()
    {
        // triggeres the correct tracks to be played on scene load
        FindObjectOfType<MusicPlayer>().GetComponent<MusicPlayer>().MusicPlay();

        // sets up scene count for non-lvl scenes
        totalSceneCount = SceneManager.sceneCountInBuildSettings;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        lastLvl = totalSceneCount - 2; // game over -- success
    }


    // trigger next level and remove player from scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FadeOutNextLevel());
        collision.GetComponent<PlayerHealth>().ChangeAliveStatus();
    }
    // fade out delay controller when ending level
    IEnumerator FadeOutNextLevel()
    {
        yield return new WaitForSeconds(fadeDelay);
        GetComponentInChildren<Animator>().SetTrigger("FadeOut");
    }
    // Triggered by animator when entered level exit collider after time delay
    public void LoadNextLevel()
    {
        // checks if theres no more levels, goes back to start
        if (currentScene >= lastLvl )
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }




    // load the sequence from other script
    public void FadeOutGameOver()
    {
        StartCoroutine(CoFadeOutGameOver());
    }
    // fade out delay controller when game over
    private IEnumerator CoFadeOutGameOver()
    {
        yield return new WaitForSeconds(fadeDelay);
        GetComponentInChildren<Animator>().SetTrigger("GameOverFadeOut");
    }
    // triggered from animator when player is dead   -- or some other funciotnality that may cause the player to losoe the game --
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}
