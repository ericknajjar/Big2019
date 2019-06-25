using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _hadouken;
    [SerializeField]
    Transform _spawnPoint;

    public void SpawnHadouken()
    {
        Instantiate(_hadouken, _spawnPoint.position, Quaternion.identity);
    }
}
