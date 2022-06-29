using System;

public class Timer
{
    Action m_EndTimer;
    float m_Time;
    float m_CurrentTime;
    bool m_Looping;
    int m_LoopingTime;
    public Timer(Action endTimer, float time, bool looping = false, int loopingTime = 1){
        m_EndTimer = endTimer;
        m_Time = time;
        m_CurrentTime = 0.0f;
        m_Looping = looping;
        m_LoopingTime = loopingTime;
    }
    
    public bool Update(float deltaTime){    
        m_CurrentTime += deltaTime;
        if(m_CurrentTime >= m_Time){
            m_EndTimer?.Invoke();
            m_CurrentTime = m_Time;
            
            if (m_Looping){
                m_LoopingTime--;
                return m_LoopingTime <= 0;
            }
            
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
}
