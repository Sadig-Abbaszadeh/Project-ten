﻿public struct CoordPair
{
    public int x { get; private set; }
    public int y { get; private set; }

    public CoordPair(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}