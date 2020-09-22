using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        _backpackContents.OnWeaponAdd += WeaponAddHandler;
        _backpackContents.OnWeaponTake += WeaponTakeHandler;
    }

    private void OnDisable()
    {
        _backpackContents.OnWeaponAdd -= WeaponAddHandler;
        _backpackContents.OnWeaponTake -= WeaponTakeHandler;
    }
    
    private void WeaponAddHandler(Weapon weapon)
    {
        WWWForm form = CreateForm(weapon.weaponSettings.id, "add");
        
        StartCoroutine(Upload(form));
    }
    
    private void WeaponTakeHandler(Weapon weapon)
    {
        WWWForm form = CreateForm(weapon.weaponSettings.id, "take");
        
        StartCoroutine(Upload(form));
    }

    private WWWForm CreateForm(int weaponID, string eventType)
    {
        WWWForm form = new WWWForm();
        form.AddField("weapon_id", weaponID);
        form.AddField("event_type", eventType);

        return form;
    }

    private IEnumerator Upload(WWWForm addedForm)
    {
        UnityWebRequest www = UnityWebRequest.Post(_address, addedForm);
        www.SetRequestHeader("auth", _authKey);
        
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Upload error.");
        }
    }
}
