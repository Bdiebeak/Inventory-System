using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BackpackContents),typeof(BoxCollider))]
public class BackpackDragAndDropContents : MonoBehaviour
{
    private BackpackContents _backpackContents;
    
    private GameObject _currentGameObjectInTrigger;
    private DragAndDropBehaviour _currentDragAndDropBehaviour;
    private bool _subscribed = false;
    
    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Awake()
    {
        _backpackContents = GetComponent<BackpackContents>();
    }

    private void OnTriggerStay(Collider other)
    {
        var stayingGameObject = other.attachedRigidbody?.gameObject;
        
        if (stayingGameObject == null ||
            stayingGameObject?.GetComponent<DragAndDropBehaviour>()?.IsDragged == false ||
            stayingGameObject?.GetComponent<Weapon>() == null ||
            _currentGameObjectInTrigger == stayingGameObject)
        {
            return;
        }

        ResetCurrentObject();
        
        _currentGameObjectInTrigger = stayingGameObject;
        _currentDragAndDropBehaviour = _currentGameObjectInTrigger.GetComponent<DragAndDropBehaviour>();
        
        TakeFromBackpack();
        
        if (_subscribed)
        {
            return;
        }
        
        _currentDragAndDropBehaviour.OnDrop += AddToBackpack;
        _subscribed = true;
    }

    private void AddToBackpack()
    {
        _backpackContents.TryAddWeapon(_currentGameObjectInTrigger.GetComponent<Weapon>());

        ResetCurrentObject();
    }

    private void TakeFromBackpack()
    {
        _backpackContents.TryTakeWeapon(_currentGameObjectInTrigger.GetComponent<Weapon>());
    }

    private void OnTriggerExit(Collider other)
    {
        var exitingGameObject = other.attachedRigidbody?.gameObject;
        if ((exitingGameObject == null) || (_currentGameObjectInTrigger != exitingGameObject))
        {
            return;
        }

        TakeFromBackpack();
        ResetCurrentObject();
    }

    private void ResetCurrentObject()
    {
        if (_currentDragAndDropBehaviour != null)
        {
            _currentDragAndDropBehaviour.OnDrop -= AddToBackpack;
        }
        _subscribed = false;
        _currentDragAndDropBehaviour = null;
        _currentGameObjectInTrigger = null;
    }
}
