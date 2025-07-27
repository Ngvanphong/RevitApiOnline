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
    internal class DemoCombineButton
    {
        public void DemoCombine(UIControlledApplication app)
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


            string buttonId1 = $"CreatePipe11{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData1 = new PushButtonData(buttonId1, "Create Piping",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePipeBinding).FullName);
            pushButtonData1.LongDescription = "Long Desciption 1";
            pushButtonData1.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData1.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));


            string buttonId2 = $"CreatePipe12{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData2 = new PushButtonData(buttonId2, "Create Piping 2",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData2.LongDescription = "Long Desciption 2";
            pushButtonData2.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData2.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));

            string buttonId3 = $"CreatePipe13{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData3 = new PushButtonData(buttonId3, "Create Piping 3",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData3.LongDescription = "Long Desciption 3";
            pushButtonData3.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData3.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));

            string nameSplitButton = "SplitButton224";
            SplitButtonData splitButtonData = new SplitButtonData(nameSplitButton, "Split Button");
            splitButtonData.LongDescription = "Split Btn";
            splitButtonData.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            splitButtonData.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));


            // smal button 
            string buttonId4 = $"CreatePipe14{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData4 = new PushButtonData(buttonId4, "Create Piping 4",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData4.LongDescription = "Long Desciption 3";
            pushButtonData4.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData4.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));

            string buttonId5 = $"CreatePipe14{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData5 = new PushButtonData(buttonId5, "Create Piping 5",
                Assembly.GetExecutingAssembly().Location, typeof(CreatePineNewBinding).FullName);
            pushButtonData5.LongDescription = "Long Desciption 3";
            pushButtonData5.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData5.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));


            IList<RibbonItem> listStackItem= ribbonPanel.AddStackedItems(splitButtonData,pushButtonData4,pushButtonData5);
            foreach(RibbonItem ribbonItem in listStackItem)
            {
                if(ribbonItem is SplitButton)
                {
                    SplitButton splitButton = (SplitButton)ribbonItem;
                    if (splitButton.Name == nameSplitButton)
                    {
                        splitButton.AddPushButton(pushButtonData1);
                        splitButton.AddPushButton(pushButtonData2);
                        splitButton.AddPushButton(pushButtonData3);
                    }
                }
            }
        }
    }
}
