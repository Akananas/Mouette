using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private int boatCount = 0;
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
        if (boatCount >= goalBoat)
        {
            boatCount = 0;
            goalBoat *= 2;
            UIManager.Instance.ScoreBoat(0, goalBoat);
            return true;
        }
        UIManager.Instance.ScoreBoat(boatCount, goalBoat);
        return false;
    }
}
