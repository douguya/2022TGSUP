using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
public class sugorokuManager : MonoBehaviourPunCallbacks
{

    private int XGoal, YGoal;                       //ゴールの座標

    public GameObject[] Player = new GameObject[4]; //プレイヤーオブジェクト取得
    public Width[] height = new Width[10];                             //Massの縦列のオブジェクトの取得・一番下で二次元配列にしている
    private int Playerturn = 0;                     //プレイヤーの手番管理

    private int Playcount = 0;                      //プレイヤーの参加人数
    public int play = 0;                           //誰の番か
    public Hashtable hashRoom;
    public GameObject GameStartButton;
    public GameObject Dcomment;
    public GameObject SceneManager;
    public GameObject RadyButton;
    private bool gamestart = false;
    public int playersnum;
    public int Goalcount=0;


    private void Awake()
    {
  
    }


    void Start()
    {
        Dcomment = GameObject.Find("DayComment");


    }


    void Update()
    {
        
        if (gamestart)
        {
            //   photonView.RPC(nameof(SugorokuTUrntoRPC), RpcTarget.All);

        }

    }

    




    [PunRPC]
    private void GoalClear()//全てのマスのゴールを消す
    {
        for (int i = 0; i < height.Length; i++)
        {
            for (int l = 0; l < height[0].width.Length; l++)
            {
                // Debug.Log(height[i].width[l]);
                height[i].width[l].GetComponent<Mass>().GoalOff();//ゴールを消していく
            }
        }
    }



    private void GoalDecision()//初めてゴールを出現させる
    {
      
        int Week, Day;
        photonView.RPC(nameof(GoalClear), RpcTarget.All);//ランダムなゴールの場所を入れる
                                               //全てのマスのゴールを消す
        do {
            Week = Random.Range(0, height.Length);                  //week・横の列のランダム
            Day = Random.Range(0, height[0].width.Length);          //day・縦の列のランダム
       
        } while (height[Week].width[Day].GetComponent<Mass>().invalid == true);//ランダムに選んだマスが存在しているものを見つけるまで繰り返す

        Debug.Log("EWEW"+height[Week].width[Day]);
        photonView.RPC(nameof(GoalPutRPC), RpcTarget.All, Week, Day);                        //ゴール配列番号を記憶

    }
    
    public void GoalAgain()                                         //ゴールの再設置(同じ月にならないように)
    {
        int Week, Day;                                              //ランダムなゴールの場所を入れる
        photonView.RPC(nameof(GoalClear), RpcTarget.All);                                            //全てのマスのゴールを消す
        do
        {
            Week = Random.Range(0, height.Length);                  //横の列のランダム
            Day = Random.Range(0, height[0].width.Length);          //縦の列のランダム
        } while (height[Week].width[Day].GetComponent<Mass>().invalid == true && MonthCount(Day, Week) == true);//選んだマスが存在しているもの＆同じ月じゃないものを見つけるまで繰り返す
        photonView.RPC(nameof(GoalPutRPC), RpcTarget.All, Week, Day);
      //  GoalPutRPC(Week,Day);
    }

    [PunRPC]
    public void GoalPutRPC(int we,int da ){
     
        height[we].width[da].GetComponent<Mass>().GoalOn();      //ゴールの設置
        XGoal = da; YGoal = we;                                  //ゴール配列番号を記憶

        }





    private bool MonthCount(int x, int y)//ゴールと同じ月か判断する
    {
        if (WhichMonth(XGoal, YGoal) == WhichMonth(x, y))//同じ月ならtrue
        {
            return true;
        }
        else//違う月ならfalse
        {
            return false;
        }
    }
    private int WhichMonth(int x, int y)//x,yが何月にいるのか調べる
    {
        int Month = 0;
        if (x < height[0].width.Length / 2 && y < height.Length / 2) { Month = 1; }//左上の月にいるかどうか
        if (height[0].width.Length / 2 <= x && y < height.Length / 2) { Month = 2; }//左上の月にいるかどうか
        if (x < height[0].width.Length / 2 && height.Length / 2 < y) { Month = 3; }//左上の月にいるかどうか
        if (height[0].width.Length / 2 <= x && height.Length / 2 < y) { Month = 4; }//左上の月にいるかどうか
        return Month;

    }






    //======================================================================================================


    public void StartOfimitation()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        photonView.RPC(nameof(hashRoom_StartUp), RpcTarget.AllViaServer);
        StartCoroutine(StartOfimitation_TransitionToResult());
       

    }


    public IEnumerator StartOfimitation_TransitionToResult() 　　　//リザルトに飛ぶ
    {
        yield return new WaitForSeconds(0.4f);
        Start_Col_Debac();
        yield break;

    }



    public void Start_Col_Debac()
    { 
        GoalDecision();//ゴールの選択
        photonView.RPC(nameof(RadyClose), RpcTarget.All);
        GameStartButton.SetActive(false);
        photonView.RPC(nameof(AbleToPlayerControl), RpcTarget.All);
    }


    [PunRPC]
    public void RadyClose()
    {
        RadyButton.SetActive(false);

    }





    [PunRPC]
    public void hashRoom_StartUp()
    {
        hashRoom = new Hashtable();
        hashRoom["Turn_of_Player"] = 0;
        hashRoom["GoalCount"] =0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashRoom);

    }




    [PunRPC]
    public  void AbleToPlayerControl()
    {

        play = (int)PhotonNetwork.CurrentRoom.CustomProperties["Turn_of_Player"]; //Turn_of_Playerの値を取得　*可読性のため

        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[play]){
            Player[play].GetComponent<PlayerStatus>().TurnDice();             //プレイヤーをコントロール出来るようにする
           
        }
        
    }


    public void  AfterMoving()
    {
        //  Debug.Log(Player[play].GetComponent<PlayerStatus>().Goalup);
        if (Player[play].GetComponent<PlayerStatus>().Goalup == true)   //もしこの手番にゴールしていたら
        {

            photonView.RPC(nameof(GettingGoal), RpcTarget.All);
            Player[play].GetComponent<PlayerStatus>().Goalup = false;   //ゴール宣言取り消し
            GoalAgain();
      
        }

        playerRounded();
    }


    [PunRPC]
    public void GettingGoal()
    {

        Goalcount++;

        if (Goalcount >= 4)   //ゴールした数が４なら
        {
            photonView.RPC(nameof(GameFinish), RpcTarget.All);
            //ゲーム終了
        }

    }








    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // 更新されたルームのカスタムプロパティのペアをコンソールに出力する
       




       


    }




        public void playerRounded()
    {
        play++;
        if (play >= PhotonNetwork.PlayerList.Length)//プレイヤー参加人数を超えたら
        {

            play = 0;     //プレイヤー0の手番になる*****************
        }
      //  Debug.Log("################################"+ hashRoom);
        //Debug.Log("################################" +hashRoom["Turn_of_Player"]);
        hashRoom["Turn_of_Player"] = play;

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashRoom);


        StartCoroutine(playerRounded_Coroutine());
       
        


    }





    public IEnumerator playerRounded_Coroutine()
    {

        yield return new WaitForSeconds(0.2f);
        AbleToPlayerControl_Demo();

        yield break;
    }



    public void AbleToPlayerControl_Demo()
    {
        photonView.RPC(nameof(AbleToPlayerControl), RpcTarget.All);
    }






    //  public IEnumerator AfterMoving_Coroutine()
    //  {
    //
    //  yield return new WaitForSeconds(1.2f);



    //     yield break;
    //   }




    [PunRPC]
    public void GameFinish()
    {

        SceneManager.GetComponent<SceneManagaer>().TransitionToResult();
        Debug.Log("ゲーム終了!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }



    public void daycommentoff()
    {
        photonView.RPC(nameof(Daycommentoff), RpcTarget.All);
    }

    [PunRPC]
    public void Daycommentoff()//引っ込める
    {

        Dcomment.GetComponent<DayComment>().DayCommentoff();

    }




}


[System.Serializable]
public class Width//weekの子・横列のオブジェクトの取得
{
    public  GameObject[] width;




}