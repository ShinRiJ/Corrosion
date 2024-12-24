using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RustCheck.MVVM.Models
{
    class BrushAndImages
    {
        private const Int32 HIGH_IMAGE_SHAPE = 800;

        private Visibility _logoMarker;

        private Mat _imageMatInWorkToShow;
        private Mat _imageMatInWorkToSearch;

        public Mat _imageMatOriginal;
        private Mat _imageMatOriginalResized;

        private Mat _emptyMat;

        public Int32 ImageShowWidth { get; set; } = 1024;
        public Int32 ImageShowHeight { get; set; } = 630;
        public Boolean ImageExist { get; set; }

        public enum TargetImage
        {
            ResizedOriginal,
            ResizedSearch,
            Original
        };

        public Visibility LogoMarker
        {
            get => _logoMarker;
            set => _logoMarker = value;
        }

        public ImageBrush ImageBrush
        {
            get;
            init;
        }

        public Mat ImageMatInWorkToShow
        {
            get => _imageMatInWorkToShow;
            set
            {
                _imageMatInWorkToShow?.Dispose();
                _imageMatInWorkToShow = value;
                _imageMatInWorkToSearch = value.Clone();

                ImageBrushUpdate();
            }
        }

        public Mat ImageMatInWorkToSearch
        {
            get => _imageMatInWorkToSearch;
            set
            {
                _imageMatInWorkToSearch = value;
            }
        }

        public Mat ImageMat
        {
            get => _imageMatOriginalResized;
            set
            {
                _imageMatOriginal?.Dispose();
                _imageMatOriginalResized?.Dispose();

                if (value is not null)
                {
                    ImageExist = true;

                    Double OriginalRatio = (Double)value.Width / value.Height;

                    _imageMatOriginalResized = new Mat();
                    _imageMatOriginal = value.Clone();

                    if (OriginalRatio > 1)
                    {
                        ImageShowWidth = HIGH_IMAGE_SHAPE;
                        ImageShowHeight = (Int32) (HIGH_IMAGE_SHAPE / OriginalRatio);
                        CvInvoke.Resize(value, _imageMatOriginalResized, new System.Drawing.Size(HIGH_IMAGE_SHAPE, (Int32)(HIGH_IMAGE_SHAPE / OriginalRatio)), interpolation: Inter.Lanczos4);
                    }
                    else
                    {
                        ImageShowWidth = (Int32)(HIGH_IMAGE_SHAPE * OriginalRatio);
                        ImageShowHeight = HIGH_IMAGE_SHAPE;
                        CvInvoke.Resize(value, _imageMatOriginalResized, new System.Drawing.Size((Int32)(HIGH_IMAGE_SHAPE * OriginalRatio), HIGH_IMAGE_SHAPE), interpolation: Inter.Lanczos4);
                    }

                    ImageMatInWorkToShow = _imageMatOriginalResized.Clone();

                    LogoMarker = Visibility.Collapsed;
                }
                else
                {
                    ImageExist = false;

                    _emptyMat = new Mat(300, 300, DepthType.Cv8U, 3);
                    _emptyMat.SetTo(new MCvScalar(245, 245, 245));
                    ImageMatInWorkToShow = _emptyMat;

                    LogoMarker = Visibility.Visible;
                }
            }
        }

        public BrushAndImages()
        {
            ImageBrush = new ImageBrush();
            ImageMat = null;
        }

        public void AdjustGamma(TargetImage target, double gamma)
        {
            if (_imageMatOriginalResized is null || _imageMatOriginalResized is null)
                return;

            Matrix<Byte> lookUpTable;

            lookUpTable = new Matrix<byte>(1, 256);

            for (Int32 i = 0; i < 256; i++)
                lookUpTable[0, i] = (Byte)(255 * Math.Pow(i / 255.0, gamma));

            using(Mat result = new Mat())
            {

                switch (target)
                {
                    case TargetImage.ResizedOriginal:
                        Emgu.CV.CvInvoke.LUT(_imageMatOriginalResized, lookUpTable, ImageMatInWorkToShow);
                        ImageBrushUpdate();
                        break;
                    case TargetImage.ResizedSearch:
                        Emgu.CV.CvInvoke.LUT(ImageMatInWorkToShow, lookUpTable, ImageMatInWorkToSearch);
                        ImageBrushUpdate();
                        break;
                    case TargetImage.Original:
                        Emgu.CV.CvInvoke.LUT(_imageMatOriginal, lookUpTable, _imageMatOriginal);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ImageBrushUpdate(Mat? mat = null)
        {
            IntPtr hBitmap;

            if (mat is null)
                hBitmap = _imageMatInWorkToShow.ToBitmap().GetHbitmap();
            else
                hBitmap = mat.ToBitmap().GetHbitmap();

            try
            {
                ImageBrush.ImageSource = Imaging.CreateBitmapSourceFromHBitmap(
                                                hBitmap,
                                                IntPtr.Zero,
                                                Int32Rect.Empty,
                                                BitmapSizeOptions.FromEmptyOptions()
                                                );
            }
            finally
            {
                RustCheck.MainWindow.DeleteObject(hBitmap);
            }
        }
    }
}
