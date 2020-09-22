using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BackpackContents))]
public class BackpackContentsSnapper : MonoBehaviour
{
    [SerializeField] 
    private float snapSpeed = 3;
    
    [SerializeField]
    private Transform[] snapPoints;

    private Weapon[] _weapons;

    private BackpackContents _backpackContents;

    private void Awake()
    {
        _backpackContents = GetComponent<BackpackContents>();
        _weapons = new Weapon[_backpackContents.contentsSize];
    }

    private void OnEnable()
    {
        _backpackContents.OnWeaponAdd += SnapWeapon;
        _backpackContents.OnWeaponTake += UnsnapWeapon;
    }

    private void OnDisable()
    {
        _backpackContents.OnWeaponAdd -= SnapWeapon;
        _backpackContents.OnWeaponTake -= UnsnapWeapon;
    }

    private void SnapWeapon(Weapon weapon)
    {
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        
        var weaponTransform = weapon.transform;
        
        _weapons[weapon.GetWeaponTypeIndex()] = weapon;

        StartCoroutine(SmoothZeroizeTransformCoroutine(weaponTransform, snapPoints[weapon.GetWeaponTypeIndex()]));
    }

    private IEnumerator SmoothZeroizeTransformCoroutine(Transform objectTransform, Transform newParent)
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

    private void UnsnapWeapon(Weapon weapon)
    {
        if (_weapons[weapon.GetWeaponTypeIndex()] != weapon)
        {
            // В слоте другой объект
            return;
        }

        _weapons[weapon.GetWeaponTypeIndex()].transform.parent = null;
        _weapons[weapon.GetWeaponTypeIndex()] = null;
    }

    public void UnsnapWeapon(int index)
    {
        if (_weapons[index] == null)
        {
            // В слоте пусто
            return;
        }

        _weapons[index].gameObject.GetComponent<Rigidbody>().isKinematic = false;
        _backpackContents.TakeWeapon(index);
    }

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
}
