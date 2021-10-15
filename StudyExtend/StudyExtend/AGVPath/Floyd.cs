using System;
using System.Collections.Generic;
using System.Text;

namespace StudyExtend.AGVPath
{
    public class Floyd
    {
        static double INF = 1.0 / 0.0;
        //public static double[,] G = new double[4, 4] { {0,5,12,Floyd.INF },{ 10,0,5,INF},{ INF,INF,0,3},{ INF,5,12,0} };


        public static double[,] AGV = new double[43, 43]
            {
                {0,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1 },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,0,1,Floyd.INF,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1,Floyd.INF },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0,1 },
                {Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,Floyd.INF,1,Floyd.INF,Floyd.INF,Floyd.INF,1,0 },
            };
        public static Dist[,] GetFloyd(double[,] G)
        {
            int i, j, v;
            int N = G.GetLength(0);
            Dist[,] D = new Dist[N, N];
            for (i = 0; i < N; i++)
            {
                for (j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        D[i, j].length = 0;
                        D[i, j].pre = i;
                    }
                    else 
                    {
                        if (G[i, j] != INF)
                        {
                            D[i, j].length = G[i, j];
                            D[i, j].pre = i;
                        }
                        else
                        {
                            D[i, j].length = INF;
                            D[i, j].pre = -1;
                        }
                    }
                }
            }
            for (v = 0; v < N; v++)
            {
                for (i = 0; i < N; i++)
                {
                    for (j = 0; j < N; j++)
                    {
                        if (D[i, j].length > (D[i, v].length + D[v, j].length))
                        {
                            D[i, j].length = D[i, v].length + D[v, j].length;
                            D[i, j].pre = D[v, j].pre;
                        }
                    }
                }
            }
            return D;
        }

        public static Dictionary<int, string> dic = new Dictionary<int, string>() { 
            { 0,"0,0"},
            { 1,"0,1"},
            { 2,"0,2"},
            { 3,"0,3"},
            { 4,"0,4"},
            { 5,"1,1"},
            { 6,"1,2"},
            { 7,"1,3"},
            { 8,"1,4"},
            { 9,"2,1"},
            { 10,"2,2"},
            { 11,"2,3"},
            { 12,"2,4"},
            { 13,"3,1"},
            { 14,"3,2"},
            { 15,"3,3"},
            { 16,"3,4"},
            { 17,"4,-1"},
            { 18,"4,0"},
            { 19,"4,1"},
            { 20,"4,2"},
            { 21,"4,3"},
            { 22,"4,4"},
            { 23,"5,0"},
            { 24,"5,1"},
            { 25,"5,2"},
            { 26,"5,3"},
            { 27,"5,4"},
            { 28,"6,0"},
            { 29,"6,1"},
            { 30,"6,2"},
            { 31,"6,3"},
            { 32,"6,4"},
            { 33,"7,0"},
            { 34,"7,1"},
            { 35,"7,2"},
            { 36,"7,3"},
            { 37,"7,4"},
            { 38,"8,0"},
            { 39,"8,1"},
            { 40,"8,2"},
            { 41,"8,3"},
            { 42,"8,4"},
        };
        
        public static bool pre(Dist[,] D, int start, int end,out List<int> path1)
        {
            path1 = new List<int>();
            var res = false;
            while (D[start, end].pre != start)
            {
                var write = pre(D, start, D[start, end].pre,out path1);
                if (!write)
                {
                    Console.Write("->" + D[start, end].pre);
                    path1.Add(D[start, end].pre);
                    res = true;
                }
                start = D[start, end].pre;
            }

            Console.Write("->" + end);
            path1.Add(end);
            res = true;
            return res;
        }

        public static List<string> GetRes(int st,int en)
        {
            List<string> res = new List<string>();
            var D = Floyd.GetFloyd(Floyd.AGV);
            var count = 0;
            foreach (var item in D)
            {
                Console.Write(item.length);
                Console.Write(" ");
                count++;
                if (count % D.GetLength(1) == 0)
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine();

            int start = st, end = en;
            Console.WriteLine("V" + start + "到V" + end + "最短距离:" + D[start, end].length);

            Console.WriteLine();

            Console.WriteLine("V" + start + "到V" + end + "最短距离的路径:");
            List<int> path = new List<int>();
            Console.Write(start);
            path.Add(start);
            List<int> path1 = new List<int>();
            Floyd.pre(D, start, end, out path1);
            path.AddRange(path1);
            Console.WriteLine();
            string val = "";
            List<string> valString = new List<string>();
            foreach (var item in path)
            {
                Console.Write(item);
                Floyd.dic.TryGetValue(item, out val);
                Console.WriteLine(" " + val);
                valString.Add(val);
            }
            var result = "";
            for (var i = 0; i < valString.Count - 1; i++)
            {
                var s = valString[i].ToString();
                var e = valString[i + 1].ToString();

                var startarr = s.Split(',');
                var endarr = e.Split(',');

                if (startarr[0] != endarr[0])
                {
                    if (int.Parse(startarr[0]) > int.Parse(endarr[0]))
                    {
                        result = "left";
                    }
                    else
                    {
                        result = "right";
                    }
                }
                else
                {
                    if (int.Parse(startarr[1]) > int.Parse(endarr[1]))
                    {
                        result = "up";
                    }
                    else
                    {
                        result = "down";
                    }
                }
                Console.WriteLine(result);
                res.Add(result);
            }
            return res;
        }

    }
    public struct Dist {
        public double length;
        public int pre;
    }
}
