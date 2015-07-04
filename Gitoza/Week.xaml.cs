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

namespace Gitoza
{
    /// <summary>
    /// Interaction logic for Week.xaml
    /// </summary>
    public partial class Week : UserControl
    {
        public Week() {
            InitializeComponent();

            for (int i = 1; i <= 24; i++) {
                for (int j = 1; j <= 7; j++) {
                    Ellipse e = new Ellipse();
                    e.SetValue(Grid.RowProperty, j);
                    e.SetValue(Grid.ColumnProperty, i);
                    Binding binding = new Binding(string.Format("Diameters[{0}][{1}]", j, i));
                    e.SetBinding(Ellipse.WidthProperty, binding);
                    e.SetBinding(Ellipse.HeightProperty, binding);
                    e.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    e.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    grMain.Children.Add(e);
                }
                TextBlock tb = new TextBlock();
                tb.Text = (i - 1).ToString();
                tb.SetValue(Grid.ColumnProperty, i);
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grMain.Children.Add(tb);
            }
        }
    }
}
