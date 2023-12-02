using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GlobalEvents
{
    // Sự kiện cho việc chọn vũ khí
    public static event Action<WeaponType> OnWeaponSelected;

    // Phương thức để phát sự kiện chọn vũ khí
    public static void WeaponSelected(WeaponType weaponType)
    {
        OnWeaponSelected?.Invoke(weaponType);
    }
}