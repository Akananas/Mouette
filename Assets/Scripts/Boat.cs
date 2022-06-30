using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IHitComp
{
    public int direction = 1;
    [SerializeField] private float speed;
    [SerializeField] int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);
        if (CheckFinish())
            direction *= -1;
    }

    bool CheckFinish()
    {
        float X = Camera.main.WorldToScreenPoint(transform.position).x;
        if (direction > 0)
        {
            return Camera.main.WorldToScreenPoint(transform.position - Vector3.right * GetComponentInParent<SpriteRenderer>().size.x).x > Screen.width;
        }
        else
        {
            return Camera.main.WorldToScreenPoint(transform.position + Vector3.right * GetComponentInParent<SpriteRenderer>().size.x).x < 0;
        }
    }
    
    public float GetCurrentSpeed(){
        return speed * direction;
    }

    public void Hit()
    {
        Debug.Log("Boat Hit");
        health -= 10;
        if (health < 0)
        {
            BoatManager.Instance.DestroyBoat(this);
        }
    }
}
