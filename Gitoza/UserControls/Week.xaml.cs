using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gitoza.UserControls
{
    /// <summary>
    /// Interaction logic for Week.xaml
    /// </summary>
    public partial class Week : UserControl
    {
        public Week() {
            InitializeComponent();

            for (int i = 0; i < 24; i++) {
                for (int j = 0; j < 7; j++) {
                    Ellipse e = new Ellipse();
                    e.SetValue(Grid.RowProperty, j + 1);
                    e.SetValue(Grid.ColumnProperty, i + 1);
                    Binding binding = new Binding(string.Format("Diameters[{0}]", j * 24 + i));
                    e.SetBinding(Ellipse.WidthProperty, binding);
                    e.SetBinding(Ellipse.HeightProperty, binding);
                    e.Fill = new SolidColorBrush(Colors.Black);
                    binding = new Binding(string.Format("Values[{0}]", j * 24 + i));
                    e.SetBinding(Ellipse.ToolTipProperty, binding);
                    e.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    e.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    grMain.Children.Add(e);
                }
                TextBlock tb = new TextBlock();
                tb.Text = i.ToString();
                tb.SetValue(Grid.ColumnProperty, i + 1);
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grMain.Children.Add(tb);
            }
        }
    }
}
