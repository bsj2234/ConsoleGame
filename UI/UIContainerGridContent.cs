using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    public class UiContainerGridContent: UiContainerGrid
    {
        public object? Content {get; set;}


        private event Action? onClick;
        private event Func<string>? onContent;
        private event Action? onFocus;

        //진짜 생각해보자
        //며러개로 나눴어
        //현재 콘테이너이 시작과 끝은 알아야하잖아
        //그렇다면 처음에 시작과 크기를 받아올거야
        //모두위로 올라가서
        //(위치+크기)/전체인덱스 * 현재 인덱스
        //내위치 계산법 계속 부모의 크기를 알아야한다
        public UiContainerGridContent(string name, Func<string> content, Action? action, int rowCount = 1, int columnCount = 1, bool isMain = false)
            : base(name, rowCount, columnCount, isMain)
        {
            onContent = content;
            onClick += action;
            focus = false;
        }
        public UiContainerGridContent(string name, string content, Action? action, int rowCount = 1, int columnCount = 1, bool isMain = false) 
            : base(name, rowCount, columnCount, isMain)
        {
            Content = content;
            onClick += action;
            focus = false;
        }
        public void AddOnClick(Action action)
        {
            onClick += action;
        }
        public override void OnClick()
        {
            if(onClick != null)
                onClick.Invoke();
        }

        public override void OnFocus() 
        {
            focus = true;
        }
        public override void OnLoseFocus()
        {
            focus = false;
        }
        public override void Draw()
        {
            string content = "";
            base.Draw();
            if(Content != null)
            {
                content = Content.ToString().Replace("\r\n", "\n");
            }
            else if(onContent != null)
            {
                content = onContent().ToString().Replace("\r\n", "\n");
            }
            Vec2 ContextPos = new Vec2(UIPosAbsolute.X + 1, UIPosAbsolute.Y +1);
            //줄의 최대길이를 계산해서 너비계산
            //잠시만 이거 취소 이유 버그발생 사라지는버그
            int maxLineLenght = 0;
            int curLineLength = 0;
            for (int i = 0; i < content.Length; i++)
            {
                curLineLength++;
                if (maxLineLenght < curLineLength)
                    maxLineLenght = curLineLength;
                if (content[i] == '\n')
                    curLineLength = 0;
            }
            //ContextPos.SetX(ContextPos.X - maxLineLenght);

            RenderManager.Draw(content, ContextPos.X, ContextPos.Y);
        }
    }
}
