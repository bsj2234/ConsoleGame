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
        private Actor owner;
        private Pawn opponentForItem;
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

        private event Action? OnDelayedDead;
        private event Action? OnAttacked;
        private event Action? OnAttack;

        public FightComponent(Actor owner, int maxHp, int maxMp, int maxSp)
        {
            this.owner = owner;
            this.hp = maxHp;
            this.Mp = maxMp;
            this.Sp = maxSp;
            Dead = false;
            OnDelayedDead += () =>
            {
                if (opponentForItem is Player)
                {
                    UIFightLogManager.Append($"{owner.Name} is killed by {opponentForItem.Name}!\n");
                    GetItem();
                }
            };
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

        public void GetItem()
        {
            GetDropItem();
            foreach (var item in dropItems)
            {
                opponentForItem.AddItem(item);
                UIFightLogManager.Append($" {opponentForItem.Name} got {item.GetItemUiString()}\n");
            }
        }

        public Item GetDropItem()
        { 
            return GetRandomItem();
        }
        public int GetDropGold()
        {
            return GetRandomGold();
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

        public void StartFight(Pawn opponentForItem)
        {
            this.opponentForItem = opponentForItem;
        }

        private Item GetRandomItem()
        {
            List<Item> dropItem = GetDropTable(owner.Name);
            return dropItem[Random.Shared.Next()%dropItem.Count];
        }
        private int GetRandomGold()
        {
            int maxDropGold = GetMaxGold(owner.Name);
            return Random.Shared.Next() % maxDropGold;
        }

        private int GetMaxGold(string name)
        {
            return 50;
        }

        private List<Item> GetDropTable(string name)
        {
            dropItems = new List< Item >();
            dropItems.Add(new ItemConsumable("Apple"));
            dropItems.Add(new ItemConsumable("Pear"));
            dropItems.Add(new ItemConsumable("Melon"));
            dropItems.Add(new ItemConsumable("Cake"));
            dropItems.Add(new ItemConsumable("Bisket"));
            return dropItems;
        }
    }
}
