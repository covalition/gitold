using System.Windows;
using System.Windows.Controls;

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
                    if (h < 24 || d < 7) { // excluding h = 24 and d = 7
                        Day day = new Day();
                        day.SetValue(Grid.RowProperty, d + 1);
                        day.SetValue(Grid.ColumnProperty, h + 1);
                        day.DataContext = ((ViewModels.MainViewModel)Application.Current.MainWindow.DataContext).DayViewModels[d, h]; // if d == 8 then all days
                        grMain.Children.Add(day);
                    }
                }
            }
        }
    }
}
