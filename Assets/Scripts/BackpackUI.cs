using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    [Tooltip("UI, отображающий содержимое рюкзака.")]
    public GameObject contentsUI;
    
    [Tooltip("Image с режимом Filled, отображающее получение доступа к " +
             "содержимому рюкзака при удержании на нем ЛКМ.")]
    public Image accessBar;
    
    [Tooltip("Время, в течении которого нужно удерживать ЛКМ на рюкзаке " +
             "для получения доступа к его содержимому.")]
    public float timeToAccessContents = 0.5f;
    
    private bool _accessEnabled;
    private float _currentTime;
    private float _requiredTime;

    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    private void Start()
    {
        _graphicRaycaster = contentsUI.GetComponent<GraphicRaycaster>();
        if (_graphicRaycaster == null)
        {
            _graphicRaycaster = contentsUI.AddComponent<GraphicRaycaster>();
        }

        _eventSystem = FindObjectOfType<EventSystem>();
    }

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
                accessBar.fillAmount = 1 - (_requiredTime - _currentTime) / timeToAccessContents;
            }
            else
            {
                _accessEnabled = true;
                accessBar.fillAmount = 1f;
            }
        }
    }

    private void OnMouseUp()
    {
        var button = TryFindButton();
        button?.onClick.Invoke();

        HideBackpackUI();
        
        ResetAccess();
        accessBar.fillAmount = 0f;
    }
    private void ShowBackpackUI()
    {
        contentsUI.SetActive(true);
    }
    
    private Button TryFindButton()
    {
        var pointerEventData = new PointerEventData(_eventSystem) {position = Input.mousePosition};
        var raycastResults = new List<RaycastResult>();

        _graphicRaycaster.Raycast(pointerEventData, raycastResults);

        return raycastResults.Select(result => result.gameObject.GetComponent<Button>())
                             .FirstOrDefault(button => button != null);
    }
    
    private void HideBackpackUI()
    {
        contentsUI.SetActive(false);
    }
    
    private void InitializeAccessTimer()
    {
        _currentTime = Time.time;
        _requiredTime = Time.time + timeToAccessContents;
    }

    private void ResetAccess()
    {
        _currentTime = 0;
        _accessEnabled = false;
    }
}
