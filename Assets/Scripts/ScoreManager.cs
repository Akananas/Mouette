using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private int boatCount = 0;
    [SerializeField] private int boatCurrent = 0;
    [SerializeField] private int goalBoat = 5;


    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    // A boat has arrived at destination
    public bool ScoreBoat()
    {
        boatCount++;
        boatCurrent++;
        if (boatCurrent >= goalBoat)
        {
            boatCurrent = 0;
            goalBoat *= 2;
            UIManager.Instance.ScoreBoat(0, goalBoat);
            return true;
        }
        UIManager.Instance.ScoreBoat(boatCurrent, goalBoat);
        return false;
    }

    public void Reset()
    {
        boatCurrent = 0;
        goalBoat = 5;
        UIManager.Instance.ScoreBoat(boatCurrent, goalBoat);
    }
}
