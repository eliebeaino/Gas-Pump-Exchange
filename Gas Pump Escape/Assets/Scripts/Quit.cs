using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Quit : MonoBehaviour
{
    [SerializeField] float delayForNextLevel = 1.5f;

    // trigger quit on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
        Destroy(collision.gameObject);
    }

    // go to start menu       -------------- MUST CHANGE TO QUIT.APPLICATION -------------
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(delayForNextLevel);
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            SceneManager.LoadScene(0);
        }
    }
}
