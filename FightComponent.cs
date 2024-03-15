using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    internal class FightComponent:Component
    {
        private Actor owner;
        private int maxHp;
        private int maxMp;
        private int maxSp;
        public int Hp { get; private set; }
        public int Mp { get; private set; }
        public int Sp { get; private set; }
        public bool Dead { get; private set; }

        private event Action? OnDead;
        private event Action? OnAttacked;
        private event Action? OnAttack;

        public FightComponent(Actor owner, int maxHp, int maxMp, int maxSp) 
        {
            this.owner = owner;
            this.Hp = maxHp;
            this.Mp = maxMp;
            this.Sp = maxSp;
            Dead = false;
        }

        public void Damaged(int damage)
        {
            Hp -= damage;
            if(Hp <= 0)
            {
                if(OnDead != null)
                {
                    OnDead();
                    Dead = true;
                }
            }
        }

        public void Attack(FightComponent other, int damage) 
        {
            other.Damaged(damage);
        }

        public void StartFightSceneRender()
        {
            //MyBuffer.AddNewScene();
            //다른 두 장면을 렌더링할 방법이 필요
            //렌더만의 문제가아니다 또다른 씬이 필요할 것 같다
            //씬이 달라지면 렌더의 방법도 달라진다
            //액터를 모두 렌더한다
            //맵은 어디영역에 그린다
            //등등 아직 할 게 많다.
            //씬은 놔두고 렌더링하고 인풋만
            //교체하는게 이 프로젝트에 알맞겠다
            //너무 어려울거겉아
            //일단 인풋부터 처리해보자
        }
    }
}
