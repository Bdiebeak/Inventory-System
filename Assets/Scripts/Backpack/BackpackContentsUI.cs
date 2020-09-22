using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(BackpackContents))]
public class BackpackContentsUI : MonoBehaviour
{
    [Tooltip("UI, отображающий содержимое рюкзака.")]
    [SerializeField]
    private GameObject contentsUI = null;

    [Header("Параметры отображения содержимого")]
    [Tooltip("Список UI-Image для отображения иконок объектов.")]
    [SerializeField]
    private Image[] contentsImages;

    [Tooltip("Иконка, которая будет отображаться в качестве пустого слота оружия.")]
    [SerializeField] 
    private Sprite emptyIcon = null;
    
    [Header("Параметры активации содержимого")]
    [Tooltip("Image с режимом Filled, отображающее получение доступа к " +
             "содержимому рюкзака при удержании на нем ЛКМ.")]
    [SerializeField]
    private Image accessBar = null;
    
    [Tooltip("Время, в течении которого нужно удерживать ЛКМ на рюкзаке " +
             "для получения доступа к его содержимому.")]
    [SerializeField]
    private float timeToAccessContents = 0.5f;
    
    private bool _accessEnabled;
    private float _currentTime;
    private float _requiredTime;

    private BackpackContents _backpackContents;

    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    private void Awake()
    {
        _backpackContents = GetComponent<BackpackContents>();
        
        _graphicRaycaster = contentsUI.GetComponent<GraphicRaycaster>();
        if (_graphicRaycaster == null)
        {
            _graphicRaycaster = contentsUI.AddComponent<GraphicRaycaster>();
        }
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    
    private void OnEnable()
    {
        _backpackContents.OnWeaponAdd += WeaponAddHandler;
        _backpackContents.OnWeaponTake += WeaponTakeHandler;
    }

    private void OnDisable()
    {
        _backpackContents.OnWeaponAdd -= WeaponAddHandler;
        _backpackContents.OnWeaponTake -= WeaponTakeHandler;
    }

    private void WeaponAddHandler(Weapon weapon)
    {
        contentsImages[weapon.GetWeaponTypeIndex()].sprite = weapon.weaponSettings.icon;
    }
    
    private void WeaponTakeHandler(Weapon weapon)
    {
        contentsImages[weapon.GetWeaponTypeIndex()].sprite = emptyIcon;
    }

    private void OnValidate()
    {
        Array.Resize(ref contentsImages, GetComponent<BackpackContents>().contentsSize);
    }
    
    private void Reset()
    {
        Array.Resize(ref contentsImages, GetComponent<BackpackContents>().contentsSize);
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

        if (button != null)
        {
            button.onClick.Invoke();
            
        }

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
