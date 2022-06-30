using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IHitComp
{
    public int direction = 1;
    [SerializeField] private float speed;

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
            return Camera.main.WorldToScreenPoint(transform.position - Vector3.right * GetComponent<SpriteRenderer>().size.x).x > Screen.width;
        }
        else
        {
            return Camera.main.WorldToScreenPoint(transform.position + Vector3.right * GetComponent<SpriteRenderer>().size.x).x < 0;
        }
    }

    public void Hit()
    {
    }
}
