using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
 
public class GManager : MonoBehaviour
{
    public static GManager instance;
    public GameObject prefabCircle;
    public GameObject prefabDiamond;
    public GameObject resultObj;
    public TextMeshProUGUI infoTMP;
    int nowPlayer;
 
    int[,] board = new int[3, 3];//3*3のint型２次元配列を定義
    bool win;
    bool draw;

    public OnlineManager m_OnlineManager;
    void Awake ()
    {
        if (instance == null) {
        
            instance = this;     
        }
        else {
            Destroy (gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //2次元配列boardの全ての値を初期化（-1に）
        for (int i = 0; i < 3;i++)
        {
            for (int j = 0; j < 3;j++)
            {
                board[i, j] = -1;
            }
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        if(win||draw)return;
 
        infoTMP.text = (nowPlayer +1) + "P's Turn";
        bool next = false;
        if(!m_OnlineManager._BattlePlayer){
            return;
        }
        if((nowPlayer + 1) != m_OnlineManager._BattlePlayer.id){
            return;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(null !=hit.collider)
            {
                Vector3 pos = hit.collider.gameObject.transform.position;
                int x = (int)pos.x + 1;
                int y = (int)pos.y + 1;
                if( -1 == board[y,x])
                {
                    Vector2Int _PosXY = new Vector2Int(x,y);
                    SetMark(pos,_PosXY,OnlineManager.instance._BattlePlayer.id);
                    next = true;

                    m_OnlineManager.SendTouch(x, y,pos,OnlineManager.instance._BattlePlayer.id);

                }
            }
        }
        if (next)//勝敗チェック開始
        {
            CheckNext();
        }
    }
 
    public void OnClickRetry()
    {
        OnlineManager.instance.LeaveRoom();
        SceneManager.LoadScene("Main");
    }
    public void OnClickDebug()//デバッグ用（各配列の値を出力する）
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("(i,j) = (" + i + "," + j + ") = " + board[i, j]);
            }
        }
    }
    public void SetMark(Vector3 _Pos,Vector2Int _PosXY,int _nowPlayer){
        GameObject prefab = prefabDiamond;
        if(1 == _nowPlayer) prefab = prefabCircle;
        Instantiate(prefab, _Pos, Quaternion.identity);//ここまでは1次元配列ver.と同じ記載
        board[_PosXY.y,_PosXY.x] = nowPlayer; //2次元配列なので[x,y]にプレイヤー番号を入れる
    }
    public void ChangeTurn(){
        nowPlayer++;
        if(2 <= nowPlayer)
        {
            nowPlayer = 0;
        }
    }
    public void CheckNext(){
        win = false;
        int prefabNum;//プレファブが並ぶ数をカウントする変数
    
        //横並びの調査
        for (int i = 0; i < 3; i++)
        {
            prefabNum = 0;//初期化
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == -1 || board[i, j] != nowPlayer)//空きマスまた相手のコマがある場合
                {
                    prefabNum = 0;//並びをリセット
                }
                else
                {
                    prefabNum++;//自分のコマならカウント
                }
    
                if (prefabNum == 3)//３つ揃ったら勝ち
                {
                    resultObj.SetActive(true);
                    infoTMP.text = (nowPlayer +1) + "P Win!";
                    win = true;
                }
            }
        }
    
        //縦並びの調査
        for (int i = 0; i < 3; i++)
        {
            prefabNum = 0;
            for (int j = 0; j < 3; j++)
            {
                if (board[j, i] == -1 || board[j, i] != nowPlayer)
                {
                    prefabNum = 0;
                }
                else
                {
                    prefabNum++;
                }

                if (prefabNum == 3)
                {
                    resultObj.SetActive(true);
                    infoTMP.text = (nowPlayer +1) + "P Win!";
                    win = true;
                }
            }
        }
    
        //斜め並び（右上がり）の調査
        for (int i = 0; i < 3; i++)
        {
            prefabNum = 0;
            int up = 0;//上ずれ用
    
            for (int j = i; j < 3; j++)
            {
                if (board[j, up] == -1 || board[j, up] != nowPlayer)
                {
                    prefabNum = 0;
                }
                else
                {
                    prefabNum++;
                }

                if (prefabNum == 3)
                {
                    resultObj.SetActive(true);
                    infoTMP.text = (nowPlayer +1) + "P Win!";
                    win = true;
                }
    
                up++;
            }
        }

        //斜め並び（右下がり）の調査
        for (int i = 0; i < 3; i++)
        {
            int down = 2;//下ずれ用
            prefabNum = 0;
    
            for (int j = i; j < 3; j++)
            {
                if (board[j, down] == -1 || board[j, down] != nowPlayer)
                {
                    prefabNum = 0;
                }
                else
                {
                    prefabNum++;
                }

                if (prefabNum == 3)
                {
                    resultObj.SetActive(true);
                    infoTMP.text = (nowPlayer +1) + "P Win!";
                    win = true;
                }
    
                down--;
            }
        }
        if(!win)
        {
            //引き分け
            int cnt = 0; //空いてる場所を数える用
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == -1) //空きマスがあったら
                    {
                        cnt++;//カウントする
                    }
                }
            }
                
            if(0== cnt) //空きマスがない場合
            {
                resultObj.SetActive(true);
                infoTMP.text = "Draw!";//引き分け処理
                draw = true;
            }

            ChangeTurn();
        }
    }
}