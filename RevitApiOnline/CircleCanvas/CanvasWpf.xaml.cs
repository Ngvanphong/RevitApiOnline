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

namespace RevitApiOnline.CircleCanvas
{
    /// <summary>
    /// Interaction logic for CanvasWpf.xaml
    /// </summary>
    public partial class CanvasWpf : Window
    {
        public CanvasWpf()
        {
            InitializeComponent();
        }

        private void textTextChanged(object sender, TextChangedEventArgs e)
        {
            if(canvas!=null && canvas.Children != null)
            {
                this.canvas.Children.Clear();
                int.TryParse(txtRebarNumber.Text, out int numberRebar);
                CanvasRebarHelper.CreateRebarCanvas(numberRebar, 150 - 20, canvas);
            }
           
        }
    }
}
