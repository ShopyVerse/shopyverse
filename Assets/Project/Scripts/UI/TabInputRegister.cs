using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabInputRegister : MonoBehaviour
{  //Inputs
   public TMP_InputField Email; // 0
   public TMP_InputField Password; // 1
   public TMP_InputField Username; // 2
   public TMP_InputField Aurora; // 3
   
   public int InputSelected;

   private void Update()
   {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if(InputSelected>3)
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
            if(InputSelected == 2)
            {
                Username.Select();                            
            }
            if(InputSelected == 3)
            {
                Aurora.Select();                            
            }
    }
   public void EmailSelected() => InputSelected = 0;
   public void PasswordSelected() => InputSelected = 1;
   public void UsernameSelected() => InputSelected = 2;
   public void AuroraSelected() => InputSelected = 3;

}
