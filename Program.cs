using System;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

class Program
{
    static void circleDetection(string file)
    {
        // Read the image
        Mat inputImage = new Mat(file, ImreadModes.Color);

        // Convert image to grayscale for circle detection
        Mat grayscaleImage = new Mat();
        CvInvoke.CvtColor(inputImage, grayscaleImage, ColorConversion.Bgr2Gray);

        // Circle detection using Transformasi Hough Circle
        CircleF[] circles = CvInvoke.HoughCircles(
            grayscaleImage,
            HoughModes.Gradient,
            dp: 1,
            minDist: 20,
            param1: 100,
            param2: 30,
            minRadius: 0,
            maxRadius: 0
        );

        string outputLabel = "";
        // Make sure at least one cirle
        if (circles.Length > 0)
        {
            // Get first circle
            CircleF circle = circles[0];
            // Check if circle in the center of image
            PointF imageCenter = new PointF(inputImage.Width / 2, inputImage.Height / 2);
            double distanceToCenter = Math.Sqrt(Math.Pow(circle.Center.X - imageCenter.X, 2) + Math.Pow(circle.Center.Y - imageCenter.Y, 2));

            // Check if the circle with correct radius
            if (distanceToCenter <= inputImage.Width / 2 && circle.Radius >= 100 && circle.Radius <= 200)
            {
                Console.WriteLine("Label: GO (Accepted)");
                CvInvoke.Circle(inputImage, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(0, 255, 0), 2);
                outputLabel = "Label: GO (Accepted)";
            }
            else
            {
                Console.WriteLine("Label: AG (Arguably GO)");
                CvInvoke.Circle(inputImage, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(0, 255, 0), 2);
                outputLabel = "Label: AG (Arguably GO)";
            }
        }
        else
        {
            Console.WriteLine("Label: NG (NO GO)");
            outputLabel = "Label: NG (NO GO)";
        }

        CvInvoke.Imshow($"Result {outputLabel}", inputImage);
        CvInvoke.WaitKey();

    }

    static void Main(string[] args)
    {
        string folderPath = "./img";
        if (Directory.Exists(folderPath))
        {
            // Getting files in folder
            string[] files = Directory.GetFiles(folderPath);

            Console.WriteLine("List file:");
            foreach (string file in files)
            {
                Console.WriteLine(file);
                circleDetection(file);
            }
        }
        else
        {
            Console.WriteLine("Directory not found.");
        }
    }
}