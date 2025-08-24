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
using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.UI;

namespace RevitApiOnline.FamilyChange
{
    /// <summary>
    /// Interaction logic for ChangeFamilyTypeWpf.xaml
    /// </summary>
    public partial class ChangeFamilyTypeWpf : Window
    {
        private ExternalEvent _familyCategoryEvent;
        private ExternalEvent _getTypeEvent;
        private ExternalEvent _changeTypeEvent;
        public ChangeFamilyTypeWpf(ExternalEvent familyCategoryEvent, ExternalEvent getTypeEvent,
            ExternalEvent changeTypeEvent)
        {
            InitializeComponent();
            _familyCategoryEvent = familyCategoryEvent;
            _getTypeEvent= getTypeEvent;
            _changeTypeEvent = changeTypeEvent;
        }

        private void familyCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _familyCategoryEvent.Raise();
        }

        private void comboboxFamilyChanged(object sender, SelectionChangedEventArgs e)
        {
            
            _getTypeEvent.Raise();
        }

        private void btnAddClick(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as GridDataContext;
            FamilyOriginTarget familyOriginTarget = new FamilyOriginTarget();
            familyOriginTarget.Families = ChangeFamilyTypeAppShow.listFamilyVm;
            dataContext.ObservableFamilyOriginTarget.Add(familyOriginTarget);
        }
    }
}
