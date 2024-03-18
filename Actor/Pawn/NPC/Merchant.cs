using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class Merchant:Actor, IInteractable
    {
        public Merchant(string name, Vec2 position, Vec2 size, bool overlap) : base(name, position, size, overlap)
        {

        }
        public void Interact()
        {
            GameManager.StartShop();
        }
    }
}
