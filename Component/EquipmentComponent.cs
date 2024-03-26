using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class EquipmentComponent
    {
        ItemWeapon? ItemWeapon { get; set; }

        public event EventHandler OnEquiep;
        public bool Equipable()
        {
            if (ItemWeapon == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Item? Equip()
        {
            return ItemWeapon;
        }
    }
}
