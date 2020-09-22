using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponSettings weaponSettings;

    private void Start()
    {
        if (weaponSettings == null)
        {
            Debug.LogError($"{gameObject.name} не назначены настройки.");
        }
    }
}
