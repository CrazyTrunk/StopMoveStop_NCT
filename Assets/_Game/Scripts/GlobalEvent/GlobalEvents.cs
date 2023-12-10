﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GlobalEvents
{
    // Sự kiện cho việc chọn vũ khí
    public static event Action<WeaponType> OnWeaponSelected;
    public static event Action<ItemData> OnShopItemClick;

    public static event Action OnPlayClick;

    // Phương thức để phát sự kiện chọn vũ khí
    public static void WeaponSelected(WeaponType weaponType)
    {
        OnWeaponSelected?.Invoke(weaponType);
    }
    public static void ShopItemClicked(ItemData itemData)
    {
        OnShopItemClick?.Invoke(itemData);
    }
    public static void OnPlayClicked()
    {
        OnPlayClick?.Invoke();
    }
}