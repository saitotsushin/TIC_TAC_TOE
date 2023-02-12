using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattlePlayer : MonoBehaviourPunCallbacks {
    public int data_x;
    public int data_z;
    public byte decision;
    private byte sendDecision;

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

        if (_photonView.IsMine) {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
            }
        }
        //何かしらをクリックした場合、送信用に保持しておく
        if (decision != 0) sendDecision = decision;
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //     // オーナーの場合
    //     if (stream.IsWriting) {
    //         stream.SendNext(this.data_x);
    //         stream.SendNext(this.data_z);
    //         stream.SendNext(this.sendDecision);
    //         sendDecision = 0;
    //     }
    //     // オーナー以外の場合
    //     else {
    //         this.data_x = (int)stream.ReceiveNext();
    //         this.data_z = (int)stream.ReceiveNext();
    //         this.decision = (byte)stream.ReceiveNext();
    //     }
    // }
    public void SendTouch(int _PosX, int _PosY,Vector3 _pos,int _nowPlayer){
        Debug.Log("SendTouch Player");
        photonView.RPC(nameof(RpcSendTouchPos), RpcTarget.Others,_PosX,_PosY,_pos,_nowPlayer);
    }

    [PunRPC]
    private void RpcSendTouchPos(int _PosX, int _PosY,Vector3 _pos, int _nowPlayer) {
        Debug.Log(_PosX + "/" + _PosY);
        Vector2Int _PosXY = new Vector2Int(_PosX, _PosY);
        GManager.instance.SetMark(_pos,_PosXY,_nowPlayer);
        GManager.instance.CheckNext();
    }
}
