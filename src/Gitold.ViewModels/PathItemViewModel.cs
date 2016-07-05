using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Gitold.ViewModels
{
    public class PathItemViewModel: ViewModelBase
    {
        private string _caption;

        public string Caption {
            get {
                return _caption;
            }
            set {
                if (value != _caption) {
                    _caption = value;
                    RaisePropertyChanged(nameof(Caption));
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected {
            get {
                return _isSelected;
            }
            set {
                if (value != _isSelected) {
                    _isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }
    }
}
