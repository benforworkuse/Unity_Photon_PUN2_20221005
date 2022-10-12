using Photon.Pun;
using UnityEngine;

public class SceneController : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("ª±®a¹w¸mª«")] private GameObject prefabPlayer;
    private void Awake()
    {
        InitiailizePlayer();
    }
    private void InitiailizePlayer()
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(-5f, 5f);
        pos.y = 6f;
        PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);
    }
}
