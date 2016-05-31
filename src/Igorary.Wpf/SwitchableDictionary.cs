using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Igorary.Wpf
{
    public class SwitchableDictionary : ResourceDictionary
    {
        public string Name { get; set; }

        private List<NamedResourceSource> _sources = new List<NamedResourceSource>();

        public List<NamedResourceSource> Sources {
            get {
                return _sources;
            }
            set {
                if(value != _sources) {
                    _sources = value;
                    setSource();
                }
            }
        }

        private void setSource() {
            if (Sources != null) {
                NamedResourceSource sourceFound = Sources.FirstOrDefault(s => s.Name == CurrentDictionaryName);
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
                    setSource();
                }
            }
        }

        public string[] GetDictionaryNames() {
            return Sources.Select(s => s.Name).ToArray();
        }
    }
}
