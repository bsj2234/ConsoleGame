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
        static void Adventure()
        {
            //
            Wall wall1 = new Wall("wall", new Vec2(20, 20), new Vec2(10, 2), false);
            HuntArea huntarea = new HuntArea("huntarea", new Vec2(100, 33), new Vec2(10, 2));
            Merchant merchant = new Merchant("merchant", new Vec2(80, 33), new Vec2(1, 1), false);
            while (true)
            {
                CharacterAdventureInput(player);
                RenderManager.RenderAllActorRelativeOffset(AllActors, player.GetPosition(), Program.SCREEN_CENTER_OFFSET);
                InputManager.UpdateKeys();
                if (gameState != GameState.ADVENTURE)
                {
                    break;
                }
            }
        }
        static void Fight()
        {
            Enemy enemy = new Enemy("KKong", 10);
            enemy.SetOpponent(player);
            UiContainerGrid MainFightUi = InitFightUi(enemy);

            while (true)
            {
                UIInput();
                RenderManager.RenderUIContainer(MainFightUi);
                InputManager.UpdateKeys();
                if (enemy.IsDead())
                {
                    RenderManager.RenderUIContainer(MainFightUi);
                    Thread.Sleep(2000);
                    gameState = GameState.ADVENTURE;
                    KillCount++;
                    break;
                }
                if (gameState != GameState.FIGHT)
                {
                    break;
                }
            }
        }

        private static UiContainerGrid InitFightUi(Enemy enemy)
        {
            UiContainerGrid MainFightUi = new UiContainerGrid("MainFightUi", 2, 1, true);
            MainFightUi.SetRowRatio(new double[]{ 2,1});

            var FightTopUi = new UiContainerGrid("FightTopUi", 1, 2);
            FightTopUi.SetColRatio(new double[] { 1, 2 });

            var FightStaticUi = new UiContainerGrid("FightStaticUi", 2, 1);

            var StaticPlayer = new UIContainerGridContent("PlayerStatic", () => { return $"{player.GetHp()}"; }, null);
            var StaticEnemy = new UIContainerGridContent("EnemyStatic", () => { return $"{enemy.GetHp()}"; }, null);
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
                () => { }, 1, 1
                );
            var FightBottomUi = new UiContainerGrid("FightSelectsUi", 1, 3);
            var SelectUi = new UiContainerGrid("Select", 2, 2);
            var FightBottom2 = new UiContainerGrid("FightBottom2", 3, 1);
            var FightLog = new UIContainerGridContent("FightLog", UIFightLogManager.GetContent, null, 3, 1);
            var FightButton1 = new UIContainerGridContent("AttackButton", "Attack", null);
            var FightButton2 = new UIContainerGridContent("SkillButton", "Skillz", null);
            var FightButton3 = new UIContainerGridContent("ItemsButton", "Items", null);
            var FightButton4 = new UIContainerGridContent("Run", "Run", null);

            MainFightUi.AddNewUI(FightTopUi, 0);
            MainFightUi.AddNewUI(FightBottomUi, 0);

            FightTopUi.AddNewUI(FightStaticUi, 0);
            FightTopUi.AddNewUI(FightSceneUi, 1);

            FightStaticUi.AddNewUI(StaticPlayer, 0);
            FightStaticUi.AddNewUI(StaticEnemy, 1);

            FightBottomUi.AddNewUI(SelectUi, 0);
            FightBottomUi.AddNewUI(FightBottom2, 1);
            FightBottomUi.AddNewUI(FightLog, 2);

            SelectUi.AddNewUI(FightButton1, 0);
            SelectUi.AddNewUI(FightButton2, 1);
            SelectUi.AddNewUI(FightButton3, 2);
            SelectUi.AddNewUI(FightButton4, 3);

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
            UICursor.InitialCursor(MainFightUi);
            return MainFightUi;
        }

        static void Win()
        {
            UiContainerGrid MainWinUi = new UiContainerGrid("MainWinUi", 1, 1, true);
            var UiItemGrid = new UIContainerGridContent("WinUi", "You Win!!!", null);
            MainWinUi.AddNewUI(UiItemGrid, 0);
            UICursor.InitialCursor(MainWinUi);
            while (true)
            {
                UIInput();
                RenderManager.RenderUIContainer(MainWinUi);
                InputManager.UpdateKeys();
                if (gameState != GameState.WIN)
                {
                    break;
                }
            }
        }
        static void Pause()
        {
            UiContainerGrid MainPauseUi = new UiContainerGrid("MainPauseUi", 1, 2, true);
            var UiItemGrid = new UiContainerGrid("UiItemGrid", 10, 6);
            MainPauseUi.AddNewUI(UiItemGrid, 0);

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
                    UiItemGrid.AddNewUI(new UiItem(inventoryComponent.Items[i].GetItemUiString(), inventoryComponent.Items[i].GetItemUiString(), null), 1);
                }
                UIInput();
                UiItemGrid.Clear();
                RenderManager.RenderUIContainer(MainPauseUi);
                InputManager.UpdateKeys();
                if (gameState != GameState.PAUSE)
                {
                    break;
                }
            }
        }
        private static void Shop()
        {
            UIContainerGridContent MainShopUi = InitShopUi();
            while (true)
            {
                UIInput();
                RenderManager.RenderUIContainer(MainShopUi);
                InputManager.UpdateKeys();
                if (gameState != GameState.SHOP)
                {
                    break;
                }
            }
        }

        public static void StartFight()
        {
            gameState = GameState.FIGHT;
        }
        public static void StartShop()
        {
            gameState = GameState.SHOP;
        }
        static async void RunAsyncs()
        {
            Task.Run(UiFocusBlinkAsync);
        }



        //input funcs
        public static void UIInput()
        {
            //input
            if (InputManager.IsKeyReleased(EInput.UP))
                UICursor.Move(EDirection.UP);
            if (InputManager.IsKeyReleased(EInput.DOWN))
                UICursor.Move(EDirection.DOWN);
            if (InputManager.IsKeyReleased(EInput.LEFT))
                UICursor.Move(EDirection.LEFT);
            if (InputManager.IsKeyReleased(EInput.RIGHT))
                UICursor.Move(EDirection.RIGHT);
            if (InputManager.IsKeyReleased(EInput.ENTER))
                UICursor.Click();
            if (InputManager.IsKeyReleased(EInput.ESCAPE))
                UICursor.Escape();
        }
        public static void CharacterAdventureInput(Player player)
        {
            //input
            if (InputManager.IsKeyPressed(EInput.UP))
                player.Move(EDirection.UP);
            if (InputManager.IsKeyPressed(EInput.DOWN))
                player.Move(EDirection.DOWN);
            if (InputManager.IsKeyPressed(EInput.LEFT))
                player.Move(EDirection.LEFT);
            if (InputManager.IsKeyPressed(EInput.RIGHT))
                player.Move(EDirection.RIGHT);
            if (InputManager.IsKeyReleased(EInput.ENTER))
                player.Interact();
            if (InputManager.IsKeyReleased(EInput.ESCAPE))
                GameManager.gameState = GameState.PAUSE;
        }



        //async funcs
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




        //DrawUi
        private static UIContainerGridContent InitShopUi()
        {
            UIContainerGridContent MainShopUi = new UIContainerGridContent("MainShop", "Welcom to shop what do you need?", null, 1, 1, true);
            UIContainerGridContent SlectUi = new UIContainerGridContent("MainShop", "Welcom to shop what do you need?", null, 2, 1, true);
            MainShopUi.AddNewUI(SlectUi, 0);
            UICursor.InitialCursor(MainShopUi);
            return MainShopUi;
        }
    }
}
