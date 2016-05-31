using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Igorary.Wpf
{
    public static class ApplicationExtensions
    {
        public static void SwitchResources(this Application app, string switchableDictionaryName, string currentDictionaryName) {
            app.Resources.MergedDictionaries.OfType<SwitchableDictionary>().First(rd => rd.Name == switchableDictionaryName).CurrentDictionaryName = currentDictionaryName;
        }

        public static string[] GetResourceNames(this Application app, string switchableDictionaryName) {
            return app.Resources.MergedDictionaries.OfType<SwitchableDictionary>().First(sd => sd.Name == switchableDictionaryName).GetDictionaryNames();
        }
    }
}
