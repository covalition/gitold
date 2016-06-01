using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Igorary.Wpf
{
    public class SwitchableDictionary : ResourceDictionary
    {
        public string Name { get; set; }

        private ObservableCollection<NamedResourceSource> _sources; // = new List<NamedResourceSource>();

        public ObservableCollection<NamedResourceSource> Sources {
            get {
                if(_sources == null) {
                    _sources = new ObservableCollection<NamedResourceSource>();
                    _sources.CollectionChanged += _sources_CollectionChanged;
                }
                return _sources;
            }
            //set {
            //    if(value != _sources) {
            //        _sources = value;
            //        setSource();
            //    }
            //}
        }

        private void _sources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            updateSource();
        }

        private void updateSource() {
            if (_sources != null && _currentDictionaryName != null) {
                NamedResourceSource sourceFound = _sources.FirstOrDefault(s => s.Name == _currentDictionaryName);
                if(sourceFound != null)
                    Source = sourceFound.Source;
            }
        }

        private string _currentDictionaryName;

        public string CurrentDictionaryName {
            get {
                return _currentDictionaryName;
            }
            set {
                if(value != _currentDictionaryName) {
                    _currentDictionaryName = value;
                    updateSource();
                }
            }
        }

        public string[] GetDictionaryNames() {
            return Sources.Select(s => s.Name).ToArray();
        }
    }
}
