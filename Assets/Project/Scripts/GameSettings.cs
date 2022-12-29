using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int speed_case = 0;

    public void CharacterMovementSpeed()
    {
        if (speed_case == 0)
        {
            speed_case = 1;
        }
        else
        {
            speed_case = 0;
        }
    }
}
