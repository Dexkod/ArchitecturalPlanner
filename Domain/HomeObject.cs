using System.Drawing;

namespace Domain;

public class HomeObject
{
    public int LocationX { get; set; }
    public int LocationY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public float V { get; set; }

    public string? Name { get; set; }
    public Color Color { get; set; }

    public HomeObject(int locationX, int locationY, int width, int height, float v, string name, Color color)
    {
        LocationX = locationX; 
        LocationY = locationY;
        Width = width;
        Height = height;
        V = v;
        Name = name;
        Color = color;
    }
}
