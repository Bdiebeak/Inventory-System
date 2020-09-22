using System;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Backpack
{
    /// <summary>
    /// Скрипт, отвечающий за рабоут с содержимым рюкзака.
    /// </summary>
    public class BackpackContents : MonoBehaviour
    {
        /// <summary>
        /// Максимальный размер содержимого.
        /// </summary>
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
        
        /// <summary>
        /// Функция доставания оружия из рюкзака.
        /// </summary>
        /// <param name="index"> Индекс оружия. </param>
        public void TryTakeWeapon(int index)
        {
            if (_contents[index] == null)
            {
                // В слоте пусто - ничего не происходит
                return;
            }

            var weapon = _contents[index];

            TryTakeWeapon(weapon);
        }
        
        /// <summary>
        /// Функция доставания оружия из рюкзака.
        /// </summary>
        /// <param name="weapon"> Объект оружия. </param>
        public void TryTakeWeapon(Weapon weapon)
        {
            if (_contents[weapon.GetWeaponTypeIndex()] != weapon)
            {
                // В слоте другой объект - ничего не происходит
                return;
            }

            _contents[weapon.GetWeaponTypeIndex()] = null;
     
            OnWeaponTake?.Invoke(weapon);
            WeaponTakeEvent?.Invoke();
        }
        
        /// <summary>
        /// Функция добавления оружия в рюкзак.
        /// </summary>
        /// <param name="weapon"> Объект оружия. </param>
        public void TryAddWeapon(Weapon weapon)
        {
            if (_contents[weapon.GetWeaponTypeIndex()] != null)
            {
                // Слот уже занят - ничего не происходит
                return;
            }
        
            _contents[weapon.GetWeaponTypeIndex()] = weapon;
        
            OnWeaponAdd?.Invoke(weapon);
            WeaponAddEvent?.Invoke();
        }
    }
}
