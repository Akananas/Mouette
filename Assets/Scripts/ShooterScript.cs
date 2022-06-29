using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    bool m_CanShoot;
    [SerializeField]float radius;
    [SerializeField]LayerMask hittableObjects;
    
    void Start(){
        m_CanShoot = true;
    }
    
    void Update(){
        if(Input.GetMouseButton(0) && m_CanShoot){
            m_CanShoot = false;
            Vector2 _mousePosition = Input.mousePosition;
            _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
            Debug.Log("Shoot");
            BasicShoot(_mousePosition);
        }
        
        m_CanShoot = true;
    }
    
    void BasicShoot(Vector2 position){
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, radius, hittableObjects);
        foreach (var obj in _hitObjects){
            Debug.Log("Hit object");
        }
    }
}
