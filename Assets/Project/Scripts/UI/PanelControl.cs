using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelControl : MonoBehaviour
{
    [SerializeField] Transform[] _UIObject;
    [SerializeField] RectTransform _parentObject;
    [SerializeField] GameObject _manInfo;
    [SerializeField] GameObject _womanInfo;
    [SerializeField] GameObject login_hud;
    [SerializeField] GameObject create_avatar_hud;

    public Color lastColorTxt;
  
    void Update()
    {
        ActiveControl(_UIObject);
        
    }
    private void Start()
    {
        _manInfo.SetActive(true);
        #if UNITY_WEBGL
            PlayerPrefs.SetString("Gender", "Man"); 
        #elif UNITY_ANDROID
            PlayerPrefs.SetString("GenderVr", "VRPlayer");
        #endif
        
    }
    void ActiveControl(Transform[] obj)
    {
        foreach (Transform item in obj)
        {
            if (item.localScale.x < 0.02f)
            {
                item.gameObject.SetActive(false);
            }
            if (item.localScale.x > 0.02f)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    public void SignUpPanel()
    {
        _UIObject[0].DOScaleX(1.7248f, 0.1f);
        _UIObject[1].DOScaleX(0, 0.1f);
        _UIObject[2].DOScaleX(0, 0.1f);
        _UIObject[3].DOScaleX(1, 0.2f);
        _UIObject[4].DOScaleX(1, 0.2f);
        _parentObject.DOAnchorPos(new Vector3(0,0,0), 0.4f);
        _manInfo.transform.parent.gameObject.SetActive(false);
        _womanInfo.transform.parent.gameObject.SetActive(false);
    }
    public void LoginPanel()
    {
        _UIObject[0].DOScaleX(0, 0.1f);
        _UIObject[1].DOScaleX(1, 0.1f);
        _UIObject[2].DOScaleX(1, 0.1f);
        _UIObject[3].DOScaleX(0, 0.2f);
        _UIObject[4].DOScaleX(0, 0.2f);
        _parentObject.DOAnchorPos(new Vector3(0, 50, 0), 0.4f);
        _manInfo.transform.parent.gameObject.SetActive(true);
        _womanInfo.transform.parent.gameObject.SetActive(true);


    }
    
    public void OnEnterGender(TextMeshProUGUI material)
    {
        material.color = Color.cyan;
    }
    public void OnExitGender(TextMeshProUGUI material)
    {
        material.color = lastColorTxt;
    }
    
    
    public void OnPointerDownGender(bool value)
    {
        OnSelect(value);
    }
    public void OnSelect(bool value)
    {
        Debug.Log(value);
#if UNITY_WEBGL
            if (value)
            {
                _manInfo.SetActive(false);
                _womanInfo.SetActive(true);
                PlayerPrefs.SetString("Gender", "Woman");
            }
            else
            {
                _manInfo.SetActive(true);
                _womanInfo.SetActive(false);
                PlayerPrefs.SetString("Gender", "Man");
            }         
#elif UNITY_ANDROID
            if (value)
            {
                _manInfo.SetActive(false);
                _womanInfo.SetActive(true);
                PlayerPrefs.SetString("GenderVr", "VRPlayerWoman");
            }
            else
            {
                _manInfo.SetActive(true);
                _womanInfo.SetActive(false);
                PlayerPrefs.SetString("GenderVr", "VRPlayer");
            }
#endif
}
    public void Open_CreateAvatarHud()
    {
        login_hud.SetActive(false);
        create_avatar_hud.SetActive(true);
    }
    public void Close_CreateAvatarHud()
    {
        login_hud.SetActive(true);
        create_avatar_hud.SetActive(false);
    }
}
