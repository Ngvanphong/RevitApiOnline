using Autodesk.Revit.UI;
using RevitApiOnline.CreaetWallByPoint;
using RevitApiOnline.CreatePiping;
using RevitApiOnline.FamilyChange;
using RevitApiOnline.PutAirTerminal;
using RevitApiOnline.ReadFileData;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using UIFramework;
using adwindow = Autodesk.Windows;

namespace RevitApiOnline.Buttons
{
    public class CreatePipeButton
    {
        public void CreatePipe(UIControlledApplication app)
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

            string buttonId = $"CreateWall{new Random().Next(1, 9000).ToString()}";
            PushButtonData pushButtonData = new PushButtonData(buttonId, "Read Data",
                Assembly.GetExecutingAssembly().Location, typeof(ReadCadBinding).FullName);
            pushButtonData.LongDescription = "Long Desciption";
            pushButtonData.Image = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));
            pushButtonData.LargeImage = new BitmapImage(new Uri("/RevitApiOnline;component/Images/icons8crop24.png", UriKind.RelativeOrAbsolute));

            PushButton pushButton = ribbonPanel.AddItem(pushButtonData) as PushButton;
            pushButton.Enabled = true;
        }
    }
}