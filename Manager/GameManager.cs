using MyBuffer;
using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class GameManager
    {
        public static List<Actor> AllActors = new List<Actor>();
        public static GameState gameState = GameState.ADVENTURE;
        public static bool UiFocusedBlink = false;
        static int KillCount = 0;

        static Player player;
        static Wall wall;



        public static void Run()
        {
            RunAsyncs();

            player = new Player("siko", 20, Program.SCREEN_CENTER_OFFSET, new Vec2(1, 1), false);
            Wall wall1 = new Wall("wall", new Vec2(20, 20), new Vec2(10, 2), false);
            HuntArea huntarea = new HuntArea("huntarea", new Vec2(100, 33), new Vec2(10, 2));
            Merchant merchant = new Merchant("merchant", new Vec2(80, 33), new Vec2(1, 1), false);


            //game loop
            while (true)
            {
                //win condition
                if (KillCount >= 2)
                {
                    gameState = GameState.WIN;
                }
                switch (gameState)
                {
                    case GameState.ADVENTURE:
                        Adventure();
                        break;
                    case GameState.FIGHT:
                        Fight();
                        break;
                    case GameState.WIN:
                        Win();
                        break;
                    case GameState.PAUSE:
                        Pause();
                        break;
                    case GameState.SHOP:
                        Shop();
                        break;
                }
            }
        }

        private static void Shop()
        {
            UIContainerGridContent MainShopUi = new UIContainerGridContent("MainShop", "Welcom to shop what do you need?", null, 1, 1, true);
            while (true)
            {
                RenderManager.RenderUIContainer(MainShopUi);
                if (InputManager.IsKeyReleased(EInput.ESCAPE))
                {
                    gameState = GameState.ADVENTURE;
                    break;
                }
            }
        }

        public static void StartFight()
        {
            gameState = GameState.FIGHT;
        }

        static void Adventure()
        {
            RenderManager.RenderAllActorRelativeOffset(AllActors, player.GetPosition(), Program.SCREEN_CENTER_OFFSET);
            while (true)
            {
                CharacterInput(player);
                RenderManager.RenderAllActorRelativeOffset(AllActors, player.GetPosition(), Program.SCREEN_CENTER_OFFSET);
                if(gameState != GameState.ADVENTURE)
                {
                    break;
                }
            }
        }
        static void Fight()
        {
            Enemy enemy = new Enemy("KKong", 10);
            enemy.SetOpponent(player);
            UiContainerGrid MainFightUi = new UiContainerGrid("MainFightUi", 2, 1, true);
            var FightSceneUi = new UIContainerGridContent("FightSceneUi", """
                                                                      ...                             
                                                                   /**// (##(&      ****/,            
                                                                       ((((#   (,(##((%/(@            
                                                                      *% ((&  /##((((#&@              
                                                                    % (**,,,/*/(((&                   
                                                          .((.   .(/(//*,,#@&///(*                    
                                                          #**///(////////////////(/                   
                                                            #(((/ .//////////////(@                   
                                                               . //// ,/   /////##@                   
                                                               ////// ,*///, *#/*,,//#@     *///(     
                                                               ,.     (, *((///(((%(###%%   (*,//((#  
                                                                ,/, ...,///##,,*/&#(* *(    ,#(((#*   
                                                                    ((*//##@@@     ##  @@ #%          
                                                                        #(((((                        
                                                                        %@@                           
                   .*(,, */#.       .(/,*,*#.          
                  *///**((%%*     *////**/*/((@        
                (//////(#*      */////////(&          
                (///////(#*      /*///////(#*          
                #///////((#      (////////(%           
                #///////(#.     (///////(#            
                 #////((#(*/,,,*(//////((@            
                 .#(((*///,/*,/*(/////((@             
                  ///**/////////////((((#((           
                 .////////////////////((((#,          
                 .(////////////////////(((((&          
                /#////////////////////(((((((&         
       /*,*////(@(//////////  /*///((((///%#@         
     (/.  .*////(#, ///////    /((((&((///*///#%%#    
     #////,,////(##((,.,,,,#(((,,,,,((@(((/////////((#
""",
                () => { }, 3, 3
                );
            var FightBottomUi = new UiContainerGrid("FightSelectsUi", 1, 3);

            var SelectUi = new UiContainerGrid("Select", 2, 2);
            var FightBottom2 = new UiContainerGrid("FightBottom2", 3, 1);
            var FightLog = new UIContainerGridContent("FightLog", UIFightLogManager.GetContent, null, 3, 1);
            var FightButton1 = new UIContainerGridContent("AttackButton", "Attack", null);
            var FightButton2 = new UIContainerGridContent("SkillButton", "Skillz", null);
            var FightButton3 = new UIContainerGridContent("ItemsButton", "Items", null);
            var FightButton4 = new UIContainerGridContent("Run", "Run", null);
            var StaticPlayer = new UIContainerGridContent("PlayerStatic", () =>
            { return $"{player.GetHp()}"; }
            , null);
            string.Format("str{0}", new object[5] { 1, 2, 3, 4, 5 });

            MainFightUi.AddNewUI(FightSceneUi);
            MainFightUi.AddNewUI(FightBottomUi);
            FightBottomUi.AddNewUI(SelectUi);
            FightBottomUi.AddNewUI(FightBottom2);
            FightBottomUi.AddNewUI(FightLog);
            SelectUi.AddNewUI(FightButton1);
            SelectUi.AddNewUI(FightButton2);
            SelectUi.AddNewUI(FightButton3);
            SelectUi.AddNewUI(FightButton4);
            FightSceneUi.AddNewUI(StaticPlayer);

            var StaticEnemy = new UIContainerGridContent("EnemyStatic", () => { return $"{enemy.GetHp()}"; }, null);
            FightSceneUi.AddNewUI(StaticEnemy);
            FightButton1.AddOnClick(() =>
            {
                UIFightLogManager.Clear();
                //Todo Make ondead dropitem
                //Todo make function for render UI currently it's hard to read
                player.Attack(enemy);
                RenderManager.RenderUIContainer(MainFightUi);
                Thread.Sleep(1000);
                enemy.Attack(player);
                RenderManager.RenderUIContainer(MainFightUi);
                Thread.Sleep(1000);
            });
            Task debugHp = Task.Run(() => { player.Damaged(10); });
            UICursor.InitialCursor(MainFightUi);



            while (true)
            {
                UIInput();
                RenderManager.RenderUIContainer(MainFightUi);
                if (enemy.IsDead())
                {
                    RenderManager.RenderUIContainer(MainFightUi);
                    Thread.Sleep(2000);
                    gameState = GameState.ADVENTURE;
                    KillCount++;
                    break;
                }
            }


        }

        static void Win()
        {
            UiContainerGrid MainWinUi = new UiContainerGrid("MainWinUi", 1, 1, true);
            var UiItemGrid = new UIContainerGridContent("WinUi","You Win!!!",null);
            MainWinUi.AddNewUI(UiItemGrid);
            RenderManager.RenderUIContainer(MainWinUi);
            if(InputManager.IsKeyReleased(EInput.ESCAPE))
            {
                gameState = GameState.ADVENTURE;
            }
        }
        static void Pause()
        {
            UiContainerGrid MainPauseUi = new UiContainerGrid("MainPauseUi", 1, 2, true);
            var UiItemGrid = new UiContainerGrid("UiItemGrid", 10, 6);
            MainPauseUi.AddNewUI(UiItemGrid);

            UICursor.InitialCursor(MainPauseUi);
            RenderManager.RenderUIContainer(MainPauseUi);

            while (true)
            {
                InventoryComponent inventoryComponent = player.GetInventory();
                for (int i = 0; i < 6; i++) 
                {
                    if (inventoryComponent.Items.Count <= i)
                    {
                        break;
                    }
                    UiItemGrid.AddNewUI(new UiItem(inventoryComponent.Items[i].GetItemUiString(), inventoryComponent.Items[i].GetItemUiString(),null));
                }
                RenderManager.RenderUIContainer(MainPauseUi);
                UiItemGrid.Clear();

                //UIInput();
                if (InputManager.IsKeyReleased(EInput.ENTER))
                {
                    gameState = GameState.ADVENTURE;
                    break;
                }
            }
        }

        public static void StartShop()
        {
            gameState = GameState.SHOP;
        }


        static async void RunAsyncs()
        {
            Task.Run(UiFocusBlinkAsync);
        }
        // 깜빡임 지원을 위해
        static Task UiFocusBlinkAsync()
        {
            while (true)
            {
                Thread.Sleep(300);
                UiFocusedBlink = !UiFocusedBlink;
            }
        }
        //https://gist.github.com/Shensd/01342e2f399de4dca2ca87b36059ba0a
        static Task MusicAsync()
        {
            while (true)
            {
                //Console.Beep(659, 125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(523, 125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(784, 125); Thread.Sleep(375); Console.Beep(392, 125); Thread.Sleep(375); Console.Beep(523, 125); Thread.Sleep(250); Console.Beep(392, 125); Thread.Sleep(250); Console.Beep(330, 125); Thread.Sleep(250); Console.Beep(440, 125); Thread.Sleep(125); Console.Beep(494, 125); Thread.Sleep(125); Console.Beep(466, 125); Thread.Sleep(42); Console.Beep(440, 125); Thread.Sleep(125); Console.Beep(392, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(784, 125); Thread.Sleep(125); Console.Beep(880, 125); Thread.Sleep(125); Console.Beep(698, 125); Console.Beep(784, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(587, 125); Console.Beep(494, 125); Thread.Sleep(125); Console.Beep(523, 125); Thread.Sleep(250); Console.Beep(392, 125); Thread.Sleep(250); Console.Beep(330, 125); Thread.Sleep(250); Console.Beep(440, 125); Thread.Sleep(125); Console.Beep(494, 125); Thread.Sleep(125); Console.Beep(466, 125); Thread.Sleep(42); Console.Beep(440, 125); Thread.Sleep(125); Console.Beep(392, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(784, 125); Thread.Sleep(125); Console.Beep(880, 125); Thread.Sleep(125); Console.Beep(698, 125); Console.Beep(784, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(587, 125); Console.Beep(494, 125); Thread.Sleep(375); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(415, 125); Console.Beep(440, 125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(440, 125); Console.Beep(523, 125); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(698, 125); Thread.Sleep(125); Console.Beep(698, 125); Console.Beep(698, 125); Thread.Sleep(625); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(415, 125); Console.Beep(440, 125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(440, 125); Console.Beep(523, 125); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(622, 125); Thread.Sleep(250); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(523, 125); Thread.Sleep(1125); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(415, 125); Console.Beep(440, 125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(440, 125); Console.Beep(523, 125); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(698, 125); Thread.Sleep(125); Console.Beep(698, 125); Console.Beep(698, 125); Thread.Sleep(625); Console.Beep(784, 125); Console.Beep(740, 125); Console.Beep(698, 125); Thread.Sleep(42); Console.Beep(622, 125); Thread.Sleep(125); Console.Beep(659, 125); Thread.Sleep(167); Console.Beep(415, 125); Console.Beep(440, 125); Console.Beep(523, 125); Thread.Sleep(125); Console.Beep(440, 125); Console.Beep(523, 125); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(622, 125); Thread.Sleep(250); Console.Beep(587, 125); Thread.Sleep(250); Console.Beep(523, 125);

                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(293, 200);
                //Console.Beep(246, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(415, 200);
                //Console.Beep(415, 200);
                //Console.Beep(440, 200);
                //Console.Beep(493, 200);
                //Console.Beep(440, 200);
                //Console.Beep(440, 200);
                //Console.Beep(440, 200);
                //Console.Beep(329, 200);
                //Console.Beep(293, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(369, 200);
                //Console.Beep(329, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(293, 200);
                //Console.Beep(246, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(415, 200);
                //Console.Beep(415, 200);
                //Console.Beep(440, 200);
                //Console.Beep(493, 200);
                //Console.Beep(440, 200);
                //Console.Beep(440, 200);
                //Console.Beep(440, 200);
                //Console.Beep(329, 200);
                //Console.Beep(293, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(369, 200);
                //Console.Beep(329, 200);
                //Console.Beep(329, 200);
                //Console.Beep(369, 200);
                //Console.Beep(329, 200);
            }
        }


        public static void UIInput()
        {
            //input
            EInput input = InputManager.GetReleasedInput();
            switch (input)
            {
                case EInput.UP:
                    UICursor.Move(EDirection.UP);
                    break;
                case EInput.DOWN:
                    UICursor.Move(EDirection.DOWN);
                    break;
                case EInput.LEFT:
                    UICursor.Move(EDirection.LEFT);
                    break;
                case EInput.RIGHT:
                    UICursor.Move(EDirection.RIGHT);
                    break;
                case EInput.ENTER:
                    UICursor.Click();
                    break;
                case EInput.ESCAPE:
                    UICursor.Escape();
                    break;
            }
        }
        public static void CharacterInput(Player player)
        {
            //input
            EInput input = InputManager.GetPushedInput();
            switch (input)
            {
                case EInput.UP:
                    player.Move(EDirection.UP);
                    break;
                case EInput.DOWN:
                    player.Move(EDirection.DOWN);
                    break;
                case EInput.LEFT:
                    player.Move(EDirection.LEFT);
                    break;
                case EInput.RIGHT:
                    player.Move(EDirection.RIGHT);
                    break;
                case EInput.ENTER:
                    player.Interact();
                    break;
                case EInput.ESCAPE:
                    GameManager.gameState = GameState.PAUSE;
                    break;
            }
        }
    }
}
