using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
public class WeaponSettings : ScriptableObject
{
    public new string name;
    public float weight;
    public int id;
    public WeaponType type;
    public Sprite icon;
}
