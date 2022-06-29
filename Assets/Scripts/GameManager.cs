using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    List<Timer> m_Timers = new List<Timer>();
    
    void Awake(){
        if (Inst == null){
            Inst = this;
        }
        else{
            Destroy(this.gameObject);
        }
    }
    
    void Update(){
        for(int i = m_Timers.Count - 1; i >= 0; --i){
            if(m_Timers[i].Update(Time.deltaTime)){
                m_Timers.RemoveAt(i);
            }
        }
    }
    
    public Timer AddTimer(Action endTimer, float time){
        Timer newTimer = new Timer(endTimer, time);
        m_Timers.Add(newTimer);
        return newTimer;
    }
}
