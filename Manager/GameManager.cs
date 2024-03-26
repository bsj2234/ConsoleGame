using MyBuffer;
using MyData;
using System.Diagnostics;

namespace ConsoleGameProject
{
    public static class GameManager
    {
        //
        public static List<Actor> AllActors = new List<Actor>();
        public static GameState gameState = GameState.ADVENTURE;
        public static bool UiFocusedBlink = false;
        static int KillCount = 0;
        public static double DeltaTime = 0.0;

        public static Player player = new Player("siko", 30, new Vec2(61, 48), new Vec2(1, 1), false, ECharacterType.PIKA);
        static Merchant merchant = new Merchant("merchant", new Vec2(80, 33), new Vec2(1, 1), false);



        public static void Run()
        {
            RunAsyncs();
            MapGenerator.DrawMapOne();
            //Initial item for debug
            player.GetInventory().Add(new ItemConsumable(player.GetInventory(), "Ap1"));
            player.GetInventory().Add(new ItemConsumable(player.GetInventory(), "Ap2"));
            player.GetInventory().Add(new ItemConsumable(player.GetInventory(), "Ap3"));

            Stopwatch stopwatch = Stopwatch.StartNew();
            DeltaTime = stopwatch.Elapsed.TotalSeconds;
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
                DeltaTime = stopwatch.Elapsed.TotalSeconds - DeltaTime;
            }
        }
        static void Adventure()
        {
            //
            new Wall("wall3", new Vec2(20, 30), new Vec2(20, 2), false);
            new Wall("wall4", new Vec2(36, 20), new Vec2(4, 10), false);

            new Wall("wall", new Vec2(20, 20), new Vec2(20, 2), false);

            new Goal("CustomGoal1", new Vec2(25, 18), new Vec2(1, 1));

            HuntArea huntarea = new HuntArea("huntarea", new Vec2(100, 33), new Vec2(10, 2));
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


            //Todo Make multiple enemy and make it random
            Enemy enemy = new Enemy("KKong", 10, (ECharacterType)(Random.Shared.Next() % Enum.GetValues<ECharacterType>().Length));
            enemy.SetOpponent(player);



            UiContainerGrid MainFightUi = new UiContainerGrid("MainFightUi", 2, 1, true);
            MainFightUi.SetRowRatio(new double[] { 2, 1 });

            var FightTopUi = new UiContainerGrid("FightTopUi", 1, 2);
            FightTopUi.SetColRatio(new double[] { 1, 2 });

            var FightStaticUi = new UiContainerGrid("FightStaticUi", 2, 1);
            var FightSceneUi = new UiContainerGrid("FightSceneUi", 2, 2);
            FightSceneUi.SetRowRatio(new double[] { .5, 1 });
            var FightEnemyArt = new UiContainerGridContent("FightEnemyArt", enemy.GetFightCharacterArt, null);
            FightEnemyArt.DrawBorder = false;
            var FightPlayerArt = new UiContainerGridContent("FightPlayerArt", player.GetFightCharacterArt, null);
            FightPlayerArt.DrawBorder = false;
            var FightBottomUi = new UiContainerGrid("FightSelectsUi", 1, 3);
            var SelectUi = new UiContainerGrid("Select", 2, 2);
            var FightBottom2 = new UiContainerGrid("FightBottom2", 1, 1);
            var FightLog = new UiContainerGridContent("FightLog", UiFightLogManager.GetContent, null, 3, 1);
            var AttackButton = new UiContainerGridContent("AttackButton", "Attack", null);
            var SkillButton = new UiContainerGridContent("SkillButton", "Skillz", null);
            var ItemButton = new UiContainerGridContent("ItemsButton", "Items", null);
            var RunButton = new UiContainerGridContent("Run", "Run", null);

            MainFightUi.AddNewUI(FightTopUi, 0);
            MainFightUi.AddNewUI(FightBottomUi, 0);

            FightTopUi.AddNewUI(FightStaticUi, 0);
            FightTopUi.AddNewUI(FightSceneUi, 1);

            FightSceneUi.AddNewUI(null, 0);
            FightSceneUi.AddNewUI(FightEnemyArt, 1);
            FightSceneUi.AddNewUI(FightPlayerArt, 2);

            FightStaticUi.AddNewUI(new UiPawnStatus("PlayerStatic", player.GetFightComponent().GetUiStatus, null), 0);
            FightStaticUi.AddNewUI(new UiPawnStatus("EnemyStatus", enemy.GetFightComponent().GetUiStatus, null), 1);

            FightBottomUi.AddNewUI(SelectUi, 0);
            FightBottomUi.AddNewUI(FightBottom2, 1);
            FightBottomUi.AddNewUI(FightLog, 2);

            SelectUi.AddNewUI(AttackButton, 0);
            SelectUi.AddNewUI(SkillButton, 1);
            SelectUi.AddNewUI(ItemButton, 2);
            SelectUi.AddNewUI(RunButton, 3);


            UiInventoryContainer inventoryUi = new UiInventoryContainer("playerInventory", 2, 3, player.GetInventory());


            //OnAttackButton
            //attack
            AttackButton.AddEvenetOnClick((object s, EventArgs args) =>
            {
                UiFightLogManager.Clear();
                player.Attack(enemy);
                RenderManager.RenderUIContainer(MainFightUi);
                Thread.Sleep(1000);
                enemy.Attack(player);
                RenderManager.RenderUIContainer(MainFightUi);
                Thread.Sleep(1000);
            });
            //OnSkillButton
            SkillButton.AddEvenetOnClick((object s, EventArgs args) =>
            {

            });
            //OnItemButton
            //MoveToItemSelect
            ItemButton.AddEvenetOnClick((object s, EventArgs args) =>
            {

                FightBottom2.AddNewUI(inventoryUi, 0);
                inventoryUi.RefreshItems();
                UiCursor.FocusTo(FightBottom2);
            });
            //OnRunButton
            //Try run back to adventure
            RunButton.AddEvenetOnClick((object s, EventArgs args) =>
            {
                UiFightLogManager.Clear();
                UiFightLogManager.Append("Try to run!\n");
                RenderManager.RenderUIContainer(MainFightUi);
                Thread.Sleep(1000);
                if (Random.Shared.Next() % 10 < 3)
                {
                    UiFightLogManager.Append("Succeed!\n");
                    RenderManager.RenderUIContainer(MainFightUi);
                    Thread.Sleep(1000);
                    UiCursor.ReturnToAdventure();
                }
                else
                {
                    UiFightLogManager.Append("Failed!\n");
                    RenderManager.RenderUIContainer(MainFightUi);
                    UiFightLogManager.Append("Enemy attack!\n");
                    enemy.Attack(player);
                    RenderManager.RenderUIContainer(MainFightUi);
                    Thread.Sleep(1000);
                }
            });
            UiManager.SetMain(MainFightUi);
            UiCursor.InitialCursor(MainFightUi);








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
                    UiFightLogManager.Clear();
                    UiCursor.ReturnToAdventure();
                    break;
                }
                if (gameState != GameState.FIGHT)
                {
                    UiFightLogManager.Clear();
                    break;
                }
            }
        }
        static void Win()
        {
            UiContainerGrid MainWinUi = new UiContainerGrid("MainWinUi", 1, 1, true);
            UiManager.SetMain(MainWinUi);
            var UiItemGrid = new UiContainerGridContent("WinUi", "You Win!!!", null);
            MainWinUi.AddNewUI(UiItemGrid, 0);
            UiCursor.InitialCursor(MainWinUi);
            while (true)
            {
                UIInput();
                RenderManager.RenderUIContainer(MainWinUi);
                InputManager.UpdateKeys();
                if (gameState != GameState.WIN)
                {
                    KillCount = 0;
                    break;
                }
            }
        }
        static void Pause()
        {
            UiContainerGrid MainPauseUi = new UiContainerGrid("MainPauseUi", 1, 3, true);
            UiManager.SetMain(MainPauseUi);
            MainPauseUi.SetColRatio(new double[] { .3, .7, .3 });

            var UiLeftSideBar = new UiContainerGrid("UiLeftSideBar", 2, 1);
            UiLeftSideBar.SetColRatio(new double[] { .3, .7 });
            MainPauseUi.AddNewUI(UiLeftSideBar, 0);

            UiLeftSideBar.AddNewUI(new UiPawnStatus("PlayerStatic", player.GetFightComponent().GetUiStatus, null), 0);


            var SelectsUi = new UiContainerGrid("SelectsUi", 2, 1);
            UiLeftSideBar.AddNewUI(SelectsUi, 1);

            var UiItemGrid = new UiInventoryContainer("UiItemGrid", 10, 6, player.GetInventory());
            MainPauseUi.AddNewUI(UiItemGrid, 1);

            //Todo EquipmentUi

            UiCursor.InitialCursor(MainPauseUi);
            RenderManager.RenderUIContainer(MainPauseUi);

            InventoryComponent inventoryComponent = player.GetInventory();
            //UiItemGrid.Clear();
            while (true)
            {
                UiItemGrid.RefreshItems();
                UIInput();

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
            UiContainerGrid MainShopUi = new UiContainerGrid("MainShop", 2, 1, true);
            UiManager.SetMain(MainShopUi);
            MainShopUi.SetRowRatio(new double[] { .1, 1.5 });
            MainShopUi.AddNewUI(new UiContainerGridContent("Header", "Welcom to shop what do you need?", null), 0);

            UiContainerGrid TransferUi = new UiContainerGrid("TransferUi", 2, 2);
            TransferUi.SetRowRatio(new double[] { .1, 1.5 });
            MainShopUi.AddNewUI(TransferUi, 1);

            TransferUi.AddNewUI(new UiContainerGridContent("BuyHeader", "Buy", null), 0);
            TransferUi.AddNewUI(new UiContainerGridContent("SellHeader", "Sell", null), 1);

            UiInventoryContainer BuyItems = new UiInventoryContainer("BuyItems", 6, 6, player.GetInventory());
            TransferUi.AddNewUI(BuyItems, 2);
            UiInventoryContainer SellItems = new UiInventoryContainer("SellItems", 6, 6, merchant.GetInventory());
            TransferUi.AddNewUI(SellItems, 2);

            UiCursor.InitialCursor(TransferUi);

            while (true)
            {
                SellItems.RefreshItems();
                BuyItems.RefreshItems();
                //Todo Udtate Inventory When Used
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
        public static bool UIInput()
        {
            bool pressed = false;
            //input
            if (InputManager.IsKeyReleased(EInput.UP))
            {
                UiCursor.Move(EDirection.UP);
                pressed = true;
            }
            if (InputManager.IsKeyReleased(EInput.DOWN))
            {
                UiCursor.Move(EDirection.DOWN);
                pressed = true;
            }
            if (InputManager.IsKeyReleased(EInput.LEFT))
            {
                UiCursor.Move(EDirection.LEFT);
                pressed = true;
            }
            if (InputManager.IsKeyReleased(EInput.RIGHT))
            {
                UiCursor.Move(EDirection.RIGHT);
                pressed = true;
            }
            if (InputManager.IsKeyReleased(EInput.ENTER))
            {
                UiCursor.Click();
                pressed = true;
            }
            if (InputManager.IsKeyReleased(EInput.ESCAPE))
            {
                UiCursor.Escape();
                pressed = true;
            }
            return pressed;
        }
        public static void CharacterAdventureInput(Player player)
        {
            //대각 예외
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
            if (InputManager.IsKeyReleased(EInput.T))
                player.FindPath();
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




    }
}
