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
using Autodesk.Revit.DB;

namespace RevitApiOnline.DataGridLearn
{
    /// <summary>
    /// Interaction logic for DataGridWpf.xaml
    /// </summary>
    public partial class DataGridWpf : Window
    {
        private Document _doc;
        public DataGridWpf(Document doc)
        {
            InitializeComponent();
            _doc = doc;
        }

        private void comboboxFamilyChanged(object sender, SelectionChangedEventArgs e)
        {
            var indexRow = dataGridFamilyBeam.SelectedIndex;
            System.Windows.Controls.ComboBox combobox = sender as System.Windows.Controls.ComboBox;
            if (combobox.SelectedItem != null)
            {
                BeamFamilyVM selectedFamily = combobox.SelectedItem as BeamFamilyVM;
                Family familySelected = DataGridAppShow.ListFamilyBeam.First(x => x.Id == selectedFamily.FamilyId);
                List<FamilySymbolVM> listtSymbolVm = new List<FamilySymbolVM>();
                foreach(ElementId symbolId in familySelected.GetFamilySymbolIds())
                {
                    FamilySymbol symbol = _doc.GetElement(symbolId) as FamilySymbol;
                    FamilySymbolVM symboVm=new FamilySymbolVM();
                    symboVm.TypeId = symbol.Id;
                    symboVm.TypeName= symbol.Name;
                    listtSymbolVm.Add(symboVm);
                }
                List<DataGridItem> dataSources = dataGridFamilyBeam.ItemsSource as List<DataGridItem>;
                dataSources[indexRow].FamilySymbols= listtSymbolVm;
                dataSources[indexRow].SelectedSymbol = listtSymbolVm[0];
            }
        }

        private void btnOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
