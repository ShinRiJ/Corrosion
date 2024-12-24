using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Flann;
using Emgu.CV.Ocl;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace Corrosion_damage
{
    class CorrosionCalculator : IDisposable
    {
        private const Int32 HIGH_IMAGE_SHAPE = 800;
        private Boolean disposed = false;
        private Image<Bgr, Byte> OriginalForCropping { get; set; }
        public Image<Bgr, Byte> Original { get; set; }
        public Image<Bgr, Byte> Processed { get; set; }
        public Image<Gray, Byte> Mask { get; set; }
        public Double OriginalRatio { get; set; }

        public List<Plastin> Plastins { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Предотвращаем финализацию
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                    Clear();
                }
                // Освобождаем неуправляемые ресурсы здесь, если есть

                disposed = true;
            }
        }

        ~CorrosionCalculator()
        {
            Dispose(false); // Финализатор вызывает Dispose(false)
        }

        public CorrosionCalculator(String inputPath)
        {
            Plastins = new List<Plastin>();
            Original = new Image<Bgr, Byte>(inputPath);
            OriginalForCropping = new Image<Bgr, Byte>(inputPath);

            Int32 getWidth = Original.Width;
            Int32 getHeight = Original.Height;

            OriginalRatio = (Double)getWidth / getHeight;

            if (OriginalRatio > 1)
                Original = Original.Resize(HIGH_IMAGE_SHAPE, (Int32)(HIGH_IMAGE_SHAPE / OriginalRatio), Emgu.CV.CvEnum.Inter.NearestExact);
            else
                Original = Original.Resize((Int32)(HIGH_IMAGE_SHAPE * OriginalRatio), HIGH_IMAGE_SHAPE, Emgu.CV.CvEnum.Inter.NearestExact);

            Processed?.Dispose();
            Processed = new Image<Bgr, Byte>(Original.Size);
            Original.CopyTo(Processed);
        }

        public void Clear()
        {
            OriginalRatio = 0;

            Plastins.Clear();

            Original.Dispose();
            Processed.Dispose();
            Mask.Dispose();
            OriginalForCropping.Dispose();

            Original = null;
            Processed = null;
            Mask = null;
            OriginalForCropping = null;
        }

        public Boolean Update(String inputPath)
        {
            if (true)
            {
                Clear();
                Original = new Image<Bgr, Byte>(inputPath);
                OriginalForCropping = new Image<Bgr, Byte>(inputPath);

                Int32 getWidth = Original.Width;
                Int32 getHeight = Original.Height;

                OriginalRatio = (Double) getWidth / getHeight;

                if (OriginalRatio > 1)
                    Original = Original.Resize(HIGH_IMAGE_SHAPE, (Int32)(HIGH_IMAGE_SHAPE / OriginalRatio), Emgu.CV.CvEnum.Inter.NearestExact);
                else
                    Original = Original.Resize((Int32)(HIGH_IMAGE_SHAPE * OriginalRatio), HIGH_IMAGE_SHAPE, Emgu.CV.CvEnum.Inter.NearestExact);

                Processed?.Dispose();
                Processed = new Image<Bgr, Byte>(Original.Size);
                Original.CopyTo(Processed);

                return true;
            }

            return false;
        }

        public void AdjustGamma(Image<Bgr, Byte> dest, double gamma)
        {
            Matrix<Byte> lookUpTable;

            Plastins = new List<Plastin>();

            lookUpTable = new Matrix<byte>(1, 256);

            for (Int32 i = 0; i < 256; i++)
                lookUpTable[0, i] = (Byte)(255 * Math.Pow(i / 255.0, gamma));

            Emgu.CV.CvInvoke.LUT(Original, lookUpTable, dest);
        }

        public void FindPlastins(Image<Hsv, Byte> hsvWorkImage, Image<Bgr, Byte> src)
        {
            src.CopyTo(Processed);

            Hsv min = new Hsv(0, 20, 0);
            Hsv max = new Hsv(179, 255, 170);

            Mask = hsvWorkImage.InRange(min, max);
            Mask = Mask.AbsDiff(new Gray(255));

            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2, 2), new Point(-1, -1));

            CvInvoke.Erode(Mask, Mask, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));
            CvInvoke.Dilate(Mask, Mask, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));

            VectorOfVectorOfPoint filtredContours = new VectorOfVectorOfPoint();

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();

            CvInvoke.FindContours(Mask, contours, hierarchy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            hsvWorkImage.Dispose();

            var _hierarchy = (Int32[,,])hierarchy.GetData();

            for (Int32 index = 0; index < contours.Size; index++)
            {
                if (CvInvoke.ContourArea(contours[index]) > 8000 && _hierarchy[0, index, 3] != -1)
                {
                    Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[index]);
                    double boundingBoxArea = boundingBox.Width * boundingBox.Height;

                    if (CvInvoke.ContourArea(contours[index]) / boundingBoxArea > 0.7)
                    {
                        VectorOfPoint buffer = new VectorOfPoint();
                        CvInvoke.ApproxPolyDP(contours[index], buffer, 0.05 * CvInvoke.ArcLength(contours[index], true), true);

                        if (buffer.Size == 4)
                        {
                            filtredContours.Push(buffer);
                            Plastins.Add(new Plastin(buffer));
                        }
                        else
                        {
                            RotatedRect minAreaRect = CvInvoke.MinAreaRect(contours[index]);
                            PointF[] boxPoints = CvInvoke.BoxPoints(minAreaRect);
                            Point[] _buf = new Point[boxPoints.Length];

                            for (Int32 i = 0; i < boxPoints.Length; i++)
                                _buf[i] = new Point((Int32)boxPoints[i].X, (Int32)boxPoints[i].Y);

                            filtredContours.Push(new VectorOfPoint(_buf));
                            Plastins.Add(new Plastin(buffer));
                        }
                    }
                }
            }

            filtredContours.Dispose();
            contours.Dispose();


            Plastins.Sort((pl1, pl2) => (pl1.Center.Y * 5 + pl1.Center.X).CompareTo(pl2.Center.Y * 5 + pl2.Center.X));

            for (Int32 i = 0; i < Plastins.Count; i++)
                Plastins[i].Num = i + 1;

            foreach (var item in Plastins)
            {
                CvInvoke.DrawContours(Processed, new VectorOfVectorOfPoint(item.BorderContour), -1, new MCvScalar(0, 255, 0), 1);
                CvInvoke.Circle(Processed, item.Center, 15, new MCvScalar(0, 255, 0), -1);
                CvInvoke.PutText(Processed, item.Num.ToString(), item.CenterForNum, FontFace.HersheySimplex, 1, new MCvScalar(255, 255, 255), 2);
            }
        }

        public void CropFromOrig(Plastin plst)
        {
            Image<Gray, Byte> mask = new Image<Gray, Byte>(OriginalForCropping.Size);
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(OriginalForCropping.Size);

            Double scaleX = (Double) OriginalForCropping.Width / Original.Width;
            Double scaleY = (Double) OriginalForCropping.Height / Original.Height;

            Point[] scaledPoints = new Point[plst.BorderContour.Size];

            for (Int32 i = 0; i < plst.BorderContour.Size; i++)
                scaledPoints[i] = new Point((Int32)(plst.BorderContour[i].X * scaleX), (Int32)(plst.BorderContour[i].Y * scaleY));

            Int32 minX = scaledPoints.Min(p => p.X);
            Int32 minY = scaledPoints.Min(p => p.Y);
            Int32 maxX = scaledPoints.Max(p => p.X);
            Int32 maxY = scaledPoints.Max(p => p.Y);

            SortPointsClockwise(scaledPoints);

            Int32 width = maxX - minX;
            Int32 height = maxY - minY;

            Double expansionFactor = 0.1f;

            Int32 expandX = (Int32)(width * expansionFactor);
            Int32 expandY = (Int32)(height * expansionFactor);

            Point[] expandedPoints = new Point[scaledPoints.Length];

            expandedPoints[0] = new Point(scaledPoints[0].X - expandX / 2, scaledPoints[0].Y - expandY / 2);
            expandedPoints[1] = new Point(scaledPoints[1].X + expandX / 2, scaledPoints[1].Y - expandY / 2);
            expandedPoints[2] = new Point(scaledPoints[2].X + expandX / 2, scaledPoints[2].Y + expandY / 2);
            expandedPoints[3] = new Point(scaledPoints[3].X - expandX / 2, scaledPoints[3].Y + expandY / 2);

            CvInvoke.DrawContours(mask, new VectorOfVectorOfPoint(new VectorOfPoint(expandedPoints)), -1, new MCvScalar(255, 255, 255), -1);

            OriginalForCropping.Copy(result, mask);

            double angle = Math.Atan2(expandedPoints[1].Y - expandedPoints[0].Y, expandedPoints[1].X - expandedPoints[0].X) * (180.0 / Math.PI);

            result = result.GetSubRect(new System.Drawing.Rectangle(minX - expandX / 2, minY - expandY / 2, width + expandX, height + expandY));

            Mat rotationMatrix = new Mat();
            CvInvoke.GetRotationMatrix2D(new PointF(result.Width / 2f, result.Height / 2f), angle, 1.0, rotationMatrix);
            CvInvoke.WarpAffine(result, result, rotationMatrix, result.Size);

            plst.preImg = result.Clone();
            plst.resImg = result.Clone();
            result.Dispose();
        }

        private void SortPointsClockwise(Point[] points)
        {
            float centerX = 0;
            float centerY = 0;

            foreach (var point in points)
            {
                centerX += point.X;
                centerY += point.Y;
            }

            centerX /= points.Length;
            centerY /= points.Length;

            Array.Sort(points, (p1, p2) =>
            {
                double angle1 = Math.Atan2(p1.Y - centerY, p1.X - centerX);
                double angle2 = Math.Atan2(p2.Y - centerY, p2.X - centerX);
                return angle1.CompareTo(angle2);
            });
        }
    }

    class Plastin
    {
        public Int32 Num { get; set; }
        public Double Corrosion { get; set; }
        public Boolean Showed { get; set; } = false;
        public Point Center { get; init; }
        public Point CenterForNum { get; init; }
        public Image<Bgr, Byte>? preImg { get; set; }
        public Image<Bgr, Byte>? resImg { get; set; }
        public Image<Bgr, Byte>? resImgWithCnt { get; set; }
        public VectorOfPoint BorderContour { get; init; }

        public Plastin(VectorOfPoint contour)
        {
            BorderContour = new VectorOfPoint(contour.ToArray());
            Moments moments = CvInvoke.Moments(BorderContour);
            Center = new Point((Int32) (moments.M10 / moments.M00), (Int32)(moments.M01 / moments.M00));
            CenterForNum = new Point((Int32)(moments.M10 / moments.M00) - 9, (Int32)(moments.M01 / moments.M00) + 10);
        }

        public void AreaCalc()
        {
            resImgWithCnt = resImg.Clone();
            Image<Hsv, Byte> result = resImgWithCnt.Convert<Hsv, Byte>();
            Image<Gray, Byte> mask = new Image<Gray, Byte>(result.Size);

            Hsv lower = new Hsv(0, 0, 0);
            Hsv upper = new Hsv(179, 70, 255);

            mask = result.InRange(lower, upper);
            mask = mask.AbsDiff(new Gray(255));

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();

            CvInvoke.FindContours(mask, contours, hierarchy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            Moments moments = CvInvoke.Moments(mask);

            Int32 a = CvInvoke.CountNonZero(mask); ;
            Int32 b = resImg.Width * resImg.Height;

            Corrosion = 100 * a / (Double) b;

            CvInvoke.DrawContours(resImgWithCnt, contours, -1, new MCvScalar(0, 255, 0), 1);

            mask.Dispose();
            result.Dispose();
        }
    }
}
