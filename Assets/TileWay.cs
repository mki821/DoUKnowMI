using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWay
{
    public int x;
    public int y;

    public Vector2 ConverVector2 // 자동으로 해쥼 (많이 쓸거가틈)
    {
        get => new(x, y);
    }

    // class끼리 더하는거임 아무튼 개편함
    public static TileWay operator +(TileWay a, TileWay b)
    {
        return new TileWay(a.x + b.x, a.y + b.y);
    }
    public static TileWay operator -(TileWay a, TileWay b) // 빼는것도 할건뎅
    {
        return new TileWay(a.x - b.x, a.y - b.y);
    }

    public TileWay(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    // 유용한 기능일껄??
    public static TileWay[] GetWays(TileWay start, TileWay end)
    {
        var NowCoords = new TileWay(start.x, start.y);

        List<TileWay> Ways = new();

        while (NowCoords.x != end.x || NowCoords.y != end.y)
        {
            // X 움직이기
            if (NowCoords.x != end.x)
            {
                if (end.x > NowCoords.x)
                { // 오른쪽으로 가야징
                    NowCoords.x++;
                }
                else
                { // 왼쪽으로 가야짐
                    NowCoords.x--;
                }
            }

            //Y 움직일껀뎅
            if (NowCoords.y != end.y)
            {
                if (end.y > NowCoords.y)
                { // 위로 가야징
                    NowCoords.y++;
                }
                else
                { // 아래로 가야짐
                    NowCoords.y--;
                }
            }

            Ways.Add(new TileWay(NowCoords.x, NowCoords.y));
        }

        // 변환 (솔직히 변환 안해도 되긴 하는데 이러게 하면 마음이 편함 ~_-)
        TileWay[] SendPacket = new TileWay[Ways.Count - 1 /* <-- 어차피 마지막은 안넣으니까 뺌 ㅅㄱㅋㅋㅋ */];
        for (int i = 0; i < Ways.Count - 1 /* 마지막꺼는 어차피 end랑 똑같은 좌표임ㅁㅁㅁㅁ */; i++)
            SendPacket[i] = Ways[i];

        return SendPacket;
    }
    
    // 가는 방향구하기
    public enum Direction {
        None,
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        RightUp,
        LeftDown,
        RightDown
    }
    public static Direction GetDirection(TileWay start, TileWay end) {
        var diff = end - start;
        if (diff.x == 0) {
            return diff.y == 1 ? Direction.Up : Direction.Down;
        } else if (diff.x == 1) {
            return diff.y == 0 ? Direction.Right : (diff.y == 1 ? Direction.RightUp : Direction.RightDown);
        } else if (diff.x == -1) {
            return diff.y == 0 ? Direction.Left : (diff.y == 1 ? Direction.LeftUp : Direction.LeftDown);
        }
        return Direction.None;
    }
    public static TileWay GetWayToDirection(Direction direction) {
        switch (direction)
        {
            case Direction.Up:
                return new TileWay(0,1);
            case Direction.Down:
                return new TileWay(0,-1);
            case Direction.Left:
                return new TileWay(-1,0);
            case Direction.Right:
                return new TileWay(1,0);
            case Direction.LeftUp:
                return new TileWay(-1,1);
            case Direction.RightUp:
                return new TileWay(1,1);
            case Direction.LeftDown:
                return new TileWay(-1,-1);
            case Direction.RightDown:
                return new TileWay(1,-1);
            default:
                return null;
        }
    }
}