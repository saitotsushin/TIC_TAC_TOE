using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    // public GameObject BattleMg;
    public static OnlineManager instance;
    public GameObject mPlayer;
    public BattlePlayer _BattlePlayer;
    PunTurnManager punTurnManager = default;
    public Text MyPlayerName;
    public Text OtherPlayerName;
    public Text MyPlayerNameNumber;
    public Text OtherPlayerNameNumber;
    public GameObject WaiteMessage;
    public List<Sprite> PlayerIconList;
    public Image IconPlayer1;
    public Image IconPlayer2;
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
        PhotonNetwork.ConnectUsingSettings();
    }
    //参考
    //https://enia.hatenablog.com/entry/unity/introduction/21
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinRandomRoom();
        //ルームから退出するには、PhotonNetwork.LeaveRoom()を使用します。ルームから退出した後は、元のマスターサーバーへ再び転送されるので、OnConnectedToMaster()コールバックも呼ばれます。        
        // https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/322089#%E3%83%AB%E3%83%BC%E3%83%A0%E3%81%8B%E3%82%89%E9%80%80%E5%87%BA
    }
    // 入室に失敗した場合に呼ばれるコールバック
    // １人目は部屋がないため必ず失敗するので部屋を作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // 最大8人まで入室可能
        PhotonNetwork.CreateRoom("Room", roomOptions); //第一引数はルーム名
    }
    // ルームから退出した時に呼ばれるコールバック
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
        Debug.Log("相手がルームから退出しました");
        LeaveRoom();
        SceneManager.LoadScene("Main");
    }
    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        string Nickname = StartForm.instance.inputField.text;
        if(Nickname == ""){
            Nickname = "guest" + UnityEngine.Random.Range(1000, 9999);
        }
        MyPlayerName.text = Nickname;

        Debug.Log("入室しました");
        mPlayer = PhotonNetwork.Instantiate("Player",this.transform.position, Quaternion.identity);
        _BattlePlayer = mPlayer.GetComponent<BattlePlayer>();
        _BattlePlayer.NickName = Nickname;
        StartForm.instance.HideForm();
        
        PhotonNetwork.NickName = Nickname;

        SetPlayerName();
    }
    // 他のプレイヤーが入室してきた時
    public override void OnPlayerEnteredRoom(Player _Player)
    {
        Debug.Log("他のプレイヤーが入室してきた時 OnPlayerEnteredRoom");
        WaiteMessage.SetActive(false);
        SetPlayerName();
        
    }
    public void SetPlayerName(){
        var players = PhotonNetwork.PlayerList;
        var others = PhotonNetwork.PlayerListOthers;
        foreach (var player in others) {
            WaiteMessage.SetActive(false);
            StartGame();
        }
    }
    public void SendTouch(int _PosX, int _PosY,Vector3 _pos,int _nowPlayer){
        _BattlePlayer = mPlayer.GetComponent<BattlePlayer>();
        _BattlePlayer.SendTouch(_PosX,_PosY,_pos,_nowPlayer);
    }
    public void StartGame(){
        _BattlePlayer.SendPlayerData(
            PhotonNetwork.NickName
        );
        SetPlayerIcon();
    }
    public void SetPlayerIcon(){
        int _ActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        if(_ActorNumber == 1){
            IconPlayer1.sprite = PlayerIconList[0];
            IconPlayer2.sprite = PlayerIconList[1];
            MyPlayerNameNumber.text = "1P";
            OtherPlayerNameNumber.text = "2P";
        }else{
            IconPlayer1.sprite = PlayerIconList[1];
            IconPlayer2.sprite = PlayerIconList[0];
            MyPlayerNameNumber.text = "2P";
            OtherPlayerNameNumber.text = "1P";
        }
    }
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
}
