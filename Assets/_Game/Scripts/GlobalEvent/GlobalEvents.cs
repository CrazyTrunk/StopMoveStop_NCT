using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GlobalEvents
{
    // Sự kiện cho việc chọn vũ khí
    public static event Action<PlayerData> OnWeaponSelected;
    public static event Action<ItemData> OnShopItemClick;
    public static event Action OnXMarkSkinShopClicked;
    public static event Action OnPlayClick;
    public static event Action OnReviveClick;

    // Phương thức để phát sự kiện chọn vũ khí
    public static void WeaponSelected(PlayerData playerData)
    {
        OnWeaponSelected?.Invoke(playerData);
    }
    public static void ShopItemClicked(ItemData itemData)
    {
        OnShopItemClick?.Invoke(itemData);
    }
    public static void OnPlayClicked()
    {
        OnPlayClick?.Invoke();
    }
    public static void OnXMarkSelect()
    {
        OnXMarkSkinShopClicked?.Invoke();
    }
    public static void OnReviveClicked()
    {
        OnReviveClick?.Invoke();
    }
}