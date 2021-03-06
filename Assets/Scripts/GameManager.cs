using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    List<Timer> m_Timers = new List<Timer>();
    public bool IsPlaying {get; set;}
    public CameraShake CamShaker {get; private set;}

    public Action OnRestart;
    
    void Awake(){
        if (Inst == null){
            Inst = this;
        }
        else{
            Destroy(this.gameObject);
        }
        IsPlaying = false;
        CamShaker = Camera.main.GetComponent<CameraShake>();
    }

    public void StartPlaying(){
        CamShaker.ResetShake();
        IsPlaying = true;
        OnRestart();
    }
    
    void Update(){
        for(int i = m_Timers.Count - 1; i >= 0; --i){
            if(m_Timers[i].Update(Time.deltaTime)){
                m_Timers.RemoveAt(i);
            }
        }
    }
    
    public Timer AddTimer(Action endTimer, float time, bool looping = false, int loopingTime = 1){
        Timer newTimer = new Timer(endTimer, time, looping, loopingTime);
        m_Timers.Add(newTimer);
        return newTimer;
    }
}
