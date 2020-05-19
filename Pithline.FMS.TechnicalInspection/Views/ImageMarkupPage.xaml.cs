using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.TechnicalInspection.Common;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.TechnicalInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ImageMarkupPage : VisualStateAwarePage
    {

        
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        #region Private Vars
        private InkManager _inkManager;
        private uint _penID;
        private uint _touchID;
        private Windows.Foundation.Point _previousContactPt;
        private Windows.Foundation.Point currentContactPt;
        private double x1;
        private double y1;
        private double x2;
        private double y2;
        private WriteableBitmap _image;
        private string _fileName;
        private Windows.UI.Color _brushColor;
        #endregion

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        


        public ImageMarkupPage()
        {
            this.InitializeComponent();
            

            this._inkManager = new InkManager();

            //#FF003f82
            this._brushColor = Colors.Blue;
            this.Loaded += DrawImageView_Loaded;
        }

        

       



        void DrawImageView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var height = Convert.ToInt32(this.PanelCanvas.ActualHeight);
                var width = Convert.ToInt32(this.PanelCanvas.ActualWidth);

                this._image = BitmapFactory.New(width, height);
                this._image.GetBitmapContext();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region Panel Codes
        private void panelcanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Get information about the pointer location.
            PointerPoint pt = e.GetCurrentPoint(PanelCanvas);
            _previousContactPt = pt.Position;

            // Accept input only from a pen or mouse with the left button pressed. 
            PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
            if (pointerDevType == PointerDeviceType.Pen || pointerDevType == PointerDeviceType.Mouse && pt.Properties.IsLeftButtonPressed)
            {
                // Pass the pointer information to the InkManager.
                _inkManager.ProcessPointerDown(pt);
                _penID = pt.PointerId;
                e.Handled = true;
            }

            else if (pointerDevType == PointerDeviceType.Touch)
            {
                // Process touch input
                _inkManager.ProcessPointerDown(pt);
                _penID = pt.PointerId;
                e.Handled = true;
            }
        }

     async   private void panelcanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (e.Pointer.PointerId == _penID)
                {
                    Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(PanelCanvas);


                    // Pass the pointer information to the InkManager. 
                    _inkManager.ProcessPointerUp(pt);
                }

                else if (e.Pointer.PointerId == _touchID)
                {
                    // Process touch input
                    PointerPoint pt = e.GetCurrentPoint(PanelCanvas);


                    // Pass the pointer information to the InkManager. 
                    _inkManager.ProcessPointerUp(pt);
                }

                _touchID = 0;
                _penID = 0;

                var backMarkup = await ApplicationData.Current.RoamingFolder.CreateFileAsync("markupimage_" + App.Task.CaseNumber + this.listView.SelectedIndex, CreationCollisionOption.ReplaceExisting);
                if (_inkManager.GetStrokes().Count > 0)
                {

                    //buffer.Seek(0);
                    using (var os = await backMarkup.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await _inkManager.SaveAsync(os);
                    }
                }

                // Call an application-defined function to render the ink strokes.
                e.Handled = true;
            }
            catch (Exception ex)
            {
                 //new MessageDialog(ex.Message,"Error").ShowAsync();
            }
        }

        private void panelcanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId == _penID)
            {
                PointerPoint pt = e.GetCurrentPoint(PanelCanvas);
                // Render a red line on the canvas as the pointer moves. 
                // Distance() is an application-defined function that tests
                // whether the pointer has moved far enough to justify 
                // drawing a new line.
                currentContactPt = pt.Position;
                x1 = _previousContactPt.X;
                y1 = _previousContactPt.Y;
                x2 = currentContactPt.X;
                y2 = currentContactPt.Y;

                if (Distance(x1, y1, x2, y2) > 2.0)
                {
                    Line line = new Line()
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        StrokeThickness = 3.0,
                        Stroke = new SolidColorBrush(this._brushColor)
                    };
                    _previousContactPt = currentContactPt;

                    // Draw the line on the canvas by adding the Line object as
                    // a child of the Canvas object.
                    PanelCanvas.Children.Add(line);

                    // Pass the pointer information to the InkManager.
                    _inkManager.ProcessPointerUpdate(pt);
                }


                this._image.DrawLine(Convert.ToInt32(x1), Convert.ToInt32(y1), Convert.ToInt32(x2), Convert.ToInt32(y2), this._brushColor);

            }

            else if (e.Pointer.PointerId == _touchID)
            {
                // Process touch input
                PointerPoint pt = e.GetCurrentPoint(PanelCanvas);

                // Render a red line on the canvas as the pointer moves. 
                // Distance() is an application-defined function that tests
                // whether the pointer has moved far enough to justify 
                // drawing a new line.
                currentContactPt = pt.Position;
                x1 = _previousContactPt.X;
                y1 = _previousContactPt.Y;
                x2 = currentContactPt.X;
                y2 = currentContactPt.Y;

                if (Distance(x1, y1, x2, y2) > 2.0)
                {
                    Line line = new Line()
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        StrokeThickness = 3.0,
                        Stroke = new SolidColorBrush(this._brushColor)
                    };
                    _previousContactPt = currentContactPt;

                    // Draw the line on the canvas by adding the Line object as
                    // a child of the Canvas object.
                    PanelCanvas.Children.Add(line);

                    // Pass the pointer information to the InkManager.
                    _inkManager.ProcessPointerUpdate(pt);
                }

                using (this._image.GetBitmapContext())
                {
                    this._image.DrawLineAa(Convert.ToInt32(x1), Convert.ToInt32(y1), Convert.ToInt32(x2), Convert.ToInt32(y2), this._brushColor);
                }
            }
        }

        #endregion

        #region Util Functions
        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }
        #endregion

        private async void Save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var height = Convert.ToInt32(this.PanelCanvas.ActualHeight);
            var width = Convert.ToInt32(this.PanelCanvas.ActualWidth);

            //save image to buffer
            //var signBuffer = new InMemoryRandomAccessStream();
            //await this._inkManager.SaveAsync(signBuffer);
            //signBuffer.Seek(0);

            //load the two images
            //var background = await BitmapFactory.New(width, height).FromContent(new Uri("ms-appx:///Assets/drawImg1.jpg"));
            var background = await BitmapFactory.New(width, height).FromContent(new Uri(this._fileName));
            //var sign = await BitmapFactory.New(width, height).FromStream(signBuffer);

            //get context for bliting
            using (background.GetBitmapContext())
            {
                //using (this._image.GetBitmapContext())
                //{
                background.Blit
                    (
                    new Rect(0, 0, background.PixelWidth, background.PixelHeight),
                    this._image,
                    new Rect(1, 1, this._image.PixelWidth, this._image.PixelHeight),
                    WriteableBitmapExtensions.BlendMode.Alpha
                    );
                //}


                var filename = Guid.NewGuid() + ".jpg";
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            //await background.SaveToFile(file, BitmapEncoder.JpegEncoderId);
        }

      

        async private void Snapshot_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //save image to buffer
                this.Progress.Opacity = 1;
                
                //var buffer = new InMemoryRandomAccessStream();
                //await this._inkManager.SaveAsync(buffer);
               

                ClearCanvas();

                var bf = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("markupimage_" +App.Task.CaseNumber+ this.listView.SelectedIndex) as StorageFile;
                
                if (bf != null)
                {
                    using (var inputStream = await bf.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        if (inputStream.Size > 0)
                        {
                            await _inkManager.LoadAsync(inputStream);
                            foreach (var item in _inkManager.GetStrokes())
                            {
                                RenderStroke(item);
                            }
                        }
                    }
                }

                this.Progress.Opacity = 0;
            }
            catch (Exception ex)
            {
                this.Progress.Opacity = 0;

                new MessageDialog(ex.Message, "Error").ShowAsync();
            }
        }

        private void ClearCanvas()
        {
            try
            {
                _inkManager.Mode = Windows.UI.Input.Inking.InkManipulationMode.Erasing;
                var strokes = _inkManager.GetStrokes();
                for (int i = 0; i < strokes.Count; i++)
                {
                    strokes[i].Selected = true;
                }
                _inkManager.DeleteSelected();
                PanelCanvas.Children.Clear();
                _inkManager.Mode = InkManipulationMode.Inking;
            }
            catch (Exception)
            {

            }
        }

        private void RenderStroke(InkStroke stroke, double width = 3.0, double opacity = 1)
        {
            // Each stroke might have more than one segments
            var renderingStrokes = stroke.GetRenderingSegments();

            //
            // Set up the Path to insert the segments
            var path = new Windows.UI.Xaml.Shapes.Path();
            path.Data = new PathGeometry();
            ((PathGeometry)path.Data).Figures = new PathFigureCollection();

            var pathFigure = new PathFigure();
            pathFigure.StartPoint = renderingStrokes.First().Position;
            ((PathGeometry)path.Data).Figures.Add(pathFigure);

            //
            // Foreach segment, we add a BezierSegment
            foreach (var renderStroke in renderingStrokes)
            {
                pathFigure.Segments.Add(new BezierSegment()
                {
                    Point1 = renderStroke.BezierControlPoint1,
                    Point2 = renderStroke.BezierControlPoint2,
                    Point3 = renderStroke.Position
                });
            }

            // Set the general options (i.e. Width and Color)
            path.StrokeThickness = width;
            path.Stroke = new SolidColorBrush(Colors.Blue);

            // Opacity is used for highlighter
            path.Opacity = opacity;

            PanelCanvas.Children.Add(path);
        }

        async private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
           var file =await ApplicationData.Current.RoamingFolder.TryGetItemAsync("markupimage_" +App.Task.CaseNumber+ this.listView.SelectedIndex);
           if (file !=null)
           {
              await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
           }
        }

    }
}
