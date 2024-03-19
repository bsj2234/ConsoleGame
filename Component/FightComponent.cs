using MyBuffer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class FightComponent:Component
    {
        private Pawn owner;
        public Pawn opponentForItem;
        private int maxHp;
        private int maxMp;
        private int maxSp;
        protected int hp;
        private List<Item> dropItems;
        private int dropGold;

        public int GetHp()
        {
            return hp;
        }
        public int Mp { get; private set; }
        public int Sp { get; private set; }
        public bool Dead { get; private set; }

        public event Action? OnDelayedDead;
        private event Action? OnAttacked;
        private event Action? OnAttack;

        public FightComponent(Pawn owner, int maxHp, int maxMp, int maxSp)
        {
            this.owner = owner;
            this.hp = maxHp;
            this.maxHp = maxHp;
            this.Mp = maxMp;
            this.maxSp = maxSp;
            this.Sp = maxSp;
            this.maxSp = maxSp;
            Dead = false;

        }

        public void SetOpponent(Pawn opponentForItem) 
        {
            this.opponentForItem = opponentForItem;
        }

        public void Damaged(int damage)
        {
            hp -= damage;
            if(hp <= 0)
            {
                Dead = true;
            }
            ConsoleExtender.PositionConsoleWindowDemo.ShakeWindow(3, 10, 50);
        }

        public bool IsDead()
        {
            if (Dead)
            {
                if (OnDelayedDead != null && Dead == true)
                {
                    OnDelayedDead.Invoke();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Attack(FightComponent other, int damage) 
        {
            other.Damaged(damage);
        }

        public void Heal(int healAmount)
        {
            hp += healAmount;
            if(hp > maxHp)
            {
                hp = maxHp;
            }
        }
    }
}
