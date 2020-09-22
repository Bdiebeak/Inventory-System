using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Класс оружия.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Характеристики оружия.
        /// </summary>
        public WeaponСharacteristics weaponСharacteristic;

        private void Awake()
        {
            if (weaponСharacteristic == null)
            {
                Debug.LogError($"{gameObject.name} не назначены настройки.");
            }
        }
        
        /// <summary>
        /// Получить индекс типа оружия в перечислении.
        /// </summary>
        /// <returns> Индекс типа из общего перечисления. </returns>
        public int GetWeaponTypeIndex()
        {
            return (int) weaponСharacteristic.type;
        }
    }
}
