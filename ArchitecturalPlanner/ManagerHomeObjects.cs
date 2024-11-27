using Domain;

namespace ArchitecturalPlanner;

public class ManagerHomeObjects
{
    private static Button FocusObject;
    private const int Widht = 100;
    private const int Height = 70;
    private const int IntervalX = 120;
    private const int YBorder = 27;
    private const int XBorder = 10;
    private Form _form;
    public event Action<Control> Redrawing;
    public event Action<Control> RemoveControl;
    public Dictionary<Button, HomeObject> Objects { get; private set; } = new Dictionary<Button, HomeObject>();
    public Button Home { get; set; }
    private List<Control> RightPanel = new List<Control>();

    public ManagerHomeObjects(Form form)
    {
        foreach (var item in TypeObjects)
        {
            item.MouseDown += ButtonDefault_MouseDown!;
            item.MouseUp += ButtonDefault_MouseUp!;
        }

        this._form = form;
    }

    public Button Add(HomeObject homeObject, TypeHomeObject type)
    {
        var home = TypeObjects[(int)type].Copy();

        var button = new Button()
        {
            Name = (Guid.NewGuid()).ToString(),
            Width = homeObject.Width,
            Height = homeObject.Height,
            Text = homeObject.Name,
            BackColor = homeObject.Color,
            Location = new Point(homeObject.LocationX, homeObject.LocationY)
        };

        switch (type)
        {
            case TypeHomeObject.Room:
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 2;
                break;
            case TypeHomeObject.Door:
                button.Width = 5;
                button.BackColor = Color.Gray;
                break;
        }

        button.MouseDown += Button_MouseDown!;
        button.MouseUp += Button_MouseUp!;
        button.MouseClick += Button_Click!;

        Objects.Add(button, homeObject);
        return button;
    }

    public List<Button> TypeObjects { get; } = new List<Button>()
    {
        new Button()
        {
            Name = ((int)TypeHomeObject.Room).ToString(),
            Width = Widht,
            Height = Height,
            Text = "Room",
            BackColor = Color.AliceBlue,
            Location = new Point(XBorder, YBorder)
        },
        new Button()
        {
            Name = ((int)TypeHomeObject.Door).ToString(),
            Width = Widht,
            Height = Height,
            Text = "Door",
            BackColor = Color.AliceBlue,
            Location = new Point(XBorder + IntervalX, YBorder)
        }
    };

    private void ButtonDefault_MouseDown(object sender, MouseEventArgs e)
    {
        FocusObject = (Button)sender;
    }
    private void ButtonDefault_MouseUp(object sender, MouseEventArgs e)
    {
        if (FocusObject == null)
        {
            return;
        }

        var button = (Button)sender;
        var home = new HomeObject(e.X, e.Y, Widht, Height, 0, string.Empty, Color.Blue);
        var newButton = Add(home, (TypeHomeObject)Convert.ToInt32(button.Name));
        Redrawing.Invoke(newButton);
        FocusObject = null;
    }

    private void Button_MouseDown(object sender, MouseEventArgs e)
    {
        FocusObject = (Button)sender;
    }
    private void Button_MouseUp(object sender, MouseEventArgs e)
    {
        if (FocusObject != null)
        {
            var button = (Button)sender;

            Point newLocation = _form.PointToClient(Cursor.Position);
            int x = newLocation.X - FocusObject.Width / 2;
            int y = newLocation.Y - FocusObject.Height / 2;

            if (Home.Location.X > x)
            {
                x = Home.Location.X;
            }
            else if (Home.Location.X + Home.Width < x + button.Width)
            {
                x = Home.Location.X + Home.Width - button.Width;
            }
            if (Home.Location.Y > y)
            {
                y = Home.Location.Y;
            }
            else if (Home.Location.Y + Home.Height < y + button.Height)
            {
                y = Home.Location.Y + Home.Height - button.Height;
            }

            FocusObject.Location = new Point(x, y);
            FocusObject = null;
        }
    }

    private void Button_Click(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var home = Objects[btn];

        foreach (var item in RightPanel)
        {
            RemoveControl.Invoke(item);
        }
        RightPanel.Clear();

        int y = 35;
        CreatePropertyLeftPannel(btn.Text, "Name", btn.Name, y);
        y += 60;
        CreatePropertyLeftPannel(btn.Width.ToString(), "Width", btn.Name, y);
        y += 60;
        CreatePropertyLeftPannel(btn.Height.ToString(), "Height", btn.Name, y);
        y += 60;
        CreatePropertyLeftPannel(home.V.ToString(), "V", btn.Name, y);
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        var text = sender as TextBox;

        Button btn = Objects.Select(_ => _.Key).First(_ => _.Name == text!.Name);
        var home = Objects[btn];

        if (btn.Name == Home.Name)
        {
            return;
        }

        switch (text!.AccessibleName)
        {
            case "Name":
                btn.Text = $"{text!.Text}\n{home.V} м";
                break;
            case "Width":
                int width = 0;
                if (int.TryParse(text.Text, out width))
                {
                    btn.Width = width;
                }
                break;
            case "Height":
                int height = 0;
                if (int.TryParse(text.Text, out height))
                {
                    btn.Height = height;
                }
                break;
            case "V":
                int result = 0;                
                if(int.TryParse(text.Text, out result))
                {
                    home.V = result;

                    var homeObject = Objects[Home];
                    homeObject.V = Objects.Where(_ => _.Key != Home).Select(_ => _.Value.V).Sum();
                    Home.Text = $"{homeObject.V} м";
                }
                btn.Text = $"{btn!.Text.Split('\n').FirstOrDefault()}\n{home.V} м";
                break;
        }
    }

    private void CreatePropertyLeftPannel(string value, string name, string btnName, int y)
    {
        var labelName = new Label
        {
            Text = name,
            Location = new Point(930, y)
        };

        y += 30;
        RightPanel.Add(labelName);
        Redrawing.Invoke(labelName);

        var textBoxName = new TextBox()
        {
            Name = btnName,
            AccessibleName = name,
            Location = new Point(930, y),
            Text = value
        };
        textBoxName.TextChanged += TextBox_TextChanged!;
        RightPanel.Add(textBoxName);
        Redrawing.Invoke(textBoxName);
    }
}
