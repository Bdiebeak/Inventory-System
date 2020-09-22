using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Backpack
{
    /// <summary>
    /// Скрипт, отвечающий за приснапливание объектов к своему месту на рюкзаке.
    /// </summary>
    [RequireComponent(typeof(BackpackContents))]
    public class BackpackContentsSnapper : MonoBehaviour
    {
        [Tooltip("Скорость, с которой объект достигает своего места.")]
        [SerializeField] 
        private float snapSpeed = 3;
    
        [Tooltip("Объекты-родители, для снапинга объектов.")]
        [SerializeField]
        private Transform[] snapPoints;

        private Weapon[] _weapons;
        private BackpackContents _backpackContents;
    
        private void OnValidate()
        {
            ResizeArray();
        }

        private void Reset()
        {
            ResizeArray();
        }

        private void ResizeArray()
        {
            int size = GetComponent<BackpackContents>().contentsSize;
            Array.Resize(ref snapPoints, size);
        }

        private void Awake()
        {
            _backpackContents = GetComponent<BackpackContents>();
            _weapons = new Weapon[_backpackContents.contentsSize];
        }

        private void OnEnable()
        {
            _backpackContents.OnWeaponAdd += OnWeaponAddHandler;
            _backpackContents.OnWeaponTake += OnWeaponTakeHandler;
        }

        private void OnDisable()
        {
            _backpackContents.OnWeaponAdd -= OnWeaponAddHandler;
            _backpackContents.OnWeaponTake -= OnWeaponTakeHandler;
        }

        private void OnWeaponAddHandler(Weapon weapon)
        {
            SnapWeapon(weapon);
        }

        private void OnWeaponTakeHandler(Weapon weapon)
        {
            UnsnapWeapon(weapon);
        }
        
        private void SnapWeapon(Weapon weapon)
        {
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            _weapons[weapon.GetWeaponTypeIndex()] = weapon;

            var weaponTransform = weapon.transform;
            StartCoroutine(SmoothlyZeroizeTransformCoroutine(weaponTransform, snapPoints[weapon.GetWeaponTypeIndex()]));
        }

        private void UnsnapWeapon(Weapon weapon)
        {
            if (_weapons[weapon.GetWeaponTypeIndex()] != weapon)
            {
                // В слоте другой объект - ничего не происходит
                return;
            }

            _weapons[weapon.GetWeaponTypeIndex()].gameObject.GetComponent<Rigidbody>().isKinematic = false;
            _weapons[weapon.GetWeaponTypeIndex()].transform.parent = null;
            _weapons[weapon.GetWeaponTypeIndex()] = null;
        }

        private IEnumerator SmoothlyZeroizeTransformCoroutine(Transform objectTransform, Transform newParent)
        {
            objectTransform.parent = newParent;
            
            while (objectTransform.parent == newParent && 
                   (Vector3.Distance(objectTransform.localPosition, Vector3.zero) > 0.05f ||
                    Quaternion.Angle(objectTransform.localRotation, Quaternion.identity) > 0.01f)) 
            {
                objectTransform.localPosition = Vector3.Lerp(objectTransform.localPosition, Vector3.zero, snapSpeed * Time.deltaTime);
                objectTransform.localRotation = Quaternion.Lerp(objectTransform.localRotation, Quaternion.identity, snapSpeed * Time.deltaTime);
            
                yield return null;
            }

            if (objectTransform.parent != newParent) yield break;
        
            objectTransform.localPosition = Vector3.zero;
            objectTransform.localRotation = Quaternion.identity;
        }
    }
}
