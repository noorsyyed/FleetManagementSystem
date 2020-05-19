
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using System.Collections.Generic;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.TechnicalInspection.UILogic
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SnapshotsViewer : ContentDialog
    {
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>


        private TranslateTransform ct;
        private bool isImageDeleted;
        private double originalY;
        private double originalX;
        private Image img;

        public SnapshotsViewer()
        {
            this.InitializeComponent();
            //CrossSlideThresholds cst = new CrossSlideThresholds();
            //cst.RearrangeStart = 10;
            //cst.SelectionStart = 12;
            //cst.SpeedBumpStart = 12;
            //cst.SpeedBumpEnd = 24;

            //GestureRecognizer gr = new GestureRecognizer();
            //gr.GestureSettings = GestureSettings.CrossSlide;
            //gr.CrossSlideHorizontally = true;
            //gr.CrossSlideThresholds = cst;

            //gr.CrossSliding += new TypedEventHandler<GestureRecognizer, CrossSlidingEventArgs>(gr_CrossSliding);

        }

        //void gr_CrossSliding(GestureRecognizer sender, CrossSlidingEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}



        async public void Open(MaintenanceRepair selectedMaintenanceRepair, Guid guid)
        {
            this.DataContext = selectedMaintenanceRepair;
            if (selectedMaintenanceRepair.IsMajorPivot)
            {

                fvSnaps.ItemsSource = selectedMaintenanceRepair.MajorComponentImgList;
                fvSnaps.SelectedItem = selectedMaintenanceRepair.MajorComponentImgList.FirstOrDefault(f => f.guid == guid);
            }
            else
            {
                fvSnaps.ItemsSource = selectedMaintenanceRepair.SubComponentImgList;
                fvSnaps.SelectedItem = selectedMaintenanceRepair.SubComponentImgList.FirstOrDefault(f => f.guid == guid);
            }
            await this.ShowAsync();
        }


        private void Image_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            img = sender as Image;
            ct = img.RenderTransform as TranslateTransform;

            if (ct == null) return;

            if (e.Delta.Translation.Y == 0)
            {
                var items = (ObservableCollection<ImageCapture>)fvSnaps.ItemsSource;
                var selectedItem = fvSnaps.SelectedItem as ImageCapture;
                ct.X = originalX;
                ct.Y = originalY;

                if (items != null && selectedItem != null)
                {
                    if (e.Delta.Translation.X > 0 && fvSnaps.SelectedIndex > 0)
                    {
                        fvSnaps.SelectedItem = items[items.IndexOf(selectedItem) - 1];
                    }
                    else if ((e.Delta.Translation.X < 0) && fvSnaps.SelectedIndex < items.Count - 1)
                    {
                        fvSnaps.SelectedItem = items[items.IndexOf(selectedItem) + 1];
                    }

                }

            }

            if (e.Delta.Translation.X == 0)
            {
                ct.Y += e.Delta.Translation.Y;
                if (ct.Y > 400)
                {
                    var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                    var ic = fvSnaps.SelectedItem as ImageCapture;
                    snaps.Remove(ic);
                    isImageDeleted = true;
                    this.Hide();
                }
                else if (ct.Y < -400)
                {
                    var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                    var ic = fvSnaps.SelectedItem as ImageCapture;
                    snaps.Remove(ic);
                    isImageDeleted = true;
                    this.Hide();
                }

            }
        }

        private void Image_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!isImageDeleted)
            {
                ct.X = originalX;
                ct.Y = originalY;
                e.Handled = true;
            }
        }
        private void Image_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            img = sender as Image;
            ct = img.RenderTransform as TranslateTransform;
            originalX = ct.X;
            originalY = ct.Y;

        }

    }
}
