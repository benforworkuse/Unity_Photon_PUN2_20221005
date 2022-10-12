using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ben
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region ��Ƥj�U
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
        #region ��Ʃж�
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion
        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("��r�ж��W��").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("��r�ж��H��").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("�e���ж�").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("���s�}�l�C��").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("���s���}�ж�").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("��J���a�W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("�Ыةж��W��").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J���w�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();

            groupMain = GameObject.Find("�e���D�n").GetComponent<CanvasGroup>();

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
            print("<color=yellow>�w�g�s�u�ܥD��</color>");
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
            print("<color=green>�Ыةж����\</color>");
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("<color=green>�[�J�ж����\</color>");
            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "�ж��W��: " + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"�ж��H��: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
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
            textRoomPlayer.text = $"�ж��H��: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"�ж��H��: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}

