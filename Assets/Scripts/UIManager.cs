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
    [SerializeField] private GameObject defeatScreen;

    [SerializeField] private Image cursor;
    [SerializeField] private Text scoreBoat;
    [SerializeField] private Text boatLeft;
    [SerializeField] private Text endScoreBoat;
    [SerializeField] private Text endScoreSeagull;

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
        defeatScreen.SetActive(false);
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
        defeatScreen.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Defeat()
    {
        Time.timeScale = 0;
        playingPanel.SetActive(false);
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        defeatScreen.SetActive(true);
        ScoreManager.Instance.Endgame();
    }
    #endregion

    #region Buttons
    public void PlayButton()
    {
        ScoreManager.Instance.Reset();
        BoatManager.Instance.Reset();
        MouetteManager.Instance.Reset();
        GameManager.Inst.StartPlaying();
        Play();
    }


    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion

    public void ScoreBoat(int count, int goal)
    {
        Debug.Log("UIScore");
        scoreBoat.text = count + " / " + goal;
    }

    public void BoatLeft(int left)
    {
        boatLeft.text = left.ToString();
    }

    public void EndScore(int boats, int seagulls)
    {
        endScoreBoat.text = "Youo saved " + boats + "boats";
        endScoreSeagull.text = "and fed " + seagulls + "seagulls!";
    }
}
