using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(BackpackContents))]
public class BackpackDragAndDropControl : MonoBehaviour
{
    private BackpackContents _backpackContents;
    
    private GameObject _currentGameObjectInTrigger;
    private DragAndDropBehaviour _currentDragAndDropBehaviour;
    
    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Start()
    {
        _backpackContents = GetComponent<BackpackContents>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enteringGameObject = other.attachedRigidbody?.gameObject;

        if (enteringGameObject == null ||
            enteringGameObject?.GetComponent<DragAndDropBehaviour>()?.IsDragged == false ||
            enteringGameObject?.GetComponent<Weapon>() == null ||
            _currentGameObjectInTrigger == enteringGameObject)
        {
            return;
        }

        _currentGameObjectInTrigger = enteringGameObject;
        _currentDragAndDropBehaviour = _currentGameObjectInTrigger.GetComponent<DragAndDropBehaviour>();

        _currentDragAndDropBehaviour.OnDrop += AddToBackpack;
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
        _currentDragAndDropBehaviour.OnDrop -= AddToBackpack;
        _currentDragAndDropBehaviour = null;
        _currentGameObjectInTrigger = null;
    }
}
