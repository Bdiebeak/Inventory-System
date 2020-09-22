using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponSettings weaponSettings;

        private void Awake()
        {
            if (weaponSettings == null)
            {
                Debug.LogError($"{gameObject.name} не назначены настройки.");
            }
        }

        public int GetWeaponTypeIndex()
        {
            return (int) weaponSettings.type;
        }
    }
}
