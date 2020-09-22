using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackContents : MonoBehaviour
{
    private Weapon[] _contents;
    
    public event Action<Weapon> OnWeaponAdd;
    public event Action<Weapon> OnWeaponRemove;

    private void Start()
    {
        _contents = new Weapon[Enum.GetNames(typeof(WeaponType)).Length];
    }

    public void TryAddWeapon(Weapon weapon)
    {
        if (_contents[GetWeaponTypeIndex(weapon)] != null)
        {
            // Слот уже занят
            return;
        }
        
        _contents[GetWeaponTypeIndex(weapon)] = weapon;
        
        OnWeaponAdd?.Invoke(weapon);
    }

    public void TryTakeWeapon(Weapon weapon)
    {
        if (_contents[GetWeaponTypeIndex(weapon)] != weapon)
        {
            // В слоте другой объект
            return;
        }

        _contents[GetWeaponTypeIndex(weapon)] = null;
     
        OnWeaponRemove?.Invoke(weapon);
    }

    private int GetWeaponTypeIndex(Weapon weapon)
    {
        return (int) weapon.weaponSettings.type;
    }
}
