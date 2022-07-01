using System;
using UnityEngine;

public class Timer
{
    Action m_EndTimer;
    float m_Time;
    float m_CurrentTime;
    bool m_Looping;
    bool m_ConstantLooping;
    bool m_Paused;
    int m_LoopingTime;
    
    //0 or negative loop time amount for infinite timer
    public Timer(Action endTimer, float time, bool looping = false, int loopingTime = 1, bool startPaused = false){
        m_EndTimer = endTimer;
        m_Time = time;
        m_CurrentTime = 0.0f;
        m_Looping = looping;
        m_LoopingTime = loopingTime;
        m_ConstantLooping = m_Looping && m_LoopingTime <= 0;
        m_Paused = startPaused;
    }
    
    public bool Update(float deltaTime){    
        if(m_Paused){
            return false;
        }
        m_CurrentTime += deltaTime;
        if(m_CurrentTime >= m_Time){
            m_EndTimer?.Invoke();
            
            if (m_ConstantLooping){
                m_CurrentTime = m_CurrentTime - m_Time;
                return false;
            }
            
            if (m_Looping){
                m_LoopingTime--;
                m_CurrentTime = m_LoopingTime <= 0 ? m_Time : m_CurrentTime - m_Time;
                return m_LoopingTime <= 0;
            }
            
            m_CurrentTime = m_Time;
            return true;
        }
        return false;
    }

    public bool IsActive(){
        return m_CurrentTime < m_Time;
    }

    public void AddTime(float addedTime){
        m_Time += addedTime; 
    }

    public float GetRemainingTime(){
        return m_Time - m_CurrentTime;
    }
    
    public void ForceStop(bool doAnotherTick){
        m_CurrentTime = m_Time;
        m_Looping = false;
        if(!doAnotherTick){
            m_EndTimer = null;
        }
    }
    
    public void ResetTime(){
        m_CurrentTime = 0;
    }

    public void SetPauseValue(bool pause){
        m_Paused = pause;
    }
}
