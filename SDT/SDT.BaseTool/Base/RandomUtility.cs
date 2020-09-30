using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SDT.BaseTool
{
    /// <summary>
    /// 获取随机数操作工具类
    /// </summary>
    public static class RandomUtility
    {
        private static readonly Random _globalRandom = new Random();

        /// <summary>
        /// get random
        /// </summary>
        /// <returns></returns>
        [ThreadStatic]
        private static Random _localRandom;

        public static Random Random
        {
            get
            {
                if (_localRandom == null)
                {
                    lock (_globalRandom)
                    {
                        if (_localRandom == null)
                        {
                            var seed = _globalRandom.Next();
                            _localRandom = new Random(seed);
                        }
                    }
                }

                return _localRandom;
            }
        }

        public static int Next() => Random.Next();

        public static int Next(int max) => Random.Next(max);

        public static int Next(int min, int max) => Random.Next(min, max);

        public static double NextDouble() => Random.NextDouble();

        public static int[] Array(int sum, int count, int percentage = 50)
        {
            var array = new int[count];
            var curSum = 0;
            for (var i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    var num = sum - curSum;
                    array[i] = num;
                }
                else
                {
                    var max = Math.Ceiling((sum - curSum - count + i + 1) * (percentage / 100d)).ConvertInt32();
                    var num = Next(1, max);
                    curSum += num;
                    array[i] = num;
                }
            }

            return array;
        }

        /// <summary>
        /// get a random string (containing a special symbol)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string String(int length)
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = (char)Random.Next(33, 127);
            }

            return new string(chars);
        }

        /// <summary>
        /// get a random string (not containing a special symbol)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string StringCommon(int length)
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                var value = Random.Next(62);
                if (value < 10)
                {
                    value += 48;//取数字
                }
                else if (value < 36)
                {
                    value += 55;//大写字母
                }
                else
                {
                    value += 61;//小写字母
                }

                chars[i] = (char)value;
            }

            return new string(chars);
        }

        /// <summary>
        /// random sorting of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void Sort<T>(T[] arr)
        {
            //random sort algorithm for array: two randomly selected position, two position on the value of the exchange
            // times, the length of the array is used here as the exchange number
            var count = arr.Length;

            for (var i = 0; i < count; i++)
            {
                //generates two random number positions
                var rn1 = Random.Next(arr.Length);
                var rn2 = Random.Next(arr.Length);
                T temp;

                //exchange the values of two random number positions
                temp = arr[rn1];
                arr[rn1] = arr[rn2];
                arr[rn2] = temp;
            }
        }

        /// <summary>
        /// get a random color
        /// </summary>
        /// <returns></returns>
        public static Color Color()
        {
            var red = Random.Next(255);
            var green = Random.Next(255);
            var blue = 0;

            if (red + green <= 300)
            {
                blue = 400 - red - green;
                if (blue > 255)
                {
                    blue = 255;
                }
            }

            var clr = System.Drawing.Color.FromArgb(red, green, blue);
            return clr;
        }

        /// <summary>
        /// get guid (32 bit random number)
        /// </summary>
        /// <returns></returns>
        public static string GuidString() => Guid.NewGuid().ToString("N");
    }
}
