namespace ArchitecturalPlanner;

public static class ButtonExstensions
{
    public static Button Copy(this Button button)
    {
        var newBut = new Button()
        {
            Location = button.Location,
            BackColor = button.BackColor,
            Width = button.Width,
            Height = button.Height,
            Text = button.Text,
        };

        newBut.FlatAppearance.BorderColor = button.FlatAppearance.BorderColor;
        return newBut;
    }
}
