using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    public class UIContainerGridContent: UIContainerGrid
    {
        public object Content {get; set;}
        private object focusedContent;
        private object nonFocusedContent;


        private event Action onClick;
        private event Action onFocus;

        //진짜 생각해보자
        //며러개로 나눴어
        //현재 콘테이너이 시작과 끝은 알아야하잖아
        //그렇다면 처음에 시작과 크기를 받아올거야
        //모두위로 올라가서
        //(위치+크기)/전체인덱스 * 현재 인덱스
        //내위치 계산법 계속 부모의 크기를 알아야한다
        public UIContainerGridContent(object content, Action action ,int rowCount = 1, int columnCount = 1, bool isMain = false) : base(rowCount,columnCount,isMain)
        {
            Content = "  " + content;
            onClick += action;
            focus = false;
            //focusedContent = "▶" + content;
            //nonFocusedContent= "  " + content;
            focusedContent = content;
            nonFocusedContent= content;
        }

        public override void OnClick()
        {
            onClick.Invoke();
        }

        public override void OnFocus() 
        {
            focus = true;
            Content = focusedContent;
        }
        public override void OnLoseFocus()
        {
            focus = false;
            Content = nonFocusedContent;
        }
        public override void Draw()
        {
            base.Draw();

            Content = Content.ToString().Replace("\r\n", "\n");
            Vec2 ContextPos = UISize * .5 + UIPosAbsolute;
            int maxLineLenght = 0;
            int curLineLength = 0;
            //잠시만 이거 취소 이유 버그발생 사라지는버그
            //for (int i = 0; i < Content.Length; i++)
            //{
            //    curLineLength++;
            //    if(maxLineLenght < curLineLength)
            //        maxLineLenght = curLineLength;
            //    if (Content[i] == '\n')
            //        curLineLength = 0;
            //}
            //ContextPos.SetX(ContextPos.X - maxLineLenght);

            RenderManager.Draw(Content.ToString(), ContextPos.X, ContextPos.Y);
        }
        override public UI GetContent(int index)
        {
            return ContentUIList[index];
        }

    }
}
