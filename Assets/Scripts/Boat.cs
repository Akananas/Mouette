using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IHitComp
{
    public int direction = 1;
    [SerializeField] private float speed;
    [SerializeField] private float health = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);
        if (CheckFinish())
        {
            ScoreManager.Instance.ScoreBoat();
            Destroy(gameObject);
        }
    }

    // Return true if boat has arrived at destination
    bool CheckFinish()
    {
        float X = Camera.main.WorldToScreenPoint(transform.position).x;
        if (direction > 0)
            return Camera.main.WorldToScreenPoint(transform.position - Vector3.right * GetComponent<SpriteRenderer>().size.x).x > Screen.width;
        else
            return Camera.main.WorldToScreenPoint(transform.position + Vector3.right * GetComponent<SpriteRenderer>().size.x).x < 0;
    }
    
    public float GetCurrentSpeed(){
        return speed * direction;
    }

    public void Hit()
    {
        health -= 10;
        if (health < 0)
        {
            BoatManager.Instance.DestroyBoat(this);
        }
    }
}
