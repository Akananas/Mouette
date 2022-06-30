using System.Collections.Generic;
using UnityEngine;

public class MouetteScript : MonoBehaviour, IHitComp
{
    enum MouetteState{
        LookingForBoat, Chasing, Hitted,
    };

    MouetteState m_MouetteState;
    Queue<Vector2> m_Path = new Queue<Vector2>();
    [SerializeField]int m_PathStep;
    Vector2 m_NextPoint;
    Vector3 m_Dir;
    Transform m_MouetteTransform;
    Transform m_Target;
    float m_Speed;
    
    void Start(){
        m_NextPoint = new Vector2(10, 0);
        m_MouetteTransform = transform;
        m_Speed = 0;
        CalculateDirection();
        m_MouetteState = MouetteState.LookingForBoat;
    }
    
    void Update(){
        switch (m_MouetteState){
            case MouetteState.LookingForBoat:
                Movement();
                LookingForTarget();
                break;
            case MouetteState.Chasing:
                if(!Movement()){
                    m_NextPoint = m_Target.position;
                    CalculateDirection();
                }
                break;
            case MouetteState.Hitted:
                Movement();
                break;
        }
    }
        
    bool Movement(){
        if (Vector2.Distance(m_NextPoint, (Vector2)m_MouetteTransform.position) <= 0.1f){
            if(m_Path.Count == 0){
                return false;
            }
            
            m_NextPoint = m_Path.Dequeue();
            Debug.Log(m_NextPoint);
            CalculateDirection();
        }
        
        m_MouetteTransform.position += m_Dir * m_Speed * Time.deltaTime;
        return true;
    }
    
    void LookingForTarget(){
        if(m_Target){
            CalculatePath();
        }
    }
    
    void CalculatePath(){
        float _currentSpeed = m_Target.GetComponentInParent<Boat>().GetCurrentSpeed();
        Vector2 _targetPos = m_Target.position;
        m_Path.Clear();
        
        for(int i = 1; i <= m_PathStep; ++i){
            float _yPos = Mathf.Lerp(m_MouetteTransform.position.y, _targetPos.y, (float)i/(float)m_PathStep);
            float _xPos = _currentSpeed * Time.deltaTime * i + _targetPos.x;
            
            m_Path.Enqueue(new Vector2(_xPos, _yPos));
        }
        
        m_NextPoint = m_Path.Dequeue();
        CalculateDirection();
        
        m_Speed = Mathf.Abs(_currentSpeed);
        m_MouetteState = MouetteState.Chasing;
    }
    
    
    void CalculateDirection(){
        m_Dir = ((Vector3)m_NextPoint - m_MouetteTransform.position).normalized;
    }
    
    void OnTriggerEnter2D(Collider2D col){
        if(m_MouetteState == MouetteState.LookingForBoat && col.CompareTag("MouetteTarget")){
            m_Target = col.gameObject.transform;
        }
    }
    
    public void Hit(){
        Debug.Log("Mouette hit");
        m_MouetteState = MouetteState.Hitted;
        m_Path.Clear();
        m_NextPoint = new Vector2(m_MouetteTransform.position.x, -1);
    }
}
