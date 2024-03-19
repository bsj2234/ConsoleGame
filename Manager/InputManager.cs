using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    /*
     * 잠깐
     * 버퍼에서 방향만 읽어온다면다른 입력들은 사라지는걸
     * 유효하여야 하지 않는가
     * 그럼 뺴놨다가 다시넣을까? ㅋㅋ
     * 
     * 한프레임에 여러 인풋이 들어왔을때 순서대로 처리해야한다는 느낌이 든다
     * 우선순위를 정해버리자
     * 일시정지, 이동, 상호작용의 키를 사용할거야
     * while로 계속 루프돌면서 받는 방법도 있어
     * 이거롤 가자
     * 
     * 아니다 턴을 제한하자
     * WinApi 사용하자
     * 
     */

    public static class InputManager
    {
        [DllImport("User32.dll")]
        public extern static ushort GetAsyncKeyState(int virtualKey);

        static Stack<ConsoleKey> consoleInputBuffer = new Stack<ConsoleKey>();
        public static EInput GetPushedInput()
        {
            //input
            ushort pressed = (ushort)0x8000;
            if ((GetAsyncKeyState((int)VKeys.UP) & pressed) > 0)
                return EInput.UP;
            if ((GetAsyncKeyState((int)VKeys.DOWN) & pressed) > 0)
                return EInput.DOWN;
            if ((GetAsyncKeyState((int)VKeys.LEFT) & pressed) > 0)
                return EInput.LEFT;
            if ((GetAsyncKeyState((int)VKeys.RIGHT) & pressed) > 0)
                return EInput.RIGHT;
            if ((GetAsyncKeyState((int)VKeys.RETURN) & pressed) > 0)
                return EInput.ENTER;
            if ((GetAsyncKeyState((int)VKeys.ESCAPE) & pressed) > 0)
                return EInput.ESCAPE;
            return EInput.NONE;
        }
        public static EInput GetReleasedInput()
        {
            //input
            ushort pressed = (ushort)0x0001;
            if ((GetAsyncKeyState((int)VKeys.UP) & pressed) > 0)
                return EInput.UP;
            if ((GetAsyncKeyState((int)VKeys.DOWN) & pressed) > 0)
                return EInput.DOWN;
            if ((GetAsyncKeyState((int)VKeys.LEFT) & pressed) > 0)
                return EInput.LEFT;
            if ((GetAsyncKeyState((int)VKeys.RIGHT) & pressed) > 0)
                return EInput.RIGHT;
            if ((GetAsyncKeyState((int)VKeys.RETURN) & pressed) > 0)
                return EInput.ENTER;
            if ((GetAsyncKeyState((int)VKeys.ESCAPE) & pressed) > 0)
                return EInput.ESCAPE;
            return EInput.NONE;
        }
        public static bool IsKeyReleased(EInput key)
        {
            //input
            ushort released = (ushort)0x0001;
            switch(key)
            {
                case EInput.UP:
                    return (GetAsyncKeyState((int)VKeys.UP) & released) > 0;
                case EInput.DOWN:
                    return (GetAsyncKeyState((int)VKeys.DOWN) & released) > 0;
                case EInput.LEFT:
                    return (GetAsyncKeyState((int)VKeys.LEFT) & released) > 0;
                case EInput.RIGHT:
                    return (GetAsyncKeyState((int)VKeys.RIGHT) & released) > 0;
                case EInput.ESCAPE:
                    return (GetAsyncKeyState((int)VKeys.ESCAPE) & released) > 0;
                case EInput.ENTER:
                    return (GetAsyncKeyState((int)VKeys.RETURN) & released) > 0;
                default:
                    return false;
            }
        }
        public static void ClearInputBuffer() 
        {
            consoleInputBuffer.Clear();
        }
    }
}
