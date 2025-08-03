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
using Autodesk.Revit.UI;

namespace RevitApiOnline.PutAirTerminal
{
    /// <summary>
    /// Interaction logic for PutAirTerminalWpf.xaml
    /// </summary>
    public partial class PutAirTerminalWpf : Window
    {
        private ExternalEvent _familyTypeEvent;
        private ExternalEvent _putAirEvent;
        public PutAirTerminalWpf(ExternalEvent familyTypeEvent, ExternalEvent putAirEvent)
        {
            InitializeComponent();
            _familyTypeEvent = familyTypeEvent;
            _putAirEvent = putAirEvent;
        }

        private void btnPutTeminal(object sender, RoutedEventArgs e)
        {
            _putAirEvent.Raise();
        }

        private void comboboxFamilyChanged(object sender, SelectionChangedEventArgs e)
        {
            _familyTypeEvent.Raise();
        }
    }
}
