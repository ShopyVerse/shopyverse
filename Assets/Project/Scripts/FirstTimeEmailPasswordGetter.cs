using UnityEngine;
using TMPro;

public class FirstTimeEmailPasswordGetter : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _emailInput;
    
    [SerializeField]
    private TMP_InputField _passwordInput ;

    private void Start(){
        FillTextComponents("act:act");
    }

    public void FillTextComponents(string emailpassword)
    {
        string[] temp = emailpassword.Split(":");
        string email = temp[0];
        string password = temp[1];
        _emailInput.text = email;
        _passwordInput.text = password;
    }
}
