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

                for (int d = 0; d < 8; d++)
                    for (int h = 0; h < 25; h++) {
                        DayViewModel dayViewModel = DayViewModels[d, h];

                        if (d < 7 || h < 24) { // excluding h = 24 and d = 7
                            int v;
                            if (d == 7) {
                                v = 0;
                            }
                            else
                            if (h == 24) {
                                v = 0;
                            }
                            else
                                v = _values[d, h];
                            dayViewModel.Value = v;
                            dayViewModel.Percent = max != 0 ? v / max : 0.0;
                        }
                    }

                //int[] diameters = new int[7 * 24];
                //for (int x = 0; x < 7 * 24; x++)
                //    diameters[x] = (int)(((double)_values[x] / max) * 20); // http://stackoverflow.com/questions/717299/wpf-setting-the-width-and-height-as-a-percentage-value
                //Diameters = diameters;


                Properties.Settings.Default.LocalRepoPath = Path;
                Properties.Settings.Default.Save();
            }
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

        private int[] _diameters;

        public int[] Diameters {
            get {
                return _diameters;
            }
            set {
                if (value != _diameters) {
                    _diameters = value;
                    RaisePropertyChanged(() => Diameters);
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

        ///// <param name="day">0..6, or 7 = all days</param>
        ///// <param name="hour">0..23, or 24 = all hours</param>
        ///// <returns></returns>
        //internal DayViewModel GetDayViewModel(int day, int hour) {
        //    return getDayViewModels()[day, hour];
        //}

        //DayViewModel[,] dayViewModels;

        //private DayViewModel[,] getDayViewModels() {
        //    throw new Exception();
        //}

        //private int[] _values;

        //public int[] Values {
        //    get {
        //        return _values;
        //    }
        //    set {
        //        if (value != _values) {
        //            _values = value;
        //            RaisePropertyChanged(() => Values);
        //        }
        //    }
        //}
    }
}
