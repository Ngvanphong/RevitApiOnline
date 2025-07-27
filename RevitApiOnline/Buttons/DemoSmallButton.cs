using Autodesk.Revit.UI;
using RevitApiOnline.CreatePiping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UIFramework;
using adwindow = Autodesk.Windows;

namespace RevitApiOnline.Buttons
{
    internal class DemoSmallButton
    {
        public void DemoSmall(UIControlledApplication app)
        {
            adwindow.RibbonTab ribbonTab = null;
            adwindow.RibbonControl ribbonControl = RevitRibbonControl.RibbonControl;
            foreach (var tab in ribbonControl.Tabs)
            {
                if (tab.AutomationName == AppConstants.TabName)
                {
                    ribbonTab = tab;
                    break;
                }
            }
            if (ribbonTab == null)
            {
                //create ribbon tab
                app.CreateRibbonTab(AppConstants.TabName);
            }

            RibbonPanel ribbonPanel = null;
            foreach (RibbonPanel panel in app.GetRibbonPanels(AppConstants.TabName))
            {
                if (panel.Name == AppConstants.PanelName)
                {
                    ribbonPanel = panel;
                    break;
                }
            }

            if (ribbonPanel == null)
            {
                ribbonPanel = app.CreateRibbonPanel(AppConstants.TabName, AppConstants.PanelName);
            }


            string buttonId1 = $"CreatePipe8{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData1 = new PushButtonData(buttonId1, "Create Piping",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePipeBinding).FullName);
            pushButtonData1.LongDescription = "Long Desciption 1";
            pushButtonData1.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));
            pushButtonData1.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));


            string buttonId2 = $"CreatePipe9{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData2 = new PushButtonData(buttonId2, "Create Piping 2",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData2.LongDescription = "Long Desciption 2";
            pushButtonData2.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));
            pushButtonData2.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));


            string buttonId3 = $"CreatePipe10{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData3 = new PushButtonData(buttonId3, "Create Piping 3",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData3.LongDescription = "Long Desciption 3";
            pushButtonData3.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));
            pushButtonData3.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop16.png", UriKind.RelativeOrAbsolute));

            ribbonPanel.AddStackedItems(pushButtonData1,pushButtonData2,pushButtonData3);

        }
    }
}
