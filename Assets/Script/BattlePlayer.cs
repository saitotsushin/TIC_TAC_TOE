using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattlePlayer : MonoBehaviourPunCallbacks {
    public int data_x;
    public int data_z;
    public byte decision;
    private byte sendDecision;
    public string NickName;

    public int id;
    private PhotonView _photonView;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのIDを取得する。
        _photonView = this.GetComponent<PhotonView>();
        id = _photonView.OwnerActorNr;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SendTouch(int _PosX, int _PosY,Vector3 _pos,int _nowPlayer){
        photonView.RPC(nameof(RpcSendTouchPos), RpcTarget.Others,_PosX,_PosY,_pos,_nowPlayer);
    }

    [PunRPC]
    private void RpcSendTouchPos(int _PosX, int _PosY,Vector3 _pos, int _nowPlayer) {
        Vector2Int _PosXY = new Vector2Int(_PosX, _PosY);
        GManager.instance.SetMark(_pos,_PosXY,_nowPlayer);
        GManager.instance.CheckNext();
    }
    public void SendPlayerData(string _NickName){
        photonView.RPC(nameof(RpcSendNickName), RpcTarget.Others,_NickName);
    }

    [PunRPC]
    private void RpcSendNickName(string _NickName) {
        OnlineManager.instance.OtherPlayerName.text = _NickName;
    }
}
