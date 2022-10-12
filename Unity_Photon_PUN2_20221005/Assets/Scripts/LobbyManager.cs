using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ben
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region 資料大廳
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;

        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;

        private CanvasGroup groupMain;
        #endregion
        #region 資料房間
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion
        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("文字房間名稱").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("文字房間人數").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("畫布房間").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("按鈕開始遊戲").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("按鈕離開房間").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("輸入玩家名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("創建房間名稱").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入指定房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();

            groupMain = GameObject.Find("畫布主要").GetComponent<CanvasGroup>();

            inputFieldPlayerName.onEndEdit.AddListener((input) => namePlayer = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            groupMain.interactable = true;
            groupMain.blocksRaycasts = true; 
            print("<color=yellow>已經連線至主機</color>");
        }
        private void CreateRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 20;
            ro.IsVisible = true;
            PhotonNetwork.CreateRoom(nameJoinRoom, ro);
        }
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }
        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print("<color=green>創建房間成功</color>");
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("<color=green>加入房間成功</color>");
            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "房間名稱: " + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"房間人數: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();

            groupRoom.alpha = 0;
            groupRoom.interactable = false;
            groupRoom.blocksRaycasts = false;
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"房間人數: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"房間人數: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}

