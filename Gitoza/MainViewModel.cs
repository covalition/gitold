using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Gitoza
{
    public class MainViewModel: ViewModelBase
    {
        #region Refresh command

        private RelayCommand _refresh;

        public RelayCommand Refresh {
            get {
                return _refresh ?? (_refresh = new RelayCommand(refreshAction, refreshCanExecute));
            }
        }

        private void refreshAction() {
            try {
                Values = DomainFacade.GetCommitCounts(Path);
                int max = _values.Cast<int>().Max();
                if (max != 0) {
                    int[,] diameters = new int[7, 24];
                    for (int i = 0; i < 24; i++)
                        for (int j = 0; j < 7; j++) {
                            diameters[j, i] = (int)(((double)_values[j, i] / max) * 20);
                        }
                    Diameters = diameters;
                }
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

        private int[,] _diameters;

        public int[,] Diameters {
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

        private int[,] _values;

        public int[,] Values {
            get {
                return _values;
            }
            set {
                if (value != _values) {
                    _values = value;
                    RaisePropertyChanged(() => Values);
                }
            }
        }
    }
}
