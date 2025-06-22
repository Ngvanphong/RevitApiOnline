using Autodesk.Revit.DB;
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

namespace RevitApiOnline.Wpf
{
    /// <summary>
    /// Interaction logic for GridLearn.xaml
    /// </summary>
    public partial class GridLearn : Window
    {
        public GridLearn()
        {
            InitializeComponent();
        }

        private void textBoxValueChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox= sender as System.Windows.Controls.TextBox;
            string valueText = textBox.Text;
        }

        private void btnClickOk(object sender, RoutedEventArgs e)
        {
            string valueText = textBoxDemo.Text;
            bool isCheckAll = checkBoxAll.IsChecked==true;
            bool isCheckMade= checkBoxMade.IsChecked==true;
            bool isCheckFeMade= checkBoxFeMale.IsChecked==true;

            
        }
    }
}
