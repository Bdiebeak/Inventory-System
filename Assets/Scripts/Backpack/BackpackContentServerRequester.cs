using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Weapons;

namespace Backpack
{
    /// <summary>
    /// Класс, отвечающий за отправление запросов на сервер.
    /// </summary>
    [RequireComponent(typeof(BackpackContents))]
    public class BackpackContentServerRequester : MonoBehaviour
    {
        private readonly string _address = "https://dev3r02.elysium.today/inventory/status";
        private readonly string _authKey = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";

        private BackpackContents _backpackContents;

        private void Awake()
        {
            _backpackContents = GetComponent<BackpackContents>();
        }

        private void OnEnable()
        {
            _backpackContents.OnWeaponAdd += OnWeaponAddHandler;
            _backpackContents.OnWeaponTake += OnWeaponTakeHandler;
        }

        private void OnDisable()
        {
            _backpackContents.OnWeaponAdd -= OnWeaponAddHandler;
            _backpackContents.OnWeaponTake -= OnWeaponTakeHandler;
        }
    
        private void OnWeaponAddHandler(Weapon weapon)
        {
            var addWeaponForm = CreateForm(weapon.weaponСharacteristic.id, "add");
        
            StartCoroutine(Upload(addWeaponForm));
        }
    
        private void OnWeaponTakeHandler(Weapon weapon)
        {
            var takeWeaponForm = CreateForm(weapon.weaponСharacteristic.id, "take");
        
            StartCoroutine(Upload(takeWeaponForm));
        }

        private WWWForm CreateForm(int weaponID, string eventType)
        {
            var form = new WWWForm();
            form.AddField("weapon_id", weaponID);
            form.AddField("event_type", eventType);

            return form;
        }

        private IEnumerator Upload(WWWForm addedForm)
        {
            var request = UnityWebRequest.Post(_address, addedForm);
            request.SetRequestHeader("Authorization", _authKey);
        
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Upload error.");
            }
        }
    }
}
