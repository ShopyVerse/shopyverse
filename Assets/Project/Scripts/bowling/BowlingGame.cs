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

    public BowlingGeneral BG;

    public BallDetector BD;

    public bool isFinished;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player = GameObject.FindWithTag("Player");
            BG = Player.GetComponent<BowlingGeneral>();
            if (Input.GetKey(KeyCode.Q))
            {
                ResetPins();
                Debug.Log("basildı");
            }
        }
    }

    void Start()
    {
        //pins = GameObject.FindGameObjectsWithTag("Pin"); // çekmek yerine elle atılcak
        Pv = GetComponent<PhotonView>();
        positions = new Vector3[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }
    }

    private void Update()
    {
        Pv.RPC("StartRound", RpcTarget.All, null);
        Pv.RPC("Round", RpcTarget.All, null);
        Pv.RPC("roundController", RpcTarget.All, null);
        Pv.RPC("roundtable", RpcTarget.All, roundCounter);
    }

    [PunRPC]
    public void roundtable(int roundCounter)
    {
        round.text = roundCounter.ToString();
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
    void Round()
    {
        if (BD.ball_detected == true)
        {
            if (!updated)
            {
                StartCoroutine(roundincrement());
                updated = true;
            }
            StartCoroutine(Scorebool());
        }
    }

    IEnumerator roundincrement()
    {
        yield return new WaitForSeconds(6);
        roundCounter++;
        ResetPins();
    }

    [PunRPC]
    public void roundController()
    {
        if (roundCounter == 1)
        {
            RoundName = p1_text.text;
        }
        else if (roundCounter == 2)
        {
            RoundName = p2_text.text;
        }
        else if (roundCounter == 3)
        {
            RoundName = p1_text.text;
        }
        else if (roundCounter == 4)
        {
            RoundName = p2_text.text;
        }
        else if (roundCounter == 5)
        {
            RoundName = "";

            isFinished = true;
            p1_score = int.Parse(p1_scoretext.text);
            p2_score = int.Parse(p2_scoretext.text);

            if (p1_score > p2_score)
            {
                WinnerName = p1_text.text;
            }
            else if (p1_score < p2_score)
            {
                WinnerName = p2_text.text;
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
        p2_text.text = "";
        p1_text.text = "";
        p1_scoretext.text = "";
        p2_scoretext.text = "";
        round.text = "";
        roundCounter = 0;
        WinnerName = "";
        isFinished = false;
        startround = false;
    }

    bool startround = false;

    [PunRPC]
    public void StartRound()
    {
        if (p1_text.text != "" && p2_text.text != "")
        {
            if (!startround)
            {
                roundCounter = 1;
                startround = true;
            }
        }
    }
}
