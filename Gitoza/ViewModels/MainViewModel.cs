using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Gitoza.ViewModels
{
    using BusinessLogic;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel() {
            Path = Properties.Settings.Default.LocalRepoPath;
        }

        #region Refresh command

        private RelayCommand _refresh;

        public RelayCommand Refresh {
            get {
                return _refresh ?? (_refresh = new RelayCommand(refreshAction, refreshCanExecute));
            }
        }

        private void refreshAction() {
            try {
                int[,] _values = DomainFacade.GetCommitCounts(Path);
                int max = _values.Cast<int>().Max();

                for (int d = 0; d < 8; d++) {
                    DayViewModels[d, 24].Value = 0; // clear sum

                    for (int h = 0; h < 25; h++) {
                        if (d == 0)
                            DayViewModels[7, h].Value = 0; // clear sum

                        DayViewModel dayViewModel = DayViewModels[d, h];

                        if (d < 7 && h < 24) {
                            dayViewModel.Value = _values[d, h];
                            DayViewModels[d, 24].Value += _values[d, h];
                            DayViewModels[7, h].Value += _values[d, h];
                            dayViewModel.Percent = max != 0 ? (double)dayViewModel.Value / max : 0.0;
                        }
                        else
                            dayViewModel.Percent = 0;
                    }
                }
                Properties.Settings.Default.LocalRepoPath = Path;
                Properties.Settings.Default.Save();
            }

            //int[] diameters = new int[7 * 24];
            //for (int x = 0; x < 7 * 24; x++)
            //    diameters[x] = (int)(((double)_values[x] / max) * 20); // http://stackoverflow.com/questions/717299/wpf-setting-the-width-and-height-as-a-percentage-value
            //Diameters = diameters;


            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private bool refreshCanExecute() {
            return true;
        }

        #endregion

        private string _path;

        public string Path {
            get {
                return _path;
            }
            set {
                if (value != _path) {
                    _path = value;
                    RaisePropertyChanged(() => Path);
                }
            }
        }

        DayViewModel[,] _dayViewModels;

        public DayViewModel[,] DayViewModels {
            get {
                if (_dayViewModels == null) {
                    _dayViewModels = new DayViewModel[8, 25];
                    // initialize the array
                    for (int d = 0; d < 8; d++)
                        for (int h = 0; h < 25; h++)
                            _dayViewModels[d, h] = new DayViewModel();
                }
                return _dayViewModels;
            }
        }
    }
}
