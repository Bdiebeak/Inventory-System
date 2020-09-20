using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public GameObject UIgo;
    public Image AccessUIgo;
    public float timeToAccess;
    
    private bool _accessEnabled;
    private float _currentTime;
    private float _requiredTime;

    private void OnMouseDown()
    {
        InitializeAccessTimer();
    }

    private void OnMouseDrag()
    {
        if (_accessEnabled)
        {
            ShowBackpackUI();
        }
        else
        {
            if (_currentTime < _requiredTime)
            {
                _currentTime += Time.deltaTime;
                AccessUIgo.fillAmount = 1 - (_requiredTime - _currentTime) / timeToAccess;
            }
            else
            {
                _accessEnabled = true;
                AccessUIgo.fillAmount = 1f;
            }
        }
    }

    private void OnMouseUp()
    {
        HideBackpackUI();
        ResetAccess();
        AccessUIgo.fillAmount = 0f;
    }

    private void ShowBackpackUI()
    {
        UIgo.SetActive(true);
    }
    
    private void HideBackpackUI()
    {
        UIgo.SetActive(false);
    }
    
    private void InitializeAccessTimer()
    {
        _currentTime = Time.time;
        _requiredTime = Time.time + timeToAccess;
    }

    private void ResetAccess()
    {
        _currentTime = 0;
        _accessEnabled = false;
    }
}
