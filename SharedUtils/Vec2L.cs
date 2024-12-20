namespace SharedUtils;

public class Vec2L
{
    public readonly long X;
    public readonly long Y;

    public Vec2L(long x, long y)
    {
        X = x;
        Y = y;
    }

    public Vec2L Move(long x, long y)
    {
        return new Vec2L(X + x, Y + y);
    }
    public Vec2L Move(Vec2L moveVec2L)
    {
        return new Vec2L(X + moveVec2L.X, Y + moveVec2L.Y);
    }

    public bool IsWithinBounds(long maxX, long maxY, long minX = 0, long minY = 0)
    {
        if (X > maxX) return false;
        if (X < minX) return false;
        if (Y > maxY) return false;
        if (Y < minY) return false;

        return true;
    }
    public bool IsWithinBounds(Vec2L maxSize, Vec2L minSize)
    {
        if (X > maxSize.X) return false;
        if (X < minSize.X) return false;
        if (Y > maxSize.Y) return false;
        if (Y < minSize.Y) return false;

        return true;
    }
    public bool IsWithinBounds(Vec2L maxSize)
    {
        if (X > maxSize.X) return false;
        if (X < 0) return false;
        if (Y > maxSize.Y) return false;
        if (Y < 0) return false;

        return true;
    }
    
    public double DistanceToPythagoras(Vec2L to)
    {
        return Math.Sqrt(Math.Pow(X - to.X, 2) + Math.Pow(Y - to.Y, 2));
    }
    
    public long DistanceToManhattan(Vec2L to)
    {
        return Math.Abs(to.X - X) + Math.Abs(to.Y - Y);
    }

    public Vec2L Difference(Vec2L to)
    {
        return new Vec2L(to.X - X, to.Y - Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var p = (Vec2L)obj;
        return X == p.X && Y == p.Y;
    }

    public Vec2L[] GetNeighbors()
    {
        return new[]
        {
            Move(1, 0),
            Move(-1, 0),
            Move(0, 1),
            Move(0, -1)
        };
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public Direction GetDirectionTo(Vec2L neighbor)
    {
        var diff = Difference(neighbor);
        return diff switch
        {
            { X: 0, Y: -1 } => Direction.UP,
            { X: 0, Y: 1 } => Direction.DOWN,
            { X: -1, Y: 0 } => Direction.LEFT,
            { X: 1, Y: 0 } => Direction.RIGHT,
            _ => throw new ArgumentOutOfRangeException(nameof(neighbor), neighbor, null)
        };
    }
}