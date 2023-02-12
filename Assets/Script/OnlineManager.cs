using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    // public GameObject BattleMg;
    public static OnlineManager instance;
    public GameObject mPlayer;
    public BattlePlayer _BattlePlayer;
    public GameObject BattleMg;
    PunTurnManager punTurnManager = default;
    // public BattleMg BattleMgSC;

    private static Hashtable _hashtable = new Hashtable();
    void Awake ()
    {
        if (instance == null) {
        
            instance = this;     
        }
        else {
            Destroy (gameObject);
        }
    }
    public void StartOnline(){
        Debug.Log("StartOnline");
        PhotonNetwork.ConnectUsingSettings();
        //使用するキャッシュ
        // BattleMg = GameObject.Find ("BattleMg");
        // BattleMgSC = BattleMg.GetComponent<BattleMg>();
    }
    private void Start() {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する

        // //使用するキャッシュ
        // BattleMg = GameObject.Find ("BattleMg");
        // BattleMgSC = BattleMg.GetComponent<BattleMg>();
    }
    //参考
    //https://enia.hatenablog.com/entry/unity/introduction/21
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        Debug.Log("OnConnectedToMaster");
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinRandomRoom();
    }
    // 入室に失敗した場合に呼ばれるコールバック
    // １人目は部屋がないため必ず失敗するので部屋を作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // 最大8人まで入室可能
        PhotonNetwork.CreateRoom("Room", roomOptions); //第一引数はルーム名
    }
    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        Debug.Log("入室しました");
        mPlayer = PhotonNetwork.Instantiate("Player",this.transform.position, Quaternion.identity);
        // BattlePlayer mPlayerSC = mPlayer.GetComponent<BattlePlayer>();
        // mPlayerSC.CreatePlayer();
        _BattlePlayer = mPlayer.GetComponent<BattlePlayer>();

        var players = PhotonNetwork.PlayerList;
        var others = PhotonNetwork.PlayerListOthers;
    }
    // 他のプレイヤーが入室してきた時
    public override void OnPlayerEnteredRoom(Player _Player)
    {
        Debug.Log("他のプレイヤーが入室してきた時 OnPlayerEnteredRoom");
    }
    public void SendTouch(int _PosX, int _PosY,Vector3 _pos,int _nowPlayer){
        Debug.Log("SendTouch");
        _BattlePlayer = mPlayer.GetComponent<BattlePlayer>();
        _BattlePlayer.SendTouch(_PosX,_PosY,_pos,_nowPlayer);
    }

}
