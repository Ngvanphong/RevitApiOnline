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

namespace RevitApiOnline.CreaetWallByPoint
{
    /// <summary>
    /// Interaction logic for CreateWallWpf.xaml
    /// </summary>
    public partial class CreateWallWpf : Window
    {
        private readonly ExternalEvent _startPointClickEvent;
        private readonly ExternalEvent _endPointClickEvent;
        private readonly ExternalEvent _drawWallEvent;
        public CreateWallWpf(ExternalEvent startPointClickEvent, ExternalEvent endPointClickEvent, ExternalEvent drawWallEvent)
        {
            InitializeComponent();
            _startPointClickEvent = startPointClickEvent;
            _endPointClickEvent = endPointClickEvent;
            _drawWallEvent = drawWallEvent;
        }

        private void btnStartClick(object sender, RoutedEventArgs e)
        {
            _startPointClickEvent.Raise();

        }

        private void btnEndClick(object sender, RoutedEventArgs e)
        {
            _endPointClickEvent.Raise();
        }

        private void btnDraw(object sender, RoutedEventArgs e)
        {
            _drawWallEvent.Raise();
        }
    }
}
