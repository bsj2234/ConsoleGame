using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MyBuffer;
using System.Security.Cryptography.X509Certificates;
using System.Buffers;
using System.Reflection;

namespace ConsoleGameProject
{
    //그리드형으로 변경
    //크기고정 그리드형
    //그리드의 사이즈 크기 고정?
    //행과 열의 크기는 다를수 있지 않냐
    //일단같게해
    //몇행 몇열
    //UI크기
    //선택
    //아니면 전체화면을 비율로 나눠서 그릴까
    //일단 전체 화면은 하나의 그리드로 만들고
    //그 안에 화면을 구분해서 게임화면도 그리고 등등
    //구분 선도 그려주면 좋겠어
    //생성할때 정해야하는건
    //내가가질행과열의크기
    //내가속할 행과열은 내가 가지는게 아니지
    //내가 가지고있는 모든 것들에 대한 경계선 처리
    //내부의 컨테이너는 보통 고정크기일텐데
    //스크롤기능을 지원해야 한다면
    //가변크기일 필요가 있을수 있음

    public class UiContainerGrid: UIContainer
    {
        //내가 무엇을 가지고 있느냐에 따라 행동이 달라진다
        //컨테이너라면 보더를 그려야 할것이다
        //일반 UI라면 보더는 그리되 하나의 UI만을 가진다
        //행렬크기설정
        protected int columnCount = 0;
        protected int rowCount = 0;
        protected List<double> rowRatio = new List<double>();
        protected List<double> colRatio = new List<double>();


        public UiContainerGrid(string name,int rowCount, int columnCount, bool isMain = false) 
            : base(name, isMain)
        {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            for (int i = 0; i < columnCount; i++)
            {
                colRatio.Add(1);
            }
            for (int i = 0; i < rowCount; i++)
            {
                rowRatio.Add(1);
            }
            NormalizeRatio(this.rowRatio);
            NormalizeRatio(this.colRatio);
        }
        public void SetColRatio(double[] ratios)
        {
            if (ratios.Length > colRatio.Count)
                return;
            for (int i = 0; i < colRatio.Count; i++)
            {
                colRatio[i] = ratios[i];
            }
            NormalizeRatio(this.colRatio);
        }
        public void SetRowRatio(double[] ratios)
        {
            if (ratios.Length > rowRatio.Count)
                return;
            for (int i = 0; i < rowRatio.Count; i++)
            {
                rowRatio[i] = ratios[i];
            }
            NormalizeRatio(this.rowRatio);
        }
        private void NormalizeRatio(List<double> list)
        {
            double sum = 0;
            foreach (double d in list)
            {
                sum += d;
            }
            for(int i = 0;i < list.Count;i++)
            {
                list[i] /= sum;
            }

        }

        public void AddNewUI(UI newUi,int index)
        {
            if (newUi == null)
            {
                ContentUIList.Add(null);//빈공간 남겨두기
                return;
            }
            int currentIndex = ContentUIList.Count;
            newUi.InitOwner(this, currentIndex);
            ContentUIList.Add(newUi);
        }
        public UI GetContent(int x, int y)
        {
            return GetContent(y * columnCount + x);
        }
        public UI GetContent(Vec2 v)
        {
            return GetContent(v.X, v.Y);
        }
        override public UI GetContent(int index)
        {
            return ContentUIList[index];
        }
        public UI GetRelativeDirectionUI(EDirection d, int index)
        {
            int temp = index;
            switch (d)
            {
                case EDirection.UP:
                    temp -= columnCount;
                    break;
                case EDirection.DOWN:
                    temp += columnCount;
                    break;
                case EDirection.LEFT:
                    temp -= 1;
                    break;
                case EDirection.RIGHT:
                    temp += 1;
                    break;
                default:
                    throw new InvalidDataException();
                    break;
            }

            return this.ContentUIList[temp];
        }

        //My parents will has position and size the origin UI will have 0,0 tn screenSize.x screenSize.y
        //then I can Make use of them
        //SO i Need Owner
        private void DrawUIBorderLine()
        {
            if(focus == true && GameManager.UiFocusedBlink == true) 
            {
                return;
            }
            //약간 노가다로 만듬
            //사실 사이즈 -1 하고 -2로 양쪽한칸씩 뺀 갯수임
            RenderManager.DrawHorizontal('-', UIPosAbsolute + new Vec2(1, 0), UiSize.X - 2);
            RenderManager.DrawHorizontal('-', UIPosAbsolute + new Vec2(1, UiSize.Y - 1), UiSize.X - 2);
            RenderManager.DrawVertical('|', UIPosAbsolute + new Vec2(0, 1), UiSize.Y - 2);
            RenderManager.DrawVertical('|', UIPosAbsolute + new Vec2(UiSize.X - 1, 1), UiSize.Y - 2);
            /*if(focus == true)
            {
                RenderManager.ColorHorizontal(ConsoleColor.Blue, UIPosAbsolute + new Vec2(1, 0), UISize.X - 2);
                RenderManager.ColorHorizontal(ConsoleColor.Blue, UIPosAbsolute + new Vec2(1, UISize.Y - 1), UISize.X - 2);
                RenderManager.ColorVertical(ConsoleColor.Blue, UIPosAbsolute + new Vec2(0, 1), UISize.Y - 2);
                RenderManager.ColorVertical(ConsoleColor.Blue, UIPosAbsolute + new Vec2(UISize.X - 1, 1), UISize.Y - 2);
            }*/
        }

        //override public Vec2 GetAbsolutePosition(int index)
        //{
        //    //뭔가 변경되면 절대값위치 다 새로 등록되어야한다 나둥에 생각해보자 이건 너무 오래걸릴듯
        //    //지그은 그냥 맨처음에 지정해주는걸로하자
        //    if (owner == null)
        //        return new Vec2(0,0);
        //    else
        //    {
        //        //owner의 형에 따라 absolutePosition이 달라진다
        //        Vec2 ownerPosition = owner.GetAbsolutePosition(index);
        //        int itemSizeX = this.UISize.X / columnCount;
        //        int itemSizeY = this.UISize.Y / rowCount;
        //        int X = GetColumnIndex(index);
        //        int Y = GetRowIndex(index);
        //        int gapX = GetGap(X);
        //        int gapY = GetGap(Y);

        //        //현재 나의 인덱스 앞의 모든 수들을 합해야함
        //        return new Vec2(ownerPosition.X + X*itemSizeX, ownerPosition.Y + Y * itemSizeY);
        //    }
        //}
        public override void OnFocus()
        {
            this.focus = true;
        }
        public override void OnLoseFocus()
        {
            this.focus = false;
        }

        public override void Draw()
        {
            //Vec2 temp = GetAbsolutePosition(index);
            this.DrawUIBorderLine();
            foreach(var item in ContentUIList)
            {
                item.Draw();
            }
        }

        public override int GetRowIndex(int index)
        {
            return index / columnCount;
        }
        public override int GetColumnIndex(int index)
        {
            return index % columnCount;
        }

        public override Vec2 GetUiSizeOfIndex(int index)
        {
            int x = GetColumnIndex(index);
            int y = GetRowIndex(index);
            //하위UI절대크기 인덱스에 해당하는 레이티오와 현제 UI의 사이즈의 곱
            return new Vec2((int)(UiSize.X * colRatio[x]) - 2, (int)(UiSize.Y * rowRatio[y]) - 2 );
        }

        public override Vec2 GetAbsoluteUiPositionOfIndex(int index)
        {
            int x = GetColumnIndex(index);
            int y = GetRowIndex(index);
            double sumRatioBefroeIndexX = 0;
            double sumRatioBefroeIndexY = 0;
            for (int i = 0; i < x; i++)
            {
                sumRatioBefroeIndexX += colRatio[i];
            }
            for (int i = 0; i < y; i++)
            {
                sumRatioBefroeIndexY += rowRatio[i];
            }
            return new Vec2((int)(sumRatioBefroeIndexX * UiSize.X) + 1 + UIPosAbsolute.X, (int)(sumRatioBefroeIndexY * UiSize.Y) + 1+ UIPosAbsolute.Y);
        }


        override public int GetMovedIndex(int cur, EDirection direction)
        {
            int temp = cur;
            int x = cur % columnCount;
            int y = cur / columnCount;
            switch (direction)
            {
                case EDirection.UP:
                    y --;
                    break;
                case EDirection.DOWN:
                    y ++;
                    break;
                case EDirection.LEFT:
                    x --;
                    break;
                case EDirection.RIGHT:
                    x ++;
                    break;
                default:
                    break;
            }
            int newIndex = x + y * columnCount;
            if (x < 0 || y < 0 || x >= columnCount || y > rowCount )
            {
                return temp;
            }
            if (newIndex >= ContentUIList.Count)
            {
                return temp; 
            }
            if (ContentUIList[newIndex] == null)
            {
                return temp; 
            }
            return x + y * columnCount;
        }

        internal void Clear()
        {
            ContentUIList.Clear();
        }
    }
}
