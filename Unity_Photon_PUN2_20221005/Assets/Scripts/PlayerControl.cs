using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using TMPro;

namespace Ben
{
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("移動速度"), Range(0, 10)] private float speed = 3.5f;
        [Header("檢查地板資料")] [SerializeField] private Vector3 groundOffSet;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("跳躍高度"), Range(0, 1000)] private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "開關走路";
        private bool isGround;
        private Transform childCanvas;
        private TextMeshProUGUI textProp;
        private int score;
        private int goalScore = 10;
        private CanvasGroup groupGame;
        private TextMeshProUGUI textWinner;
        private Button btnBackToLobby;

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            childCanvas = transform.GetChild(0);
            
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textProp = transform.Find("Canvas/道具得分").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("畫布遊戲介面").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("勝利者").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("返回遊戲大廳").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(()=>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲場景");
                }
            });
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
            BackToScene();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("掉落"))
            {
                Destroy(collision.gameObject);
                textProp.text = $"分數: {++score}";
                if (score >= goalScore) Win();
            }
        }

        [PunRPC] private void RPCUpdateName()
        {
            transform.Find("Canvas/名稱介面").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffSet, groundSize);
        }

        private void Move()
        {
            float h = Input.GetAxis("Horizontal"); 
            //Unity內建的方法，會隨時回傳值以利於腳色的動作控制，鍵盤A、D、方向鍵各自回傳-1和1的值，無A、D 等鍵盤輸入則傳回0
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

        private void BackToScene()
        {
            if (transform.position.y < -40)
            {
                rig.velocity = Vector3.zero;
                transform.position = new Vector3(Random.Range(-5f, 5f), 6f, 0);
            }
        }

        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "獲勝玩家: " + photonView.Owner.NickName;
            DestoryObjects();
        }

        private void DestoryObjects()
        {
            GameObject [] props = GameObject.FindGameObjectsWithTag("掉落物");
            for (int i = 0; i < props.Length; i++) Destroy(props[i]);
            Destroy(FindObjectOfType<SpawnProp>().gameObject);
        }
    }
}

