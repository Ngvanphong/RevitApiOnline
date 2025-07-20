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
        public DataGridWpf(Document doc, List<TreeViewItemVm> listTreeViewItem)
        {
            InitializeComponent();
            //MadePerson person1 = new MadePerson();
            //person1.Name = "N V A1";
            //MadePerson person2 = new MadePerson();
            //person2.Name = "N N A1.1";
            //MadePerson person3 = new MadePerson();
            //person3.Name = "N N A1.2";
            //person1.Persons.Add(person2);
            //person1.Persons.Add(person3);   

           
            //MadePerson person4 = new MadePerson();
            //person4.Name = "N N A2";
            //MadePerson person5 = new MadePerson();
            //person5.Name = "N N A2.1";
            //person4.Persons.Add(person5);

            //person2.Persons.Add(person4);

            //List<MadePerson> listAllPerson = new List<MadePerson>();
            //listAllPerson.Add(person1);
            //listAllPerson.Add(person4);

            //treeViewPerson.ItemsSource= listAllPerson;
            foreach(TreeViewItemVm item in listTreeViewItem)
            {
                treeViewPerson.Items.Add(item);
            }
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
