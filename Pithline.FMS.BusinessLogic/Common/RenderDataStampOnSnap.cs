using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.WIC;
using System;
//using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using D2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using WicPixelFormat = SharpDX.WIC.PixelFormat;

namespace Pithline.FMS.BusinessLogic
{
    public class RenderDataStampOnSnap
    {
      //  private static SharpDX.Direct2D1.DeviceContext d2dContext;
        public async static Task<MemoryStream> RenderStaticTextToBitmap(StorageFile imageFile)
        {
            var bitmap = new BitmapImage();
            using (var strm = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmap.SetSource(strm);
            }
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixelFormat = WicPixelFormat.Format32bppBGR;

            var wicFactory = new ImagingFactory2();
            var dddFactory = new SharpDX.Direct2D1.Factory();
            var dwFactory = new SharpDX.DirectWrite.Factory();
            WicRenderTarget renderTarget;
            Bitmap wicBitmap;

            using (var bitmapSource = LoadBitmap(wicFactory, imageFile.Path))
            {
                wicBitmap = new Bitmap(wicFactory, bitmapSource, BitmapCreateCacheOption.CacheOnLoad);

                int pixelWidth = (int)(wicBitmap.Size.Width * DisplayProperties.LogicalDpi / 96.0);
                int pixelHeight = (int)(wicBitmap.Size.Height * DisplayProperties.LogicalDpi / 96.0);

                var renderTargetProperties = new RenderTargetProperties(RenderTargetType.Default,
                    new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), 0, 0, RenderTargetUsage.None,
                    SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT);

                renderTarget = new WicRenderTarget(
                dddFactory,
                wicBitmap,
                renderTargetProperties)
                {
                    TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype
                };
            }

            renderTarget.BeginDraw();

            var textFormat = new TextFormat(dwFactory, "Segoe UI Light", 25)
            {
                TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Far
            };
            var textBrush = new SharpDX.Direct2D1.SolidColorBrush(
                renderTarget,
                SharpDX.Color.DarkBlue);

            StringBuilder sb = new StringBuilder();
            var dstamp = StampPersistData.Instance.DataStamp;
            sb.Append(dstamp.KMReading).Append("\n");
            sb.Append(dstamp.DateOfFirstReg).Append("\n");

            sb.Append(dstamp.Gps).Append("\n");
            sb.Append(dstamp.VehRegNo).Append("\n");
            sb.Append(dstamp.Make).Append("\n");

            sb.Append(dstamp.CusName).Append("\n");
            sb.Append(dstamp.InspectorName).Append("\n");
            sb.Append(dstamp.CaseNo);



            renderTarget.DrawText(
                sb.ToString(),
                textFormat,
                new SharpDX.RectangleF(1, 1, width + 50, height + 25),
                textBrush);

            //new RectangleF(width - 150, 0, width, height + 25),

            renderTarget.EndDraw();

            var ms = new MemoryStream();

            var stream = new WICStream(
                wicFactory,
                ms);

            BitmapEncoder encoder = null;
            if (imageFile.FileType == ".png")
                encoder = new PngBitmapEncoder(wicFactory);
            else if (imageFile.FileType == ".jpg")
                encoder = new JpegBitmapEncoder(wicFactory);

            encoder.Initialize(stream);

            var frameEncoder = new BitmapFrameEncode(encoder);
            frameEncoder.Initialize();
            frameEncoder.SetSize(width, height);
           // frameEncoder.PixelFormat = WicPixelFormat.FormatDontCare;
            frameEncoder.WriteSource(wicBitmap);
            frameEncoder.Commit();

            encoder.Commit();

            frameEncoder.Dispose();
            encoder.Dispose();
            stream.Dispose();

            ms.Position = 0;
            return ms;
        }



        public async static Task<MemoryStream> InterpolateImageMarkup(StorageFile imageFile, StorageFile markupFile)
        {
            try
            {
                var bitmap = new BitmapImage();
                using (var strm = await imageFile.OpenAsync(FileAccessMode.Read))
                {
                    bitmap.SetSource(strm);
                }
                var width = bitmap.PixelWidth;
                var height = bitmap.PixelHeight;
                var pixelFormat = WicPixelFormat.Format32bppBGR;

                var wicFactory = new ImagingFactory2();
                var markupFactory = new ImagingFactory2();
                var dddFactory = new SharpDX.Direct2D1.Factory();
                var dwFactory = new SharpDX.DirectWrite.Factory();
                WicRenderTarget renderTarget;
                Bitmap wicBitmap;


                SharpDX.Direct3D11.Device defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport);


                var device = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
                SharpDX.DXGI.Device2 dxgiDevice2 = device.QueryInterface<SharpDX.DXGI.Device2>();
                SharpDX.Direct2D1.Device d2dDevice = new SharpDX.Direct2D1.Device(dxgiDevice2);
               // d2dContext = new SharpDX.Direct2D1.DeviceContext(d2dDevice, SharpDX.Direct2D1.DeviceContextOptions.None);


                using (var bitmapSource = LoadBitmap(wicFactory, imageFile.Path))
                {
                    wicBitmap = new Bitmap(wicFactory, bitmapSource, BitmapCreateCacheOption.CacheOnLoad);

                    int pixelWidth = (int)(wicBitmap.Size.Width * DisplayProperties.LogicalDpi / 96.0);
                    int pixelHeight = (int)(wicBitmap.Size.Height * DisplayProperties.LogicalDpi / 96.0);

                    var renderTargetProperties = new RenderTargetProperties(RenderTargetType.Default,
                        new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), 0, 0, RenderTargetUsage.None,
                        SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT);

                    renderTarget = new WicRenderTarget(dddFactory, wicBitmap, renderTargetProperties)
                    {
                        TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype
                    };
                }

                renderTarget.BeginDraw();

                
                renderTarget.DrawBitmap(LoadBitmapFromContentFile(wicFactory,markupFile.Path,renderTarget), 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor,new SharpDX.RectangleF(30,50,100,80));





                //new RectangleF(width - 150, 0, width, height + 25),

                renderTarget.EndDraw();

                var ms = new MemoryStream();

                var stream = new WICStream(wicFactory, ms);

                BitmapEncoder encoder = null;
                if (imageFile.FileType == ".png")
                    encoder = new PngBitmapEncoder(wicFactory);
                else if (imageFile.FileType == ".jpg")
                    encoder = new JpegBitmapEncoder(wicFactory);

                encoder.Initialize(stream);

                var frameEncoder = new BitmapFrameEncode(encoder);
                frameEncoder.Initialize();
                frameEncoder.SetSize(width, height);
                //frameEncoder.piPixelFormat = WicPixelFormat.FormatDontCare;
                frameEncoder.WriteSource(wicBitmap);
                frameEncoder.Commit();

                encoder.Commit();

                frameEncoder.Dispose();
                encoder.Dispose();
                stream.Dispose();

                ms.Position = 0;
                return ms;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private static SharpDX.Direct2D1.Bitmap LoadBitmapFromContentFile(ImagingFactory2 imagingFactory,string filePath,RenderTarget renderTarget)
        {
            SharpDX.Direct2D1.Bitmap newBitmap;

            // Neccessary for creating WIC objects.
            
            //NativeFileStream fileStream = new NativeFileStream(filePath,
            //    NativeFileMode.Open, NativeFileAccess.Read);

            // Used to read the image source file.
            BitmapDecoder bitmapDecoder =  new BitmapDecoder(imagingFactory, filePath, DecodeOptions.CacheOnDemand);

            // Get the first frame of the image.
            BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            // Convert it to a compatible pixel format.
            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            // Create the new Bitmap1 directly from the FormatConverter.
            newBitmap = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(renderTarget, converter);

            //Utilities.Dispose(ref bitmapDecoder);
            //Utilities.Dispose(ref fileStream);
            //Utilities.Dispose(ref imagingFactory);

            return newBitmap;
        }


        public static SharpDX.WIC.BitmapSource LoadBitmap(ImagingFactory2 factory, string ImageFilePath)
        {
            var bitmapDecoder = new BitmapDecoder(
                factory,
                ImageFilePath,
                DecodeOptions.CacheOnDemand
                );

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            return formatConverter;
        }

    }
}
