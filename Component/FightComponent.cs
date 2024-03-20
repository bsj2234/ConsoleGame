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
        protected int hp;
        protected int mp;

        public int GetHp()
        {
            return hp;
        }
        public int Mp { get; private set; }
        public bool Dead { get; private set; }



        public event Action? OnDelayedDead;
        private event Action? OnAttacked;
        private event Action? OnAttack;

        public FightComponent(Pawn owner, int maxHp = 100, int maxMp = 0 )
        {
            this.owner = owner;
            this.hp = maxHp;
            this.maxHp = maxHp;
            this.Mp = maxMp;
            this.maxMp = maxMp;
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

        public string GetUiStatus()
        {
            return $"Name: {owner.Name} \nHP: {hp} / {maxHp} \nMP: {mp} / {maxMp} \n";
        }
    }
}
