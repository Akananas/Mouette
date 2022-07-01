using System;
using System.Collections.Generic;
using UnityEngine;

public class MouetteScript : MonoBehaviour, IHitComp
{
    enum MouetteState{
        LookingForBoat, Chasing, Hitted, Attacking,
    };

    MouetteState m_MouetteState;
    Queue<Vector2> m_Path = new Queue<Vector2>();
    [SerializeField]float m_PathStep;
    [SerializeField]AnimationCurve m_ChasingPath;
    Vector2 m_NextPoint;
    Vector3 m_Dir;
    Transform m_MouetteTransform;
    Transform m_Target;
    float m_Speed;
    [SerializeField]float m_BaseSpeed, m_ChasingSpeed;
    public Action<MouetteScript> OnHit { get; set; }
    public Action<MouetteScript> OnDeath { get; set; }
    bool m_CanChase;
    [SerializeField] ParticleSystem hitFX;
    [SerializeField] ParticleSystem fallingFX;
    [SerializeField] SpriteRenderer m_MouetteSprite;
    
    void Start(){
        if(m_NextPoint == null){
            m_NextPoint = new Vector2(10, 0);
        }
        m_MouetteTransform = transform;
        m_Speed = m_BaseSpeed;
        CalculateDirection();
        m_MouetteState = MouetteState.LookingForBoat;

        GameManager.Inst.AddTimer(() => { this.m_CanChase = true; }, 2.5f);
    }
    
    void Update(){
        switch (m_MouetteState){
            case MouetteState.LookingForBoat:
                Movement();
                if(LookingForTarget()){
                    break;
                }
                if(m_CanChase && (m_MouetteTransform.position.x > 10 || m_MouetteTransform.position.x < -10)){
                    Death();
                }
                break;
            case MouetteState.Chasing:
                Chasing();
                break;
            case MouetteState.Attacking:
                if(m_Target == null){
                    m_MouetteState = MouetteState.LookingForBoat;
                    break;
                }
                m_NextPoint = m_Target.position;
                CalculateDirection();
                Movement();
                break;
            case MouetteState.Hitted:
                if(!Movement()){
                    Death();
                }
                break;
        }
    }

    void Chasing(){
        if(m_Target == null){
            ResetLooking();
            return;
        }
        if(!Movement()){
            m_MouetteState = MouetteState.Attacking;
            m_Target.GetComponentInParent<Boat>().AddAttacker(this);
        }
    }

        
    bool Movement(){
        if (Vector2.Distance(m_NextPoint, (Vector2)m_MouetteTransform.position) <= 0.01f){
            if(m_Path.Count == 0){
                return false;
            }
            
            m_NextPoint = m_Path.Dequeue();
            CalculateDirection();
        }
        
        m_MouetteTransform.position = Vector2.MoveTowards(m_MouetteTransform.position, m_NextPoint, m_Speed * Time.deltaTime);

        return true;
    }

    void Rotation(){
        float angle = Mathf.Atan2(m_Dir.y, m_Dir.x) * 180 / Mathf.PI;
        m_MouetteSprite.flipY = angle < -90 || angle > 90;
        m_MouetteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
    bool LookingForTarget(){
        if(m_CanChase && m_Target){
            CalculatePath();
            return true;
        }
        return false;
    }
    
    //Reste à améliorer l'approche des mouettes 
    void CalculatePath(){
        float _currentSpeed = m_Target.GetComponentInParent<Boat>().GetCurrentSpeed();
        Vector2 _targetPos = m_Target.position;
        int _newStep = (int)(((int)(m_MouetteTransform.position.y - _targetPos.y) + 1) * m_PathStep);
        
        m_Path.Clear();
        float _endingX = _targetPos.x;
        for(int i = 0; i < _newStep; ++i){
            _endingX += _currentSpeed;
        }
        
        for(int i = 1; i <= _newStep; ++i){
            float _yPos = Mathf.Lerp(m_MouetteTransform.position.y, _targetPos.y, (float)i/(float)_newStep);
            float _xPos = m_ChasingPath.Evaluate((float)i/(float)_newStep) *  _currentSpeed + _endingX;
            m_Path.Enqueue(new Vector2(_xPos, _yPos));
        }
        
        m_NextPoint = m_Path.Dequeue();
        m_Speed = m_ChasingSpeed;
        CalculateDirection();
        
        m_MouetteState = MouetteState.Chasing;
    }
    
    
    void CalculateDirection(){
        m_Dir = ((Vector3)m_NextPoint - m_MouetteTransform.position).normalized;
        m_MouetteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(m_Dir.x, m_Dir.y));
        Rotation();
    }

    public void SetNextPoint(Vector2 val){
        m_NextPoint = val;
    }
    
    void OnTriggerEnter2D(Collider2D col){
        if(m_MouetteState == MouetteState.LookingForBoat && col.CompareTag("MouetteTarget")){
            m_Target = col.gameObject.transform;
        }
    }
    
    void OnTriggerExit2D(Collider2D col){
        if(m_MouetteState != MouetteState.Chasing && m_MouetteState != MouetteState.Hitted && col.CompareTag("MouetteTarget") && m_Target == col.gameObject.transform){
            ResetLooking();
        }
    }
    public void Hit(float damage = 10.0f){
        if(m_MouetteState == MouetteState.Hitted){
            return;
        }

        Debug.Log("Mouette hit");
        Instantiate(hitFX, transform.position + Vector3.back, Quaternion.identity);
        GameObject go = Instantiate(fallingFX, transform).gameObject;
        go.transform.localPosition = Vector3.zero;

        ScoreManager.Instance.ScoreSeagull();
        AudioManager.Instance.PlayClip("Hit");
        m_MouetteState = MouetteState.Hitted;
        m_Path.Clear();
        m_NextPoint = new Vector2(m_MouetteTransform.position.x, -3);
        CalculateDirection();
        OnHit?.Invoke(this);
    }

    public void Plouf(MouetteScript mouette)
    {
        if(m_MouetteState != MouetteState.Hitted){
            OnHit += Plouf;
            return;
        }
        AudioManager.Instance.PlayClip("Plouf");
        GetComponent<Animator>().SetTrigger("Plouf");
        m_MouetteTransform.rotation = Quaternion.identity;
        Death(1.0f);
    }

    public void RemovePlouf(){
        OnHit -= Plouf;
    }

    public void BombHit(float damage = 10.0f){
        Hit();
    }
    
    public void RemoveFromBoat(){
        ResetLooking();
    }

    void Death(float delayBeforeDeath = 0.0f){
        OnDeath?.Invoke(this);
        Destroy(this.gameObject, delayBeforeDeath);
    }

    void ResetLooking(){
        m_Target = null;
        m_Speed = m_BaseSpeed;
        m_MouetteState = MouetteState.LookingForBoat;
        m_NextPoint = RandomPointOutsideScreen();
        CalculateDirection();
    }
    
    Vector2 RandomPointOutsideScreen(){
        int right = UnityEngine.Random.Range(0,2);
        float x = right == 1 ? 10 : -10;
        float y = UnityEngine.Random.Range(1.0f, 4.5f);
        return new Vector2(x, y);
    }
}
