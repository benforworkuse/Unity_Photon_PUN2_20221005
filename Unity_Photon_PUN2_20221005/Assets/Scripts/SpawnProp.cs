using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

namespace Ben
{
    public class SpawnProp : MonoBehaviour
    {
        [SerializeField, Header("道具")] private List <GameObject> prefabs;
        [SerializeField, Header("生成點")] private List <Transform> spawnPoints;
        [SerializeField, Header("生成頻率"), Range(0, 5)] private float intervalSpawn = 2.5f;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
            }
        }

        private void Spawn()
        {
            int ran = Random.Range(0, spawnPoints.Count);
            int ran2 = Random.Range(0, prefabs.Count);
            PhotonNetwork.Instantiate(prefabs[ran2].name, spawnPoints[ran].position, Quaternion.identity);
        }
    }
}

