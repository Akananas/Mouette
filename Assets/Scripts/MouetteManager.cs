using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteManager : MonoBehaviour
{
    List<MouetteScript> m_Alive;
    [SerializeField]GameObject prefab;

    void Start(){
        GameManager.Inst.AddTimer(SpawnMouette, 2.5f);
    }

    void SpawnMouette(){
        Vector3 spawnPoint = GetRandomSpawnPoint();
        GameObject go = Instantiate(prefab, spawnPoint, Quaternion.identity); 
        if(spawnPoint.x > 0){
            go.GetComponent<MouetteScript>().SetNextPoint(new Vector2(-10, spawnPoint.y));
            Debug.Log(spawnPoint.x);
        }
        else{
            go.GetComponent<MouetteScript>().SetNextPoint(new Vector2(10, spawnPoint.y));
        }

        GameManager.Inst.AddTimer(SpawnMouette, 2.5f);
    }

    Vector3 GetRandomSpawnPoint(){
        int right = Random.Range(0,2);
        float x = right == 1 ? 10 : -10;
        float y = Random.Range(1.0f, 4.5f);
        return new Vector3(x, y, 0);
    }
}
