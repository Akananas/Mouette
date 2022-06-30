using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text scoreBoat;

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
            Instance = this;
    }

    public void ScoreBoat(int count, int goal)
    {
        scoreBoat.text = count + " / " + goal;
    }
}
