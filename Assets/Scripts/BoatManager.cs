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
        EnableSpawning(true);
    }

    public void EnableSpawning(bool val)
    {
        if (val)
        {
            StartCoroutine(Spawning());
        }
        else
            spawn = false;
    }

    // Spawn boat Function
    void Spawn()
    {
        Instantiate(boat, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90));
    }

    // Spawning Loop
    IEnumerator Spawning()
    {
        spawn = true;
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(range.x, range.y));
        }
    }

    public void DestroyBoat(Boat boat)
    {
        Destroy(boat.gameObject);
        boatLeft--;
        UIManager.Instance.BoatLeft(boatLeft);
        if (boatLeft <= 0) {
            UIManager.Instance.Defeat();
            EnableSpawning(false);
        }
    }

    
    // Reset Spawning (for begin play)
    public void Reset()
    {
        boatLeft = 3;
        foreach (Boat b in FindObjectsOfType<Boat>())
            Destroy(b.gameObject);
        UIManager.Instance.BoatLeft(boatLeft);
        EnableSpawning(true);
    }

}
