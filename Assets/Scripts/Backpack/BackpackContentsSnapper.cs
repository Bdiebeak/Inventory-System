using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BackpackContents))]
public class BackpackContentsSnapper : MonoBehaviour
{
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
        _backpackContents.OnWeaponRemove += UnsnapWeapon;
    }

    private void OnDisable()
    {
        _backpackContents.OnWeaponAdd -= SnapWeapon;
        _backpackContents.OnWeaponRemove -= UnsnapWeapon;
    }

    private void SnapWeapon(Weapon weapon)
    {
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        
        var weaponTransform = weapon.transform;
        
        weaponTransform.parent = snapPoints[weapon.GetWeaponTypeIndex()];
        _weapons[weapon.GetWeaponTypeIndex()] = weapon;
        
        weaponTransform.localPosition = Vector3.zero;
        weaponTransform.localEulerAngles = Vector3.zero;
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
