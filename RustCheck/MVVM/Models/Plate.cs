using Emgu.CV;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace RustCheck.MVVM.Models
{
    internal class Plate
    {
        public Double Corrosion { get; set; }
        private Image<Bgr, Byte>? _imageOriginal;
        public Int32 Num { get; set; }
        public Point Center { get; init; }
        public Point CenterForNum { get; init; }
        public Boolean Showed { get; set; }
        public VectorOfPoint BorderContour { get; init; }
        public VectorOfVectorOfPoint? InterContour { get; set; }
        public Image<Bgr, Byte>? ImageToShow { get; set; }
        public Image<Bgr, Byte>? ImageOriginal
        {
            get => _imageOriginal;
            set
            {
                _imageOriginal = value;
                ImageToShow = new Image<Bgr, Byte>(value.Size);
                value?.CopyTo(ImageToShow);
            }
        }

        public Plate(VectorOfPoint contour)
        {
            Showed = false;
            BorderContour = new VectorOfPoint(contour.ToArray());
            Moments moments = CvInvoke.Moments(BorderContour);
            Center = new Point((Int32)(moments.M10 / moments.M00), (Int32)(moments.M01 / moments.M00));
            CenterForNum = new Point((Int32)(moments.M10 / moments.M00) - 9, (Int32)(moments.M01 / moments.M00) + 10);
        }

        public void Clear()
        {
            ImageToShow?.Dispose();
            BorderContour?.Dispose();
        }

        public void CropOriginal(Double left, Double top, Double right, Double bottom)
        {
            System.Drawing.Rectangle roi = new System.Drawing.Rectangle(
                (Int32)(left * ImageOriginal.Width / 100),
                (Int32)(top * ImageOriginal.Height / 100),
                ImageOriginal.Width - ((Int32)(left * ImageOriginal.Width / 100) + (Int32)(right * ImageOriginal.Width / 100)),
                ImageOriginal.Height - ((Int32)(top * ImageOriginal.Height / 100) + (Int32)(bottom * ImageOriginal.Height / 100))
            );

            ImageToShow = ImageOriginal.Copy(roi);
        }

        public void AreaCalc()
        {
            InterContour = new VectorOfVectorOfPoint();

            Image<Hsv, Byte> result = ImageToShow.Convert<Hsv, Byte>();
            Image<Gray, Byte> mask = new Image<Gray, Byte>(result.Size);

            Hsv lower = new Hsv(0, 0, 0);
            Hsv upper = new Hsv(179, 70, 255);

            mask = result.InRange(lower, upper);
            mask = mask.AbsDiff(new Gray(255));

            Mat hierarchy = new Mat();

            CvInvoke.FindContours(mask, InterContour, hierarchy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            Moments moments = CvInvoke.Moments(mask);

            Int32 a = CvInvoke.CountNonZero(mask); ;
            Int32 b = result.Width * result.Height;

            Corrosion = 100 * a / (Double)b;

            mask.Dispose();
            result.Dispose();
        }
    }
}
