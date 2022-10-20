using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }

    public void RestartGame()
    {
        gameManager.RestartGame();
    }

    public void QuitGame()
    {
        gameManager.EndGame(null);
    }
}
