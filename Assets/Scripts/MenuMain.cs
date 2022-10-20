using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : MonoBehaviour
{

    [SerializeField] GameObject gameManager;

    public void NewGame()
    {
        gameManager.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
