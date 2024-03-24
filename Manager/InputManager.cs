using MyData;
using System.Runtime.InteropServices;

namespace ConsoleGameProject
{
    public class InputEventArgs : EventArgs
    {
        private EInput inputType;

        public InputEventArgs(EInput inputType)
        {
            this.inputType = inputType;
        }

        public EInput GetInputType()
        {
            return inputType;
        }
    }
    
    /*
     * 키를 뗏을 때 이벤트 발생
     * 어떤 키인지 검사후 키에 해당하는 이벤트가 발생했다는 사실을 알림
     * 해당이벤트를 구돗하는 구독자들에게 실행할 함수를 받아 등록함
     * 등록된 함수를 키가 뗴어졌을때 호출해줌
     * 대신 호출해준다
     */
    public static class InputManager
    {
        [DllImport("User32.dll")]
        public extern static ushort GetAsyncKeyState(int virtualKey);


        private static Dictionary<EInput, bool> PrevPressedKeys = new Dictionary<EInput, bool>();
        private static Dictionary<EInput, bool> ReleasedKeys = new Dictionary<EInput, bool>();
        private static int EInputSize = Enum.GetValues<EInput>().Length;


        private static Dictionary<EInput, EventHandler> inputPressedEventDict = new Dictionary<EInput, EventHandler>();
        private static Dictionary<EInput, EventHandler> inputReleaseEventDict = new Dictionary<EInput, EventHandler>();
        public static void AddPressedEvent(EInput key, EventHandler eventHandler)
        {
            inputPressedEventDict[key] += eventHandler;
        }
        public static void RemovePressedEvent(EInput key, EventHandler eventHandler)
        {
            inputPressedEventDict[key] -= eventHandler;
        }
        public static void OnPressedInput(EInput input)
        {
            if (inputPressedEventDict[input] != null)
            {
                inputPressedEventDict[input].Invoke(null, EventArgs.Empty);
            }
        }
        public static void AddReleasedEvent(EInput key, EventHandler eventHandler)
        {
            inputReleaseEventDict[key] += eventHandler;
        }
        public static void RemoveReleasedEvent(EInput key, EventHandler eventHandler)
        {
            inputReleaseEventDict[key] -= eventHandler;
        }
        public static void OnReleasedInput(EInput input)
        {
            if (inputReleaseEventDict[input] != null)
            {
                inputReleaseEventDict[input].Invoke(null, EventArgs.Empty);
            }
        }

        public static void Init()
        {
            //init dictionarys
            foreach (EInput input in Enum.GetValues(typeof(EInput))) 
            {
                PrevPressedKeys[input] = false;
                ReleasedKeys[input] = false;
                inputPressedEventDict[input] = null;
                inputReleaseEventDict[input] = null;
            }
        }


        public static bool IsKeyPressed(EInput key)
        {
            return IsAsyncVirtualKeyPressed(key);
        }
        public static bool IsKeyReleased(EInput key)
        {
            return ReleasedKeys[key];
        }

        //should called every end of loop
        public static void UpdateKeys()
        {
            //뗀 키들 초기화
            ResetKeyBuffer(ReleasedKeys);
            //키가 뗴어졌는지 업데이트
            foreach (EInput key in PrevPressedKeys.Keys)
            {
                if (!IsKeyPressed(key) && PrevPressedKeys[key])
                {
                    OnReleasedInput(key);
                    ReleasedKeys[key] = true;
                }
            }
            //다음 업데이트때 키가 뗴졌는지 확인을 위해 전에 누른키 업데이트
            foreach (EInput key in Enum.GetValues(typeof(EInput)))
            {
                bool pressed = IsKeyPressed((EInput)key);
                if (pressed)
                {
                    OnPressedInput(key);
                }
                PrevPressedKeys[(EInput)key] = pressed;
            }
        }
        private static bool IsAsyncVirtualKeyPressed(EInput key)
        {
            switch (key)
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
                case EInput.T:
                    return (GetAsyncKeyState((int)VKeys.KEY_T) & 0x8000) > 0;
                case EInput.Y:
                    return (GetAsyncKeyState((int)VKeys.KEY_Y) & 0x8000) > 0;
                default:
                    return false;
            }
        }
        private static void ResetKeyBuffer(Dictionary<EInput, bool> dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                dictionary[key] = false;
            }
        }
    }
}
