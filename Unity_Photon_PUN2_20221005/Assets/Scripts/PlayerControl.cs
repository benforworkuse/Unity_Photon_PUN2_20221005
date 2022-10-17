using Photon.Pun;
using UnityEngine;
using TMPro;
using Cinemachine;

namespace Ben
{
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���ʳt��"), Range(0, 10)] private float speed = 3.5f;
        [Header("�ˬd�a�O���")] [SerializeField] private Vector3 groundOffSet;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("���D����"), Range(0, 1000)] private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "�}������";
        private bool isGround;
        private Transform childCanvas;

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            childCanvas = transform.GetChild(0);
            
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);
        }

        private void Start()
        {
            GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        private void Update()
        {
            Move();
            CheckGround();
            Jump();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("����"))
            {
                PhotonNetwork.Destroy(collision.gameObject);
            }
        }

        [PunRPC] private void RPCUpdateName()
        {
            transform.Find("Canvas/�W�٤���").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffSet, groundSize);
        }

        private void Move()
        {
            float h = Input.GetAxis("Horizontal"); 
            //Unity���ت���k�A�|�H�ɦ^�ǭȥH�Q��}�⪺�ʧ@����A��LA�BD�B��V��U�ۦ^��-1�M1���ȡA�LA�BD ����L��J�h�Ǧ^0
            rig.velocity = new Vector2(speed * h, rig.velocity.y);

            ani.SetBool(parWalk, h != 0);
            if (Mathf.Abs(h) < 0.1f) return;
            transform.eulerAngles = new Vector3(0, h>0?180:0, 0);
            childCanvas.localEulerAngles= new Vector3(0, h > 0 ? 180 : 0, 0);
        }

        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffSet, groundSize, 0);
            isGround = hit; //print(hit.name);
        }

        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(new Vector2(0, jump));
            }
        }
    }
}

