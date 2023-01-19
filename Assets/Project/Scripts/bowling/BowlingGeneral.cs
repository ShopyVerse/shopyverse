using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class BowlingGeneral : MonoBehaviourPunCallbacks
{
    public GameObject BowlingText;

    public GameObject getBallText;
    public GameObject second_player_waitingText;

    public TextMeshProUGUI[] boardtexts;

    public TMP_Text Player1_text;

    public TMP_Text Player2_text;

    public TMP_Text P1_score_text;

    public TMP_Text P2_score_text;

    public GameObject p1_score_obj;

    public GameObject p2_score_obj;

    public GameObject player1_obj;

    public GameObject player2_obj;

    public GameObject canvas;

    public GameObject board_hud;

    public PhotonView Pv;

    public NetworkPlayerSync nps;

    bool isLoggined = false;

    public GameObject myB_Ball;

    public bool ball_Actived;

    public GameObject GameProps;

    public float speed = 1000;

    public Rigidbody rb;

    float force = 70;

    public bool shooting;

    public int score = 0;

    public Transform ThrowingPoint;

    public GameObject BorderObj;

    public BowlingGame border;

    public bool roundPlayed;    

    public Animator _animator;

    public bool played = false;

    public PlayfabCoinController PFCC;

    public GameObject[] pins;

    public GameObject BoardTextObj;

    public boardtextobject bto;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "border")
        {
            if (Pv.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (BorderObj == null)
                    {
                        BorderObj = other.gameObject;
                    }
                    if (border == null)
                    {
                        border = BorderObj.GetComponent<BowlingGame>();
                        Player1_text = border.p1_text;
                        Player2_text = border.p2_text;
                        P1_score_text = border.p1_scoretext;
                        P2_score_text = border.p2_scoretext;
                    }
                }
                if (!isLoggined)
                {
                    BowlingText.SetActive(true);
                }
                if (isLoggined)
                {
                    if (border.isFinished)
                    {
                        if (border.WinnerName == Pv.Owner.NickName)
                        {
                            if (!played)
                            {
                                _animator.SetTrigger("Victory");
                                PFCC.GrantVirtualCurrency();
                                played = true;
                                StartCoroutine(RoundFinish());
                            }
                        }
                        else
                        {
                            if (!played)
                            {
                                _animator.SetTrigger("Defeat");
                                played = true;
                                StartCoroutine(RoundFinish());
                            }
                        }
                    }
                    if(boardtexts[border.int_p1].text == nps.username.text)
                    {
                        if(boardtexts[border.int_p2].text == "" || boardtexts[border.int_p2].text == null)
                        {
                            second_player_waitingText.SetActive(true);
                        }             
                        else
                        {
                             second_player_waitingText.SetActive(false);
                        }                               
                        
                    }
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    isLoggined = true;
                    Pv.RPC("SetName", RpcTarget.All, null);
                    if (isLoggined)
                    {
                        BowlingText.SetActive(false);
                    }
                }
                if (isLoggined)
                {
                    Pv.RPC("CountPinsDown", RpcTarget.All, null);                    
                }
            }
        }
         if (other.gameObject.tag == "Playland")
        {
            if (Pv.IsMine)
            {
               Pv.RPC("ScoreUpdater", RpcTarget.AllBuffered, score);                
               if (Input.GetKey(KeyCode.Space))
                {
                    if (ball_Actived)
                    {
                        _animator.SetTrigger("Throwing");
                        StartCoroutine(ballCanGo());
                    }
                }
                if (shooting)
                {
                    StartCoroutine(BallReset());
                }
            }
        }
        if (other.gameObject.tag == "GetBall")
        {
            if (Pv.IsMine)
            {
                if (isLoggined)
                {
                    if (border.RoundName == Pv.Owner.NickName)
                    {
                        if (ball_Actived == false)
                        {
                            getBallText.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.G))
                            {
                                Pv.RPC("BallActive", RpcTarget.All, null);
                                ball_Actived = true;
                                if (ball_Actived)
                                {
                                    getBallText.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Playland")
        {
            if (Pv.IsMine)
            {
                BowlingText.SetActive(false);
                 second_player_waitingText.SetActive(false);
                isLoggined = false;
                Pv.RPC("ClearText", RpcTarget.All, null);
                border.ifexitplayer(0);
                score = 0;
                Pv.RPC("canNull", RpcTarget.All, null);
                
            }
        }
        if (other.gameObject.tag == "border")
        {
            if (Pv.IsMine)
            {
                BowlingText.SetActive(false);
            }
        }
        if (other.gameObject.tag == "GetBall")
        {
            if (Pv.IsMine)
            {
                getBallText.SetActive(false);
            }
        }
    }

    public void Awake()
    {
        BoardTextObj = GameObject.FindGameObjectWithTag("BoardText");
        bto = BoardTextObj.GetComponent<boardtextobject>();
    }

    void Update()
    {
        if (Pv.IsMine)
        {
            
            if (ball_Actived == false)
            {
                pins = null;
            }
           
             
        }
    }

    void Start()
    {
        rb = myB_Ball.GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        rb.maxAngularVelocity = 50;
        nps = gameObject.GetComponent<NetworkPlayerSync>();

        boardtexts = bto.boardtext;
        
        // BorderObj = GameObject.FindWithTag("border");
        // border = BorderObj.GetComponent<BowlingGame>();
        // board_hud = GameObject.FindWithTag("board_hud");
         canvas = GameObject.FindWithTag("canvas");
         BowlingText = canvas.transform.GetChild(2).gameObject;
         getBallText = canvas.transform.GetChild(3).gameObject;
         second_player_waitingText = canvas.transform.GetChild(4).gameObject;

        // player1_obj = board_hud.transform.GetChild(0).gameObject;
        // player2_obj = board_hud.transform.GetChild(1).gameObject;
        // p1_score_obj = board_hud.transform.GetChild(2).gameObject;
        // p2_score_obj = board_hud.transform.GetChild(3).gameObject;

        // P1_score_text = p1_score_obj.GetComponent<TMP_Text>();
        // P2_score_text = p2_score_obj.GetComponent<TMP_Text>();
        // Player1_text = player1_obj.GetComponent<TMP_Text>();
        // Player2_text = player2_obj.GetComponent<TMP_Text>();
    }

    IEnumerator BallReset()
    {
        yield return new WaitForSeconds(10);
        if (Pv.IsMine)
        {
            Pv.RPC("GetParent", RpcTarget.All, null);
            shooting = false;
            rb.isKinematic = true;
            myB_Ball.transform.position = GameProps.transform.position;
            Pv.RPC("BallDeactive", RpcTarget.All, null);
            ball_Actived = false;
            roundPlayed = false;
        }
    }

    IEnumerator ballCanGo()
    {
        yield return new WaitForSeconds(2f);
        pins = border.pins;
        Pv.RPC("NullParent", RpcTarget.All, null);
        rb.isKinematic = false;
        rb.AddForce(ThrowingPoint.transform.forward * force);
        shooting = true;
        roundPlayed = true;
    }

    [PunRPC]
    public void SetName()
    {
        if (boardtexts[border.int_p1].text == "" ||boardtexts[border.int_p1].text == null)
        {
             border.SetP1Text(nps.username.text);            
        }
        else if (boardtexts[border.int_p2].text == "" || boardtexts[border.int_p2].text == null)
        {
            if (boardtexts[border.int_p1].text != nps.username.text)
            {
                 border.SetP2Text(nps.username.text);                 
            }
        }
    }

    [PunRPC]
    public void ClearText()
    {
        if (boardtexts[border.int_p1].text == nps.username.text)
        {
            border.clearP1Text("");     
        }
        if (boardtexts[border.int_p2].text == nps.username.text)
        {
            border.clearP2Text("");     
        }
    }

    [PunRPC]
    public void canNull()
    {
        BorderObj = null;
        border = null;
    }

    [PunRPC]
    public void BallActive()
    {
        myB_Ball.SetActive(true);
    }

    [PunRPC]
    public void BallDeactive()
    {
        myB_Ball.SetActive(false);
    }

    [PunRPC]
    public void GetParent()
    {
        myB_Ball.transform.parent = GameProps.transform;
    }

    [PunRPC]
    public void NullParent()
    {
        myB_Ball.transform.SetParent(null);
    }

    [PunRPC]
    public void CountPinsDown()
    {
        if(pins != null)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                if (
                    pins[i].transform.eulerAngles.z > 5 &&
                    pins[i].transform.eulerAngles.z < 355 &&
                    pins[i].activeSelf
                )
                {
                    if (Pv.IsMine)
                    {
                        StartCoroutine(ScoreCounter());
                    }

                    if (canFalse)
                    {
                        if (Pv.IsMine)
                        {
                            score++;
                            canFalse = false;
                        }
                        pins[i].SetActive(false);
                    }
                }
            }
        }
        
    }

    [PunRPC]
    public void ScoreUpdater(int score)
    {
        if (BorderObj != null)
        {
            if (boardtexts[border.int_p1].text == nps.username.text)
            {
                border.P1score(score.ToString());
            }
            if (boardtexts[border.int_p2].text == nps.username.text)
            {
                border.P2score(score.ToString());
            }
        }
    }

    public bool canFalse = false;

    IEnumerator ScoreCounter()
    {
        yield return new WaitForSeconds(2);

        canFalse = true;
    }

    IEnumerator RoundFinish()
    {
        yield return new WaitForSeconds(6);

        isLoggined = false;
        score = 0;
        played = false;
    }
}
