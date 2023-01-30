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
    public GameObject rail;    
    public GameObject rails_object;    

    public bool ball_Actived;

    public GameObject GameProps;
    public GameObject firstpoint;

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

                if(border != null)
                {
                    if (border.BD.ball_detected == true)
                    {
                        Pv.RPC("CountPinsDown", RpcTarget.All, null);                    
                    }
                }
                
            }
        }
         if (other.gameObject.tag == "Playland")
        {
            if (Pv.IsMine)
            {            
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
                border.ifexitplayer(0);
                score = 0;             
                Pv.RPC("ClearText", RpcTarget.All, null);   
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
         canvas = GameObject.FindWithTag("canvas");
         BowlingText = canvas.transform.GetChild(2).gameObject;
         getBallText = canvas.transform.GetChild(3).gameObject;
         second_player_waitingText = canvas.transform.GetChild(4).gameObject;

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
            rail.transform.position = rails_object.transform.position;
            Pv.RPC("BallDeactive", RpcTarget.All, null);
            ball_Actived = false;
            roundPlayed = false;
            Pv.RPC("GetParent2", RpcTarget.All, null);
        }
    }

    IEnumerator ballCanGo()
    {
        pins = border.pins;
        yield return new WaitForSeconds(2f);    
        Pv.RPC("NullParent2", RpcTarget.All, null);
        Pv.RPC("NullParent", RpcTarget.All, null);         
        rb.isKinematic = false;
       // rb.AddForce(ThrowingPoint.transform.forward * force);
      
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

    Quaternion rot = new Quaternion(0f,0f,0f,0f);

    [PunRPC]
    public void GetParent2()
    {
        rail.transform.parent = rails_object.transform;
        rail.transform.localPosition = new Vector3(0,0,0);
        rail.transform.rotation = rot;
       

    }

    [PunRPC]
    public void NullParent()
    {
        //myB_Ball_Parent.transform.SetParent(null);
         myB_Ball.transform.parent = firstpoint.transform;     
        myB_Ball.transform.position = firstpoint.transform.position;    
    }
    [PunRPC]
    public void NullParent2()
    {
        rail.transform.SetParent(null);        
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
                            Pv.RPC("ScoreUpdater", RpcTarget.AllBuffered, score);
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
