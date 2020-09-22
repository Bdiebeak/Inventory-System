using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Scriptable Object характеристик для оружий.
    /// </summary>
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
    public class WeaponСharacteristics : ScriptableObject
    {
        public new string name;
        public float weight;
        public int id;
        public WeaponType type;
        public Sprite icon;
    }
}
