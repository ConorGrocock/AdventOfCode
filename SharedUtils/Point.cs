namespace SharedUtils;

public class Point
{
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point Move(int x, int y)
    {
        return new Point(X + x, Y + y);
    }
    public Point Move(Point movePoint)
    {
        return new Point(X + movePoint.X, Y + movePoint.Y);
    }

    public bool IsWithinBounds(int maxX, int maxY, int minX = 0, int minY = 0)
    {
        if (X > maxX) return false;
        if (X < minX) return false;
        if (Y > maxY) return false;
        if (Y < minY) return false;

        return true;
    }
    public bool IsWithinBounds(Point maxSize, Point minSize)
    {
        if (X > maxSize.X) return false;
        if (X < minSize.X) return false;
        if (Y > maxSize.Y) return false;
        if (Y < minSize.Y) return false;

        return true;
    }
    public bool IsWithinBounds(Point maxSize)
    {
        if (X > maxSize.X) return false;
        if (X < 0) return false;
        if (Y > maxSize.Y) return false;
        if (Y < 0) return false;

        return true;
    }
    
    public double DistanceToPythagoras(Point to)
    {
        return Math.Sqrt(Math.Pow(X - to.X, 2) + Math.Pow(Y - to.Y, 2));
    }
    
    public int DistanceToManhattan(Point to)
    {
        return Math.Abs(to.X - X) + Math.Abs(to.Y - Y);
    }

    public Point Difference(Point to)
    {
        return new Point(to.X - X, to.Y - Y);
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

        var p = (Point)obj;
        return X == p.X && Y == p.Y;
    }
}