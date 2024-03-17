using MyBuffer;
using MyData;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ConsoleExtender;
using System;

namespace ConsoleGameProject
{
    internal class Program
    {
        static Stack<ConsoleKey> consoleKeys = new Stack<ConsoleKey>();

        //static readonly Vec2 MapSize = new Vec2(110, 60);
        static readonly Vec2 MapSize = new Vec2(220, 60);
        //static readonly Vec2 MapSize = new Vec2( 55, 30 );
        public readonly static Vec2 CENTER_OFFSET = new Vec2(MapSize.X/2, MapSize.Y/2);

        public static List<Actor> AllActors = new List<Actor>();

        static InputState inputState = InputState.ADVENTURE;
        public static GameState gameState = GameState.ADVENTURE;

        [StructLayout(LayoutKind.Sequential)]
        public struct _COORD
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _CONSOLE_FONT_INFOEX
        {
            public ulong cbSize;
            public int nFont;
            public _COORD dwFontSize;
            public uint FontFamily;
            public uint FontWeight;
            public char[] FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SECURITY_ATTRIBUTES
        {
            int nLength;
            IntPtr lpSecurityDescriptor;
            bool bInheritHandle;
        }


        //static UIContainerGrid FightUIContainer = new UIContainerGrid(0,30);
        public static bool Blink = false;
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            RenderManager.ScreenInit(MapSize.X,MapSize.Y);
            //FightUIContatinerSetter.SetFightUI(FightUIContainer);
            Task inpuAsync = Task.Run(() => {
                while (true)
                {
                    if (Console.KeyAvailable == false)
                        continue;
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    consoleKeys.Push(keyInfo.Key);
                }
            });
            Task focusAsync = Task.Run(() => {
                while (true)
                {
                    Thread.Sleep(300);
                    Blink = !Blink;
                }
            });
            //https://gist.github.com/Shensd/01342e2f399de4dca2ca87b36059ba0a
            Task musicAsync = Task.Run(() => {
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
            });

            Player player = new Player("siko",100, CENTER_OFFSET, new Vec2(1,1),false);
            //Wall wall1 = new Wall("wall", new Vec2(20, 20), new Vec2(10, 2), false);
            HuntArea huntarea = new HuntArea("huntarea", new Vec2(100, 33), new Vec2(10, 2));



            PositionConsoleWindowDemo.SetWindowSize();

            //game loop
            while (true)
            {
                switch (gameState)
                {
                    case GameState.ADVENTURE:
                        Adventure();
                        break;
                    case GameState.FIGHT:
                        Fight();
                        break;
                }
                RenderManager.Show();
            }



            void Adventure()
            {
                //render
                //RenderManager.ScreenFillWideChar('ㅇ');

                //RenderManager.OldTime = stopwatch.ElapsedMilliseconds; // 시간을 측정한다. 1초마다 갱신한다.
                //RenderManager.CurTime = stopwatch.ElapsedMilliseconds;

                foreach (Actor actor in AllActors)
                {
                    RenderManager.DrawRelative(actor, player.GetPosition());
                }
                AdventureInput();
            }
            void AdventureInput()
            {
                //input
                while (consoleKeys.Count != 0)
                {
                    ConsoleKey input = consoleKeys.Pop();
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            player.Move(Direction.UP);
                            break;
                        case ConsoleKey.DownArrow:
                            player.Move(Direction.DOWN);
                            break;
                        case ConsoleKey.LeftArrow:
                            player.Move(Direction.LEFT);
                            break;
                        case ConsoleKey.RightArrow:
                            player.Move(Direction.RIGHT);
                            break;
                    }
                }
                consoleKeys.Clear();
            }




            void Fight()
            {
                Enemy enemy = new Enemy("KKong", 84);
                UIContainerGrid MainFightUi = new UIContainerGrid("MainFightUi",2, 1, true);
                var FightSceneUi = new UIContainerGridContent("FightSceneUi", """
                 ...                                             .*(,, */#.       .(/,*,*#.         
              /**// (##(&      ****/,                           *///**((%%*     *////**/*/((@        
                  ((((#   (,(##((%/(@                         (//////(#*      */////////(&          
                 *% ((&  /##((((#&@                           (///////(#*      /*///////(#*          
               % (**,,,/*/(((&                                #///////((#      (////////(%           
     .((.   .(/(//*,,#@&///(*                                 #///////(#.     (///////(#            
     #**///(////////////////(/                                 #////((#(*/,,,*(//////((@            
       #(((/ .//////////////(@                                 .#(((*///,/*,/*(/////((@             
          . //// ,/   /////##@                                  ///**/////////////((((#((           
          ////// ,*///, *#/*,,//#@     *///(                   .////////////////////((((#,          
          ,.     (, *((///(((%(###%%   (*,//((#                .(////////////////////(((((&          
           ,/, ...,///##,,*/&#(* *(    ,#(((#*                /#////////////////////(((((((&         
               ((*//##@@@     ##  @@ #%              /*,*////(@(//////////  /*///((((///%#@         
                   #(((((                          (/.  .*////(#, ///////    /((((&((///*///#%%#    
                   %@@                             #////,,////(##((,.,,,,#(((,,,,,((@(((/////////((#
""",
                    () => { }, 3, 3
                    );
                var FightBottomUi = new UIContainerGrid("FightSelectsUi", 1, 3);

                var SelectUi = new UIContainerGrid("Select", 2, 2);
                var FightBottom2 = new UIContainerGrid("FightBottom2", 3, 1);
                var FightLog = new UIContainerGridContent("FightLog", UIFightLogManager.GetContent ,null , 3, 1);
                var FightButton1 = new UIContainerGridContent("AttackButton", "Attack", null);
                var FightButton2 = new UIContainerGridContent("SkillButton", "Skillz", null);
                var FightButton3 = new UIContainerGridContent("ItemsButton", "Items", null);
                var FightButton4 = new UIContainerGridContent("Run", "Run", null);
                var StaticPlayer = new UIContainerGridContent("PlayerStatic", () => 
                { return $"{player.GetHp()}"; }
                , null);
                string.Format("str{0}",new object[5] {1,2,3,4,5});

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
                    //Todo Make ondead dropitem
                    player.Attack(enemy);

                    RenderManager.RenderUIContainer(MainFightUi);
                    RenderManager.Show();
                    Thread.Sleep(1000);
                    UIFightLogManager.Clear();
                });
                Task debugHp = Task.Run(() => { player.Damaged(10); });
                UICursor.InitialCursor(MainFightUi);



                while (true)
                {
                    FightInput();
                    RenderManager.RenderUIContainer(MainFightUi);
                    RenderManager.Show();
                    UIFightLogManager.Clear();
                }


            }
            void FightInput()
            {
                //input
                while (consoleKeys.Count != 0)
                {
                    ConsoleKey input = consoleKeys.Pop();
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            UICursor.Move(Direction.UP);
                            break;
                        case ConsoleKey.DownArrow:
                            UICursor.Move(Direction.DOWN);
                            break;
                        case ConsoleKey.LeftArrow:
                            UICursor.Move(Direction.LEFT);
                            break;
                        case ConsoleKey.RightArrow:
                            UICursor.Move(Direction.RIGHT);
                            break;
                        case ConsoleKey.Enter:
                            UICursor.Click();
                            break;
                        case ConsoleKey.Escape:
                            UICursor.Escape();
                            break;
                    }
                }
                consoleKeys.Clear();
            }
        }//endOfMain
        public static void ShakeScreen(int count)
        {
            Task ShakeAsync = Task.Run(() => {
                    PositionConsoleWindowDemo.ShakeWindow(3, 20, 50);
            });
        }
    }
}
