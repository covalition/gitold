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

            for (int h = 0; h < 25; h++) {

                // labels
                TextBlock tb = new TextBlock();
                if (h < 24)
                    tb.Text = h.ToString();
                else
                    tb.Text = "All Hours";
                tb.SetValue(Grid.ColumnProperty, h + 1);
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grMain.Children.Add(tb);

                for (int d = 0; d < 8; d++) {
                    //Ellipse e = new Ellipse();
                    //e.Stretch = Stretch.None;
                    //e.SetValue(Grid.RowProperty, j + 1);
                    //e.SetValue(Grid.ColumnProperty, i + 1);
                    //Binding binding = new Binding(string.Format("Diameters[{0}]", j * 24 + i));
                    //e.SetBinding(Ellipse.WidthProperty, binding);
                    //e.SetBinding(Ellipse.HeightProperty, binding);
                    //e.Fill = new SolidColorBrush(Colors.Black);
                    //binding = new Binding(string.Format("Values[{0}]", j * 24 + i));
                    //e.SetBinding(Ellipse.ToolTipProperty, binding);
                    //e.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    //e.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    if (h < 24 || d < 7) { // excluding h = 24 and d = 7
                        Day day = new Day();
                        day.SetValue(Grid.RowProperty, d + 1);
                        day.SetValue(Grid.ColumnProperty, h + 1);
                        day.DataContext = ((ViewModels.MainViewModel)Application.Current.MainWindow.DataContext).DayViewModels[d, h]; // if d == 8 then all days
                        grMain.Children.Add(day);
                    }
                }
            }

            // All hours
            //tb = new TextBlock();
            //tb.Text = "All Hours";
            //tb.SetValue(Grid.ColumnProperty, 25);
            //tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //grMain.Children.Add(tb);


        }
    }
}
