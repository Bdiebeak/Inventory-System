using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackpackContents : MonoBehaviour
{
    public readonly int contentsSize = Enum.GetNames(typeof(WeaponType)).Length;
    
    private Weapon[] _contents;
    
    public event Action<Weapon> OnWeaponAdd;
    public UnityEvent WeaponAddEvent;
    public event Action<Weapon> OnWeaponTake;
    public UnityEvent WeaponTakeEvent;

    private void Awake()
    {
        _contents = new Weapon[contentsSize];
    }

    public void TryAddWeapon(Weapon weapon)
    {
        if (_contents[weapon.GetWeaponTypeIndex()] != null)
        {
            // Слот уже занят
            return;
        }
        
        _contents[weapon.GetWeaponTypeIndex()] = weapon;
        
        OnWeaponAdd?.Invoke(weapon);
        WeaponAddEvent?.Invoke();
    }

    public void TryTakeWeapon(Weapon weapon)
    {
        if (_contents[weapon.GetWeaponTypeIndex()] != weapon)
        {
            // В слоте другой объект
            return;
        }

        _contents[weapon.GetWeaponTypeIndex()] = null;
     
        OnWeaponTake?.Invoke(weapon);
        WeaponTakeEvent?.Invoke();
    }

    public void TakeWeapon(int index)
    {
        if (_contents[index] == null)
        {
            // В слоте пусто
            return;
        }

        var weapon = _contents[index];

        TryTakeWeapon(weapon);
    }
}
