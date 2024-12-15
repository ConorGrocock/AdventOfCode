namespace SharedUtils;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public static class DirectionExtensions
{
    public static Vec2L ToVec2L(this Direction direction)
    {
        return direction switch
        {
            Direction.UP => new Vec2L(0, -1),
            Direction.DOWN => new Vec2L(0, 1),
            Direction.LEFT => new Vec2L(-1, 0),
            Direction.RIGHT => new Vec2L(1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction FromChar(char c)
    {
        return c switch
        {
            'U' => Direction.UP,
            'D' => Direction.DOWN,
            'L' => Direction.LEFT,
            'R' => Direction.RIGHT,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    public static Direction FromAngledChar(char c)
    {
        return c switch
        {
            '^' => Direction.UP,
            'v' => Direction.DOWN,
            '<' => Direction.LEFT,
            '>' => Direction.RIGHT,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    public static Direction FromString(string s)
    {
        return s switch
        {
            "UP" => Direction.UP,
            "DOWN" => Direction.DOWN,
            "LEFT" => Direction.LEFT,
            "RIGHT" => Direction.RIGHT,
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }

    public static string ToDirectionString(this Direction direction)
    {
        return direction switch
        {
            Direction.UP => "^",
            Direction.DOWN => "v",
            Direction.LEFT => "<",
            Direction.RIGHT => ">",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}