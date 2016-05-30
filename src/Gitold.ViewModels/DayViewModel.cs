using GalaSoft.MvvmLight;

namespace Gitold.ViewModels
{
    public class DayViewModel: ViewModelBase
    {
        private double _percent;

        /// <summary>
        /// 0..1
        /// </summary>
        public double Percent {
            get { return _percent;  }
            set {
                if(value != _percent) {
                    _percent = value;
                    RaisePropertyChanged(() => Percent);
                }
            }
        }


        private int _value;

        public int Value {
            get {
                return _value;
            }
            set {
                if (value != _value) {
                    _value = value;
                    RaisePropertyChanged(() => Value);
                }
            }
        }

        private bool _isSummary;

        public bool IsSummary {
            get {
                return _isSummary;
            }
            set {
                if (value != _isSummary) {
                    _isSummary = value;
                    RaisePropertyChanged(() => IsSummary);
                }
            }
        }
        
    }
}