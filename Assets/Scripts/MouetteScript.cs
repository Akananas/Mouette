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
    [SerializeField]AnimationCurve m_ChasingPath;
    Vector2 m_NextPoint;
    Vector3 m_Dir;
    Transform m_MouetteTransform;
    Transform m_Target;
    float m_Speed;
    
    void Start(){
        if(m_NextPoint == null){
            m_NextPoint = new Vector2(10, 0);
        }
        m_MouetteTransform = transform;
        m_Speed = 1;
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
            CalculateDirection();
        }
        
        m_MouetteTransform.position = Vector2.MoveTowards(m_MouetteTransform.position, m_NextPoint, m_Speed * Time.deltaTime);

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
        float _endingX = _targetPos.x;
        for(int i = 0; i <= m_PathStep; ++i){
            _endingX += _currentSpeed / 1.5f;
        }
        
        for(int i = 1; i <= m_PathStep; ++i){
            float _yPos = Mathf.Lerp(m_MouetteTransform.position.y, _targetPos.y, (float)i/(float)m_PathStep);
            float _xPos = m_ChasingPath.Evaluate((float)i/(float)m_PathStep) * _currentSpeed + _endingX;
            
            m_Path.Enqueue(new Vector2(_xPos, _yPos));
        }
        
        m_NextPoint = m_Path.Dequeue();
        m_Speed = 2;
        CalculateDirection();
        Debug.Log(m_NextPoint);
        
        m_MouetteState = MouetteState.Chasing;
    }
    
    
    void CalculateDirection(){
        m_Dir = ((Vector3)m_NextPoint - m_MouetteTransform.position).normalized;
        m_MouetteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(m_Dir.x, m_Dir.y));
    }

    public void SetNextPoint(Vector2 val){
        m_NextPoint = val;
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
        m_NextPoint = new Vector2(m_MouetteTransform.position.x, -3);
        CalculateDirection();
    }
}
