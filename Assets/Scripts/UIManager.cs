using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Image cursor;
    [SerializeField] private Text scoreBoat;

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
            Instance = this;
        // Jeu final - Appel du GameManager
        MainMenu();
        // Jeu final - Appel du GameManager
    }
        
    private void Update()
    {
        cursor.rectTransform.position = Input.mousePosition;
    }

    #region UIStates
    public void Play()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        playingPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        // Jeu final - Appel du GameManager
        Time.timeScale = 0;
        playingPanel.SetActive(false);
        pausePanel.SetActive(true);
        // Jeu final - Appel du GameManager
    }

    public void MainMenu()
    {
        Time.timeScale = 0;
        playingPanel.SetActive(false);
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    #endregion

    #region Buttons
    public void PlayButton()
    {
        ScoreManager.Instance.Reset();
        Play();
    }


    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion

    public void ScoreBoat(int count, int goal)
    {
        scoreBoat.text = count + " / " + goal;
    }
}
