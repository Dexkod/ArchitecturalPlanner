using Domain;

namespace ArchitecturalPlanner
{
    public partial class Form1 : Form
    {
        private HomeObject FocusObject;
        private ManagerHomeObjects _managerHomeObjects;
        public Form1()
        {
            InitializeComponent();
            _managerHomeObjects = new ManagerHomeObjects(this);
            _managerHomeObjects.Redrawing += Redrawing;
            _managerHomeObjects.RemoveControl += RemoveControl;
            InitHome();
            InitUpPanel();
            this.MouseMove += Form_MouseMove!;
        }

        private void Home_Click(object sender, EventArgs e)
        {

        }

        private static void Form_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void InitHome()
        {
            var homeObject = new HomeObject(20, 134, 700, 300, 0, string.Empty, Color.White);

            FocusObject = homeObject;
            var button = _managerHomeObjects.Add(homeObject, TypeHomeObject.Room);
            _managerHomeObjects.Home = button;
            Controls.Add(button);
        }

        private void InitUpPanel()
        {
            var list = _managerHomeObjects.TypeObjects;

            foreach (var obj in list)
            {
                Controls.Add(obj);
            }
        }

        private void Redrawing(Control control)
        {
            Controls.Add(control);
            control.BringToFront();
        }

        private void RemoveControl(Control control)
        {
            Controls.Remove(control);
        }
    }
}
