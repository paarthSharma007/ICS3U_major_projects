using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q2
{
    internal class Program
    {
        const double CLIFF_HEIGHT = 300;
        static void Main(string[] args)
        {
            //declaration 
            double angleClose;
            double angleFar;

            double radAngleClose;
            double radAngleFar;

            double distClose;
            double distFar;
            double distBetween;

            //Introduction 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("DISTANCE BWTWEEN BOTH BOATS ");
            Console.WriteLine("----------------------------------");

            //Input 

            Console.Write("Enter the angle of depression for farther boat(degrees) :");
            angleFar = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter the angle of depression for the closer boat(degrees) :");
            angleClose = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("----------------------------------");

            //Processing and stroage
            radAngleClose = angleClose * (Math.PI / 180);
            radAngleFar = angleFar * (Math.PI / 180);

            distFar = CLIFF_HEIGHT / Math.Tan(radAngleFar);
            distClose = CLIFF_HEIGHT / Math.Tan(radAngleClose);
            distBetween = distFar - distClose;

            //Output 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("----------------------------------");
            Console.WriteLine("The Close distance is :" + Math.Round(distClose,2) + "m");
            Console.WriteLine("The Far distance is :" + Math.Round(distFar, 2) + "m");
            Console.WriteLine("The distance between both boats is :" + Math.Round(distBetween, 2) + "m");
            Console.WriteLine("----------------------------------");

            Console.Read();
        }
    }
}
