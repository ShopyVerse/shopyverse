using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxFunctions : MonoBehaviour
{
    [SerializeField] ContentSizeFitter contentSizeFitter;
    [SerializeField] TextMeshProUGUI showHideButtonText;

    bool isChatShowing = false;

    public void ToggleChat ()
    {
        isChatShowing = !isChatShowing;
        if (isChatShowing)
        {
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            showHideButtonText.text = "Hide Chat";
        }
        else
        {
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            showHideButtonText.text = "Show Chat";
        }

    }
}
