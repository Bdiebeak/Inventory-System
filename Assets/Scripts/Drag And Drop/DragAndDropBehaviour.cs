using System;
using UnityEngine;

namespace DragAndDrop
{
    /// <summary>
    /// Класс, реализующий перетаскивание Rigidbody объектов.
    /// При перетаскивании задается фиксированная высота по координате У,
    /// к которой плавно поднимается объект.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class DragAndDropBehaviour : MonoBehaviour
    {
        [SerializeField]
        private DragAndDropSettings settings = null;
    
        public bool IsDragged { get; private set; }
        public event Action OnDrop; 

        private float _risingStep = 0;

        private Camera _mainCamera;
        private Rigidbody _rigidbody;

        private void Awake()
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
            IsDragged = true;
        }

        private void OnMouseDrag()
        {
            if (_rigidbody.isKinematic == false)
            {
                TurnOnKinematic();
            }
        
            var newPosition = CalculateNewObjectPosition();
            SmoothDrag(newPosition);
        }

        private void OnMouseUp()
        {
            TurnOffKinematic();
            ResetStep();
        
            IsDragged = false;
        
            OnDrop?.Invoke();
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
            newWorldPosition.y = settings.yDraggingHeight;
        
            return newWorldPosition;
        }

        private void SmoothDrag(Vector3 position)
        {
            transform.position = Vector3.Lerp(transform.position, position, _risingStep);
            IncreaseStep();
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
}