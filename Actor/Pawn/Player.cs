using MyBuffer;
using MyData;
using System.Diagnostics;

namespace ConsoleGameProject
{
    public class Player : Pawn
    {
        public static Random random = new Random();
        public PlayerInteractArea InteractArea;
        private PathFindingComponent pathFindingComponent = new PathFindingComponent();
        public bool Fight { get; set; }
        public Player(string name, int hp, Vec2 position, Vec2 size, bool overlap, ECharacterType characterType) : base(name, hp, position, size, overlap, characterType)
        {
            fightComponent = new FightComponent(this, hp, 100);
            //사이즈는 앞뒤 양옆 한줄씩 크게 하기 위해서
            Vec2 interactSize = size + new Vec2(2, 2);
            Vec2 interactCenter = position - interactSize * .5;
            InteractArea = new PlayerInteractArea(this, "InteractArea", interactCenter, interactSize);
            InteractArea.AddIgnoreCollision(this);
            this.AddIgnoreCollision(InteractArea);

            OnOverlap += OverlapWithHuntArae;

            RenderPriority = 999999999;
        }

        private void OverlapWithHuntArae(Actor actor)
        {
            if (actor is HuntArea)
            {
                if (Player.random.Next(100) < 20)
                {
                    GameManager.StartFight();
                }
            }
        }
        public void StartFight()
        {
            this.Fight = true;
        }

        public void Damaged(int v)
        {
            fightComponent.Damaged(v);
        }

        public override bool Move(EDirection direction)
        {
            bool moved = base.Move(direction);
            InteractArea.UpdateInteractableOverlap(GetPosition());
            int direcCount = 0;
            if (InputManager.IsKeyPressed(EInput.LEFT))
                direcCount++;
            if (InputManager.IsKeyPressed(EInput.RIGHT))
                direcCount++;
            if (InputManager.IsKeyPressed(EInput.UP))
                direcCount++;
            if (InputManager.IsKeyPressed(EInput.DOWN))
                direcCount++;
            if(direcCount >= 2)
            {
                Thread.Sleep(10);
            }
            else
            {
                Thread.Sleep(20);
            }
            return moved;
        }

        public bool CheckInteract()
        {
            List<IInteractable> interactables = InteractArea.GetInteractList();
            if (interactables.Count > 0)
            {
                interactables[0].Interact();
                return true;
            }
            return false;
        }

        public void Interact()
        {
            CheckInteract();
        }
        public override string GetFightCharacterArt()
        {
            if (GameManager.UiFocusedBlink)
            {
                return AsciiArts.All[(int)chracterType][2];
            }
            else
            {
                return AsciiArts.All[(int)chracterType][3];
            }
        }

        public void FindPath()
        {

            PosAndPath? posAnd = pathFindingComponent.FindPathBfsWithMap(in GetPosition(),100);
            if(posAnd == null)
                return;
            List<Actor> fastPaths = new List<Actor>();

            foreach (Vec2 pos in posAnd.GetValueOrDefault().Paths)
            {
                var fast = new BlinkableActor(" ", GetPosition() + pos.GetLeftTopCoord(100), new Vec2(1, 1), true);
                fastPaths.Add(fast);
                fast.RenderPriority = 1;
                fast.RenderC = '@';

                RenderManager.CustomRanderActor();
                Thread.Sleep(30);

            }
            pathFindingComponent.DestroyPath();
            foreach (var actor in fastPaths)
            {
                actor.DelayDestroy(3.0);
            }
        }
    }
}
