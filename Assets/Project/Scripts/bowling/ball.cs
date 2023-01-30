using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
   public Animator _animator;
    public BowlingGeneral BG;


   private void Start() {
      _animator = GetComponent<Animator>();
   }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {            
            if (BG.ball_Actived == true)
            {
                StartCoroutine(ballCanGo());
            }
            
        }
    }
     IEnumerator ballCanGo()
    {
        yield return new WaitForSeconds(2f);
        _animator.SetTrigger("ball");
    }
}
