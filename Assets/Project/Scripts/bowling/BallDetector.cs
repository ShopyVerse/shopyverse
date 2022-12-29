using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    public bool ball_detected = false;
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (!ball_detected)
            {
                ball_detected = true;
                 StartCoroutine(FalseMaker());
            }

        }
    }

    IEnumerator FalseMaker()
    {
         yield return new WaitForSeconds(4);
         ball_detected = false;
    }

}
