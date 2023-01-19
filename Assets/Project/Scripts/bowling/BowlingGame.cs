using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class BowlingGame : MonoBehaviourPunCallbacks
{
    PhotonView Pv;

    public GameObject[] pins;

    Vector3[] positions;

    public TMP_Text p1_text;

    public TMP_Text p1_scoretext;

    public TMP_Text p2_scoretext;

    public string p1_string;

    public string p2_string;

    public TMP_Text p2_text;

    public int p1_score;

    public int p2_score;

    public TMP_Text round;

    public string RoundName;

    public string WinnerName;

    public GameObject Player;

    public BallDetector BD;

    public bool isFinished;

    public GameObject[] players;

    public NetworkPlayerSync nps;
    public boardtextobject bto;
    public TextMeshProUGUI[] boardtexts;  

    public int int_p1;
    public int int_p2;
    public int int_s1;
    public int int_s2;
    public int int_r1;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.Q))
            {
                ResetPins();                
                
            }                                                          
            Pv.RPC("roundController", RpcTarget.All, null);
                                    
        }
    }

    void Start()
    {
        Pv = GetComponent<PhotonView>();
        positions = new Vector3[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }
        boardtexts = bto.boardtext;
    }

    private void Update()
    {
         
    }  

    void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = positions[i];
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }
    }

    public int roundCounter = 0;

    public bool updated = false;

    [PunRPC]
    public void Round()
    {
       
    }

    IEnumerator roundincrement()
    {
        yield return new WaitForSeconds(6);
        roundCounter++;
        bto.boardtext[int_r1].text = "Round: "+roundCounter.ToString();
        ResetPins();
    }

    [PunRPC]
    public void roundController()
    {
         if (p1_text.text != "" && p2_text.text != "")
        {
            if (!startround)
            {
                roundCounter = 1;
                bto.boardtext[int_r1].text = "Round: "+ roundCounter.ToString();
                startround = true;                
            }
        }
         if (BD.ball_detected == true)
        {
            if (!updated)
            {
                StartCoroutine(roundincrement());
                updated = true;
            }
            StartCoroutine(Scorebool());
        }
        if (roundCounter == 1)
        {
            RoundName = bto.boardtext[int_p1].text;
        }
        else if (roundCounter == 2)
        {
            RoundName = bto.boardtext[int_p2].text;
        }
        else if (roundCounter == 3)
        {
            RoundName = bto.boardtext[int_p1].text;
        }
        else if (roundCounter == 4)
        {
            RoundName = bto.boardtext[int_p2].text;
        }
        else if (roundCounter == 5)
        {
            RoundName = bto.boardtext[int_p1].text;
        }
        else if (roundCounter == 6)
        {
            RoundName = bto.boardtext[int_p2].text;
        }
        else if (roundCounter == 7)
        {
            RoundName = bto.boardtext[int_p1].text;
        }
        else if (roundCounter == 8)
        {
            RoundName = bto.boardtext[int_p2].text;
        }
        else if (roundCounter == 9)
        {
            RoundName = "";
            isFinished = true;
            p1_score = int.Parse(bto.boardtext[int_s1].text);
            p2_score = int.Parse(bto.boardtext[int_s2].text);

            if (p1_score > p2_score)
            {
                WinnerName = bto.boardtext[int_p1].text;
            }
            else if (p1_score < p2_score)
            {
                WinnerName = bto.boardtext[int_p2].text;
            }
            StartCoroutine(RoundClear());
        }
    }

    IEnumerator Scorebool()
    {
        yield return new WaitForSeconds(7);
        updated = false;
    }

    IEnumerator RoundClear()
    {
        yield return new WaitForSeconds(6);
        bto.boardtext[int_p1].text = "";
        bto.boardtext[int_p2].text = "";
        bto.boardtext[int_s1].text = "";
        bto.boardtext[int_s2].text = "";
        bto.boardtext[int_r1].text = "";
        roundCounter = 0;
        bto.boardtext[int_r1].text = "Round: "+roundCounter.ToString();
        WinnerName = "";
        isFinished = false;
        startround = false;
    }

    bool startround = false;

    [PunRPC]
    public void StartRound()
    {
       
    }

    public void SetP1Text(string newText)
    {
        bto.boardtext[int_p1].text = newText;
        photonView.RPC("UpdateP1Text", RpcTarget.AllBuffered, newText);
    }

    [PunRPC]
    public void UpdateP1Text(string newText)
    {
        bto.boardtext[int_p1].text = newText;
    }

    public void SetP2Text(string newText)
    {
        bto.boardtext[int_p2].text = newText;
        photonView.RPC("UpdateP2Text", RpcTarget.AllBuffered, newText);
    }

    [PunRPC]
    public void UpdateP2Text(string newText)
    {
        bto.boardtext[int_p2].text = newText;
    }

    public void clearP1Text(string clearText)
    {
        bto.boardtext[int_p1].text = clearText;
        bto.boardtext[int_s1].text = clearText;
        photonView.RPC("UpdateToClearP1Text", RpcTarget.AllBuffered, clearText);
    }

    [PunRPC]
    public void UpdateToClearP1Text(string clearText)
    {
        bto.boardtext[int_p1].text = clearText;
        bto.boardtext[int_s1].text = clearText;
    }
    public void clearP2Text(string clearText)
    {
        bto.boardtext[int_p2].text = clearText;
        bto.boardtext[int_s2].text = clearText;
        photonView.RPC("UpdateToClearP2Text", RpcTarget.AllBuffered, clearText);
    }

    [PunRPC]
    public void UpdateToClearP2Text(string clearText)
    {
        bto.boardtext[int_p2].text = clearText;
        bto.boardtext[int_s2].text = clearText;
    }
    public void P1score(string newScore)
    {        
        bto.boardtext[int_s1].text = newScore;
        photonView.RPC("UpdateP1score", RpcTarget.AllBuffered, newScore);
    }

    [PunRPC]
    public void UpdateP1score(string newScore)
    {        
        bto.boardtext[int_s1].text = newScore;
    }
    public void P2score(string newScore)
    {        
        bto.boardtext[int_s2].text = newScore;
        photonView.RPC("UpdateP2score", RpcTarget.AllBuffered, newScore);
    }

    [PunRPC]
    public void UpdateP2score(string newScore)
    {        
        bto.boardtext[int_s2].text = newScore;
    }

     public void ifexitplayer(int roundclear)
    {                
         roundCounter = roundclear;
        bto.boardtext[int_r1].text = roundclear.ToString(); 
        photonView.RPC("Punifexitplayer", RpcTarget.AllBuffered, roundclear);
    }

    [PunRPC]
    public void Punifexitplayer(int roundclear)
    {
        if (bto.boardtext[int_p1].text == "" || bto.boardtext[int_p2].text == "" )
        {
            roundCounter = roundclear;
            bto.boardtext[int_r1].text = roundclear.ToString();       
        }
         
    }
}
