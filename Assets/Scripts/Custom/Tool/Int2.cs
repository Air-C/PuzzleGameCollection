using System;
using UnityEngine;

[Serializable] // 加上这个属性, 才能在Inspector里显示
public struct Int2 {

    public int x, y;

    public Int2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Int2 operator +(Int2 a, Int2 b) {
        return new Int2(a.x + b.x, a.y + b.y);
    }

    public static implicit operator Vector2Int(Int2 a) {
        return new Vector2Int(a.x, a.y);
    }

    public static implicit operator Int2(Vector2Int a) {
        return new Int2(a.x, a.y);
    }
}