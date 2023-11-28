
using System;
using UnityEngine;

public class HandWeapon : MonoBehaviour
{
    private GameObject weaponOnHandPrefab;
    [SerializeField]private Transform parentHolder;

    private void Awake()
    {
        InitWeapon();
    }

    private void InitWeapon()
    {
        //GameObject weaponPrefab = weaponManager.LoadCurrentWeapon();

        //weaponOnHandPrefab = Instantiate(weaponPrefab, transform.position, Quaternion.identity, parentHolder);
        //weaponOnHandPrefab.transform.localRotation = Quaternion.Euler(0, 0, -90f);
    }

    public void HideWeapon()
    {
        weaponOnHandPrefab.SetActive(false);
    }
    public void ShowWeapon()
    {
        weaponOnHandPrefab.SetActive(true);
    }
}