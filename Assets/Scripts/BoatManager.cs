using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    [SerializeField] private Boat boat;
    public static BoatManager Instance;

    [SerializeField] private bool spawn = true;
    public Vector2 range;
    [SerializeField] private int boatLeft = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
            Instance = this;
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn boat Function
    void Spawn()
    {
        Instantiate(boat, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90));
    }

    public void DestroyBoat(Boat boat)
    {
        Destroy(boat.gameObject);
        boatLeft--;
        if (boatLeft <= 0) {
            // Game Over
        }
    }

    // Spawning Loop
    IEnumerator Spawning()
    {
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(range.x, range.y));
        }
    }
}
