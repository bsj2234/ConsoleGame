using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    internal class StatusComponent:Component
    {
        public double Atk {  get; set; }
        public double Hp { get; set; }
        public double Shield { get; set; }
        public int Level { get; set; }
        public double Exp { get; set; }

        public event EventHandler OnLevelUp;

        public StatusComponent(double atk, double hp, double shield)
        {
            Atk = atk;
            Hp = hp;
            Shield = shield;
        }

        public void IncreaseExp(double amount)
        {
            Exp += amount;
            if(Exp > 100.0)
            {
                LevelUp();
                Exp = 0.0;
            }
        }

        private void LevelUp()
        {
            Level++; 
        }
    }
}
