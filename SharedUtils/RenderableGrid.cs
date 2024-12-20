namespace SharedUtils;

public class RenderableGrid
{
    public Dictionary<ConsoleColor, KeyValuePair<Vec2L, char>> Points { get; set; } = new();
    public Vec2L MaxSize { get; set; } = new Vec2L(0, 0);
    public Vec2L MinSize { get; set; } = new Vec2L(0, 0);
    public Vec2L Offset { get; set; } = new Vec2L(0, 0);
    public bool ClearScreen { get; set; } = true;
    public RenderMode RenderMode { get; set; } = RenderMode.DELAY;


    public void Render()
    {
        if(RenderMode == RenderMode.DISABLED) return;

        if (ClearScreen)
        {
            Console.Clear();
        }

        foreach (var (color, (pos, character)) in Points)
        {
            Console.SetCursorPosition((int)pos.X + (int)Offset.X, (int)pos.Y + (int)Offset.Y);
            Console.ForegroundColor = color;
            Console.Write(character);
        }

        if (RenderMode == RenderMode.DELAY)
        {
            Thread.Sleep(100);
        }
    }
}