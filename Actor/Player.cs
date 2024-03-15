using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    internal class Player:Pawn
    {
        public static Random random = new Random();
        FightComponent fightComponent;
        public bool Fight { get; set; }
        public Player(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, hp, position, size, overlap)
        {
            OnOverlap += OverlapWithHuntArae;
            fightComponent = new FightComponent(this, 100, 100, 100);
        }

        private void OverlapWithHuntArae(Actor actor)
        {
            if(actor is HuntArea)
            {
                if(Player.random.Next(100) < 20)
                {
                    Program.gameState = GameState.FIGHT;
                }
            }
        }
        public void StartFight()
        {
            this.Fight = true;
        }
    }
}
