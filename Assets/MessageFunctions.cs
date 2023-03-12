using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageFunctions : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public void ShowMessage (string message)
    {
        text.text = message;
    }

    public void HideMessage ()
    {
        Destroy(gameObject);
    }
}
