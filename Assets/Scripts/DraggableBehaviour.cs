using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Класс, реализующий перетаскивание Rigidbody объектов.
/// При перетаскивании задается фиксированная высота по координате У,
/// к которой плавно поднимается объект.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DraggableBehaviour : MonoBehaviour
{
    [Header("\tНастройки")] 
    [SerializeField]
    private DraggableBehaviourSettings settings = null;

    private float _risingStep = 0;

    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError($"{gameObject.name} не назначены настройки.");
        }
        
        _mainCamera = Camera.main;
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        TurnOnKinematic();
    }

    private void OnMouseDrag()
    {
        var newPosition = CalculateNewObjectPosition();
        
        transform.position = Vector3.Lerp(transform.position, newPosition, _risingStep);
        IncreaseStep();
    }

    private void OnMouseUp()
    {
        TurnOffKinematic();
        ResetStep();
    }

    private void TurnOnKinematic()
    {
        _rigidbody.isKinematic = true;
    }
    
    private Vector3 CalculateNewObjectPosition()
    {
        var objectScreenPositionZ = _mainCamera.WorldToScreenPoint(transform.position).z;
        var newScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectScreenPositionZ);
        var newWorldPosition = _mainCamera.ScreenToWorldPoint(newScreenPosition);
        newWorldPosition.y = settings.yDraggingValue;
        
        return newWorldPosition;
    }

    private void IncreaseStep()
    {
        _risingStep += settings.risingSpeed * Time.deltaTime;
    }

    private void TurnOffKinematic()
    {
        _rigidbody.isKinematic = false;
    }

    private void ResetStep()
    {
        _risingStep = 0;
    }
}