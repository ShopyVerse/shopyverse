using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabInputLogin : MonoBehaviour
{
    //Inputs
   public TMP_InputField Email; // 0
   public TMP_InputField Password; // 1
   
   public int InputSelected;

   private void Update()
   {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if(InputSelected>1)
            {
                InputSelected = 0;
            }
        }
        SelectInput();

        
   }
   public void SelectInput()
    {
            if(InputSelected == 0)
            {
                Email.Select();                
            }
            if(InputSelected == 1)
            {
                Password.Select();                            
            }
    }
   public void EmailSelected() => InputSelected = 0;
   public void PasswordSelected() => InputSelected = 1;
}
