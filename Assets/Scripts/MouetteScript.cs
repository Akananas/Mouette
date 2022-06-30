using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteScript : MonoBehaviour, IHitComp
{
    enum MouetteState{
        LookingForBoat, Chasing, Hitted,
    };

    MouetteState mouetteState;

    void Start(){
        GameManager.Inst?.AddTimer(TimerTest, 1.0f, true, 0);
    }
    
    void Update(){
        switch (mouetteState){
            case MouetteState.LookingForBoat:
                break;
            case MouetteState.Chasing:
                break;
            case MouetteState.Hitted:
                break;
        }
    }
    
    void TimerTest(){
        Debug.Log("Timer");
    }
    public void Hit(){
    }
}
