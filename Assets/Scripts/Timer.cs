using System;
public class Timer
{
    Action m_EndTimer;
    float m_Time;
    float m_CurrentTime;
    public Timer(Action endTimer, float time){
        m_EndTimer = endTimer;
        m_Time = time;
        m_CurrentTime = 0.0f;
    }
    
    public bool Update(float deltaTime){    
        m_CurrentTime += deltaTime;
        if(m_CurrentTime >= m_Time){
            m_EndTimer?.Invoke();
            return true;
        }
        return false;
    }
}
