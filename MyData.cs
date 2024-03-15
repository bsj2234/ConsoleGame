using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyData
{
    public struct Vec2
    {
        public int X;
        public int Y;

        public  Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Set(int x, int y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public void SetX(int x)
        {
            X = (int)x;
        }

        public void SetY(int y)
        {
            Y = (int)y;
        }

        public static Vec2 operator +(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static bool operator ==(Vec2 lhs, Vec2 rhs)
        {
            if (lhs.X == rhs.X && lhs.Y == rhs.Y)
                return true;
            return false;
        }
        public static bool operator !=(Vec2 lhs, Vec2 rhs)
        {
            if (lhs.X == rhs.X && lhs.Y == rhs.Y)
                return false;
            return true;
        }
        public static Vec2 operator *(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.X * rhs.X, lhs.Y * rhs.Y);
        }
        public static Vec2 operator *(Vec2 lhs, int rhs)
        {
            return new Vec2(lhs.X * rhs, lhs.Y * rhs);
        }
        public static Vec2 operator *(Vec2 lhs, double rhs)
        {
            return new Vec2((int)(lhs.X * rhs), (int)(lhs.Y * rhs));
        }
    }

    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT
    }


    //아에 다른 화면을 그리기 위한 스테이트
    //인풋과 관련이 많게 해놓을까
    //따로 만들자
    //

    public enum GameState
    {
        ADVENTURE, FIGHT,
    }
    public enum InputState
    {
        ADVENTURE, FIGHT, PAUSE
    }

    public struct UIPositionInfo
    {
        public int StartX;
        public int StartY;
        public int EndX;
        public int EndY;

        public UIPositionInfo(int startX, int startY, int endX, int endY)
        {
            this.StartX = startX;
            this.EndX = endX;
            this.StartY = startY;
            this.EndY = endY;
        }
    }

}
