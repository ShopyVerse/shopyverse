using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayfabCoinController : MonoBehaviour
{
    public BowlingGeneral BG;

    public TMP_Text metacoinsValueText;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetVirtualCurrencies();
        }
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI
            .GetUserInventory(new GetUserInventoryRequest(),
            OnGetUserInventorySucces,
            OnError);
    }

    public void OnGetUserInventorySucces(GetUserInventoryResult result)
    {
        int metacoins = result.VirtualCurrency["MC"];
        metacoinsValueText.text = metacoins.ToString();
    }

    public void GrantVirtualCurrency()
    {
        var request =
            new AddUserVirtualCurrencyRequest {
                VirtualCurrency = "MC",
                Amount = 10
            };
        PlayFabClientAPI.AddUserVirtualCurrency (
            request,
            OnGrantVirtualCurrencySuccess,
            OnError
        );
    }

    public void OnGrantVirtualCurrencySuccess(
        ModifyUserVirtualCurrencyResult result
    )
    {
        Debug.Log("Currenct Granted!!!!!!");
    }

    public void OnError(PlayFabError error)
    {
        Debug.Log("Error:" + error.ErrorMessage);
    }
}
