using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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

namespace RevitApiOnline.ListBox
{
    /// <summary>
    /// Interaction logic for ListBoxWpf.xaml
    /// </summary>
    public partial class ListBoxWpf : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public ListBoxWpf(List<WallInfo> listWallInfo)
        {
            InitializeComponent();

            /// gian item source
            listViewWall.ItemsSource = listWallInfo;
            ///


            CollectionView collectionView = CollectionViewSource.GetDefaultView(listViewWall.ItemsSource) as CollectionView;
            PropertyGroupDescription propertyGroupDescription= new PropertyGroupDescription("NameWall");
            collectionView.GroupDescriptions.Add(propertyGroupDescription);
            //SortDescription sortDescription = new SortDescription("LevelName", ListSortDirection.Ascending);
            //collectionView.SortDescriptions.Add(sortDescription);

        }

        private void btnOk(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void listViewChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (sender as System.Windows.Controls.CheckBox);
            if (checkbox.IsChecked == true)
            {
                var selectedItems = listViewWall.SelectedItems;
                if (selectedItems != null)
                {
                    foreach(WallInfo wallInfo in listViewWall.SelectedItems)
                    {
                        wallInfo.IsChecked = true;
                    }
                }
            }

        }

        private void listViewUnChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (sender as System.Windows.Controls.CheckBox);
            if (checkbox.IsChecked == false)
            {
                var selectedItems = listViewWall.SelectedItems;
                if (selectedItems != null)
                {
                    foreach (WallInfo wallInfo in listViewWall.SelectedItems)
                    {
                        wallInfo.IsChecked = false;
                    }
                }
            }
        }

        private void levelHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            GridViewColumnHeader gridViewColumHeader= sender as GridViewColumnHeader;
            if(listViewSortCol!=null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                listViewWall.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            listViewWall.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
