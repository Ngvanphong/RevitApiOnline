using RevitApiOnline.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace RevitApiOnline.CreatePiping
{
    /// <summary>
    /// Interaction logic for CanvasColumnWpf.xaml
    /// </summary>
    public partial class CanvasColumnWpf : Window, INotifyPropertyChanged
    {

        private double scaleCanvas;
        public double ScaleCanvas
        {
            get { return scaleCanvas; }
            set { scaleCanvas = value; OnPropertyChanged(nameof(ScaleCanvas)); }
        }

        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;

        public CanvasColumnWpf()
        {
            InitializeComponent();
            this.DataContext = this;
            CoordinateCanvasHelper.GetMinMaxBoundary(ColumnCanvasAppShow.listColumnCurves, out xMin, out xMax, out yMin, out yMax);

        }

        private void CreateCurveLoopCanvas()
        {
            double thickness = 30 * (1 - ScaleCanvas);
            foreach (var listLine in ColumnCanvasAppShow.listColumnCurves)
            {
                foreach (var line in listLine)
                {
                    CoordinateCanvasHelper.CreateLineFromLineRevit(columnCanvas, line, xMin, xMax, yMin, yMax, thickness);
                }
            }
        }

        private void SetScaleCanvas()
        {
            double widthScrollView = scrollView.ViewportWidth;
            double heightScrollView = scrollView.ViewportHeight;
            double scaleX = widthScrollView / ((xMax - xMin) * 304.8 + 0.2 * (xMax - xMin) * 304.8);
            double scaleY = heightScrollView / ((yMax - yMin) * 304.8 + 0.2 * (yMax - yMin) * 304.8);
            if(scaleX > scaleY)
            {
                ScaleCanvas= scaleY - 0.1 * scaleY;
            }
            else
            {
                ScaleCanvas = scaleX- 0.1 * scaleX;
            }
        }

        private void scrollView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void scrollView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void scrollView_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void scrollView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void window_loaded(object sender, RoutedEventArgs e)
        {
            SetScaleCanvas();
            CreateCurveLoopCanvas();
        }
    }
}
