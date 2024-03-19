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


        private static Dictionary<EInput, bool> PrevPressedKeys = new Dictionary<EInput, bool>();
        private static Dictionary<EInput, bool> ReleasedKeys = new Dictionary<EInput, bool>();
        private static int EInputSize = Enum.GetValues<EInput>().Length;

        public static void Init()
        {
            ResetKeyBuffer(PrevPressedKeys);
            ResetKeyBuffer(ReleasedKeys);
        }
        public static bool IsKeyPressed(EInput key)
        {
            return GetPressedVirtualKey(key);
        }
        public static bool IsKeyReleased(EInput key)
        {
            return ReleasedKeys[key];
        }

        //should called every end of loop
        public static void UpdateKeys()
        {
            ResetKeyBuffer(ReleasedKeys);
            foreach (EInput key in PrevPressedKeys.Keys)
            {
                SetKeyReleased(key);
                for (int i = 0; i < EInputSize; i++)
                {
                    if (IsKeyPressed((EInput)i))
                    {
                        PrevPressedKeys[(EInput)i] = true;
                    }
                }
            }

        }
        public static bool GetPressedVirtualKey(EInput key)
        {
            switch(key)
            {
                case EInput.UP:
                    return (GetAsyncKeyState((int)VKeys.UP) & 0x8000) > 0;
                case EInput.DOWN:
                    return (GetAsyncKeyState((int)VKeys.DOWN) & 0x8000) > 0;
                case EInput.LEFT:
                    return (GetAsyncKeyState((int)VKeys.LEFT) & 0x8000) > 0;
                case EInput.RIGHT:
                    return (GetAsyncKeyState((int)VKeys.RIGHT) & 0x8000) > 0;
                case EInput.ESCAPE:
                    return (GetAsyncKeyState((int)VKeys.ESCAPE) & 0x8000) > 0;
                case EInput.ENTER:
                    return (GetAsyncKeyState((int)VKeys.RETURN) & 0x8000) > 0;
                default:
                    return false;
            }
        }
        private static void ResetKeyBuffer(Dictionary<EInput, bool> dictionary)
        {
            for (int i = 0; i < EInputSize; i++)
            {
                EInput key = (EInput)i;
                dictionary[key] = false;
            }
        }

        //ToDo:Fix rarly missing release key
        private static void SetKeyReleased(EInput key)
        {
            if (!IsKeyPressed(key) && PrevPressedKeys[key])
            {
                ReleasedKeys[key] = true;
                PrevPressedKeys[key] = false;
            }
        }
    }
}
