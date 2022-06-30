using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteManager : MonoBehaviour
{
    List<MouetteScript> m_Alive;
    [SerializeField]GameObject prefab;
    [SerializeField]Vector2 spawnRate;

    void Start(){
        RandomSpawnTimer();
    }

    void SpawnMouette(){
        Vector3 spawnPoint = GetRandomSpawnPoint();
        GameObject go = Instantiate(prefab, spawnPoint, Quaternion.identity); 
        if(spawnPoint.x > 0){
            go.GetComponent<MouetteScript>().SetNextPoint(new Vector2(-10, spawnPoint.y));
        }
        else{
            go.GetComponent<MouetteScript>().SetNextPoint(new Vector2(10, spawnPoint.y));
        }

        RandomSpawnTimer();
    }

    void RandomSpawnTimer(){
        float time = Random.Range(spawnRate.x, spawnRate.y);
        GameManager.Inst.AddTimer(SpawnMouette, time);
    }

    Vector3 GetRandomSpawnPoint(){
        int right = Random.Range(0,2);
        float x = right == 1 ? 10 : -10;
        float y = Random.Range(1.0f, 4.5f);
        return new Vector3(x, y, 0);
    }
}
