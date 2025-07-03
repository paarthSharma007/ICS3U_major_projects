using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1
{
    internal class Program
    {
        //Constants 
        const double LINER_COST = 21.48;
        const double WATER_COST = 0.48;
        const double POOL_WIDTH = 5.5;
        const double POOL_LENGTH = 14.2;
        const double DEEP_LENGTH = 8;
        const double SHALLOW_AREA_LENGTH = 1.8;

                static void Main(string[] args)
        {
            //Declaring 
            double deepDepth;
            double shallowDepth;


            double rampLength;
            double rampHeight;
            double rampHypotenuse;

            double deepEndside;
            double deepEndfloor;

            double shallowEndSide;
            double shallowEndFloor;

            double rampSideHigher;
            double rampSideLower;
            double rampSide;
            double rampFloor;

            double front;
            double back;

            double surfaceArea;


            double rampVolume;
            double shallowEndVolume;
            double deepEndvolume;

            double volume;


            double linerArea;
            double linerCost;
            
            double amtWater;
            double waterCost;

            double totalCost;

            //introduction
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("COST OF POOL\n");
            Console.WriteLine("--------------------------------------------");

            //input 
            Console.ForegroundColor= ConsoleColor.Red;

            Console.Write("Depth of the shallow end(m) :");
            shallowDepth = Convert.ToDouble(Console.ReadLine());
            Console.Write("Depth of deeper end(m) :");
            deepDepth = Convert.ToDouble(Console.ReadLine());

            //processing and storage 
            rampLength = POOL_LENGTH - DEEP_LENGTH - SHALLOW_AREA_LENGTH;
            rampHeight = deepDepth - shallowDepth;
            rampHypotenuse = Math.Sqrt(Math.Pow((rampLength), 2) + Math.Pow((rampHeight), 2));

            rampSideHigher = shallowDepth * rampLength;
            rampSideLower = 0.5 * rampLength * rampHeight;
            rampSide = rampSideHigher + rampSideLower;
            rampFloor = rampHypotenuse * POOL_WIDTH;

            shallowEndSide = SHALLOW_AREA_LENGTH * shallowDepth;
            shallowEndFloor = SHALLOW_AREA_LENGTH * POOL_WIDTH;
            front = shallowDepth * POOL_WIDTH;

            deepEndfloor = DEEP_LENGTH * POOL_WIDTH;
            deepEndside = deepDepth * DEEP_LENGTH;
            back = POOL_WIDTH * deepDepth;

            rampVolume = rampSide * POOL_WIDTH;
            shallowEndVolume = shallowEndSide * POOL_WIDTH;
            deepEndvolume = deepEndside * POOL_WIDTH;

            surfaceArea = front + back + 2 * shallowEndSide + shallowEndFloor + deepEndfloor + 2 * deepEndside + rampFloor + 2 * rampSide;
            volume = rampVolume + shallowEndVolume + deepEndvolume;

            linerArea = surfaceArea;
            linerCost = linerArea * LINER_COST;

            amtWater = volume;
            waterCost = amtWater * WATER_COST;

            totalCost = waterCost + linerCost;

            //output 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("The surface area is: " + Math.Round(surfaceArea,2) + " m^2");
            Console.WriteLine("The cost of Putting liner is : $" + Math.Round(linerCost, 2));
            Console.WriteLine("The Volume is: " + Math.Round(volume, 2) + " m^2");
            Console.WriteLine("The cost of Filling the pool is  : $" + Math.Round(waterCost,2));
            Console.WriteLine("The Total cost is  : $" + Math.Round(totalCost,2));
            Console.WriteLine("--------------------------------------------");
            Console.ReadLine();
        }
    }
}
