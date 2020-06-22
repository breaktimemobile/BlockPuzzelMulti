using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum Multi_Room
{
    Dia_1 = 1,
    Dia_5 = 5,
    Dia_10 = 10
}

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    string gameVersion = "1";

    Multi_Room multi_Room = Multi_Room.Dia_1;

    public int Enemy_id = -1;
    PhotonView pv;
     
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        Instance = this;


        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();

    }
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    public void Multi_Start(Multi_Room Room)
    {

        if (PhotonNetwork.IsConnected)
        {
            if (DataManager.Instance.Check_Crystal((int)Room))
            {
                this.multi_Room = Room;
                Connect();

            }
        }
        else
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    #region Public Methods


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {

#if UNITY_EDITOR
        PhotonNetwork.NickName = "Test_Player";

#else
        PhotonNetwork.NickName = Social.localUser.userName;

#endif

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.

            UIManager.Instance.Set_Multi_Player(false, "Search");
            UIManager.Instance.Set_Multi_Player(true, PhotonNetwork.NickName);

            Hashtable customRoomProperties = new Hashtable() { { "Score", 0 } , { "Rank", DataManager.Instance.state_Player.Rank }, { "Ready", 0 }, { "Check", 0 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

            Debug.Log("Join rooms "+(int)multi_Room);

            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
             customRoomProperties = new Hashtable() { { "Map", (int)multi_Room } };
            PhotonNetwork.JoinRandomRoom(customRoomProperties,2);


    }

    public void Rpc_Score()
    {
        Debug.Log("Rpc_Score");
        pv.RPC("Set_Score", RpcTarget.All);
    }

    public void Rpc_Start_Game()
    {
        pv.RPC("Start_Game", RpcTarget.All);
    }

    public void Rpc_Check_Restart(Player player)
    {
        UIManager.Instance.Btn_Multi_ReStart.gameObject.SetActive(false);

        pv.RPC("Check_Restart", RpcTarget.All, player);
    }

    public void Rpc_ReStart_Game()
    {
        pv.RPC("ReStart_Game", RpcTarget.All);
    }

    public void Rpc_Game_Over()
    {
        pv.RPC("Game_Over", RpcTarget.All);
    }


    [PunRPC]
    public void Set_Score()
    {
        Debug.Log("Rpc_Score  " + Enemy_id);

        int plyaer_Score = (int)PhotonNetwork.LocalPlayer.CustomProperties["Score"];
        int Enemy_Score = (int)PhotonNetwork.CurrentRoom.GetPlayer(Enemy_id).CustomProperties["Score"];
        UIManager.Instance.Set_Multi_Score(plyaer_Score, Enemy_Score);
        
    }

    [PunRPC]
    public void JoinRoom(bool restart)
    {
        UIManager.Instance.ToRoom(restart);
    }

    [PunRPC]
    public void Start_Game()
    {
        UIManager.Instance.Start_Game();
    }

    [PunRPC]
    public void Game_Over()
    {
        UIManager.Instance.Set_GameOver_UI();
    }


    [PunRPC]
    public void ReStart_Game()
    {

        GamePlay.instance.GameReset();

        Hashtable customRoomProperties = new Hashtable() { { "Check", 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("JoinRoom", RpcTarget.All,true);

        }

    }

    [PunRPC]
    public void Check_Restart(Player player)
    {
        if (!DataManager.Instance.Check_Crystal((int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]))
            return;

        //자신은 지우고
        if (player == PhotonNetwork.LocalPlayer)
        {
            Hashtable customRoomProperties = new Hashtable() { { "Check", 1 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);
        }
        //상대는 보이게
        else
        {
            UIManager.Instance.Start_Message_Popup(enum_Msg.Rematch);
        }


        if((int)PhotonNetwork.LocalPlayer.CustomProperties["Check"] == 1
            && (int)PhotonNetwork.CurrentRoom.GetPlayer(Enemy_id).CustomProperties["Check"] == 1){

            Rpc_ReStart_Game();

        }
    }
    public void Player_Num()
    {
        StartCoroutine("Co_Player_num");
    }

    IEnumerator Co_Player_num()
    {
        while (true)
        {
            Debug.Log("플레이어 숫자 = " + PhotonNetwork.CountOfPlayers);
            UIManager.Instance.Txt_Player_Count.text = PhotonNetwork.CountOfPlayers.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

#endregion

#region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }
    /// <summary>
    /// 마스터 연결 성공
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log("room : " + PhotonNetwork.CountOfRooms);

        //Connect();
    }

    /// <summary>
    /// 포톤서버 연결 끊음
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.LogWarningFormat("OnDisconnected {0}", cause);
        StopCoroutine("Co_Play");
    }

    /// <summary>
    /// 랜덤 룸 조인에 실패
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //룸이 없어서 실패 한다 
        Debug.Log("OnJoinRandomFailed .CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[]{"Map" ,"StartTime"};
        roomOptions.CustomRoomProperties = new Hashtable() { { "Map", (int)multi_Room } , {"StartTime" ,0 } };
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom("Room_" + Random.Range(0, 100000), roomOptions);

    }

    /// <summary>
    /// 랜덤 룸에 조인 완료
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log((int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]);

        //조인 했을떄 방장이 아니면
        if (!PhotonNetwork.IsMasterClient)
        {
            Enemy_id = PhotonNetwork.MasterClient.ActorNumber;
            UIManager.Instance.Set_Multi_Player(false, PhotonNetwork.MasterClient.NickName);
        }

    }

    /// <summary>
    /// 플레이어가 룸에 진입
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerEnteredRoom(Player other)
    {
       // Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");

        }
        else
        {   //방장일때만 시작
            Debug.Log("We load the 'Room for 2' ");
            Enemy_id = other.ActorNumber;
            UIManager.Instance.Set_Multi_Player(false, other.NickName);
            pv.RPC("JoinRoom", RpcTarget.All,false);

        }

        if (PhotonNetwork.IsMasterClient)
        {
           // Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

        }
    }

    /// <summary>
    /// 플레이어가 룸에서 나감
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerLeftRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        switch (UIManager.Instance.game_Stat)
        {
            case Game_Stat.Main:
                if (other != PhotonNetwork.LocalPlayer)
                {
                   // Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                    UIManager.Instance.Set_Multi_Player(false, "Search");
                    UIManager.Instance.Searching();

                }
                break;

            case Game_Stat.Game:
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    Hashtable customRoomProperties = new Hashtable() { { "Score", 99999999999999 } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);
                    UIManager.Instance.Set_GameOver_UI();

                }
                break;

            case Game_Stat.End:

                if (other != PhotonNetwork.LocalPlayer)
                {
                    UIManager.Instance.Start_Message_Popup(enum_Msg.Exit);
                    UIManager.Instance.Btn_Multi_ReStart.gameObject.SetActive(false);
                }
                
                    
                break;
            default:
                break;
        }


    }


#endregion
}
