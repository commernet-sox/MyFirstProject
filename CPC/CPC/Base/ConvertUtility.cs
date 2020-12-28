using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CPC
{
    /// <summary>
    /// common convert
    /// </summary>
    public sealed class ConvertUtility
    {
        #region Other Common Convertsion
        /// <summary>
        /// get the object memory address
        /// </summary>
        /// <param name="obj">traget</param>
        /// <returns></returns>
        public static string GetMomory(object obj)
        {
            var h = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var p = h.AddrOfPinnedObject();
            var result = "0x" + p.ToString("X");
            return result;
        }

        /// <summary>
        ///  convert an object to a specified type
        /// </summary>
        /// <param name="obj">the object to be converted</param>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static object ConvertObject(object obj, Type type)
        {
            if (type == null)
            {
                return obj;
            }

            if (obj == null)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            var underlyingType = Nullable.GetUnderlyingType(type);

            //conversion is not necessary if the type of the object to be converted is compatible with the target type
            if (type.IsAssignableFrom(obj.GetType()))
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) //if the base type of the object to be converted is enumerated
            {
                //if the target type can be converted and empty enumeration object for the null directly return null
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString()))
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))//if the base type of the target type is IConvertible, it is converted directly
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    var o = constructor.Invoke(null);
                    var propertys = type.GetProperties();
                    var oldType = obj.GetType();
                    foreach (var property in propertys)
                    {
                        var p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }

                    return o;
                }
            }

            return obj;
        }
        #endregion

        #region Text To Text Conversion
        /// <summary>
        /// convert the amount into capital
        /// </summary>
        /// <param name="money">amount</param>
        /// <returns>result</returns>
        public static string ConvertAmountWords(decimal money)
        {
            var cha = "零壹贰叁肆伍陆柒捌玖";//0-9 corresponding chinese characters
            var unit = "万仟佰拾亿仟佰拾万仟佰拾元角分";//digit bit corresponding chinese characters
            var result = string.Empty;
            var ch_unit = string.Empty;
            var nzero = 0;//compute continuous zero values
            money = Math.Round(Math.Abs(money), 2);
            var moneyStr = ((long)(money * 100)).ToString();

            var j = moneyStr.Length;//get the highest number
            if (j > 15)
            {
                return "溢出";
            }

            if (money == 0)
            {
                return "零元整";
            }

            unit = unit.Substring(15 - j);

            //loop out each of the values that need to be converted
            for (var i = 0; i < j; i++)
            {
                var ms_unit = moneyStr.Substring(i, 1);
                var tmp = Convert.ToInt32(ms_unit);
                string ch_cha;
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    if (tmp == 0)
                    {
                        ch_cha = string.Empty;
                        ch_unit = string.Empty;
                        nzero++;
                    }
                    else
                    {
                        if (tmp > 0 && nzero != 0)
                        {
                            ch_cha = "零" + cha.Substring(tmp * 1, 1);
                            ch_unit = unit.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch_cha = cha.Substring(tmp * 1, 1);
                            ch_unit = unit.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    if (tmp > 0 && nzero != 0)
                    {
                        ch_cha = "零" + cha.Substring(tmp * 1, 1);
                        ch_unit = unit.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (tmp > 0 && nzero == 0)
                        {
                            ch_cha = cha.Substring(tmp * 1, 1);
                            ch_unit = unit.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (tmp > 0 && nzero >= 3)
                            {
                                ch_cha = string.Empty;
                                ch_unit = string.Empty;
                                nzero++;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch_cha = string.Empty;
                                    nzero++;
                                }
                                else
                                {
                                    ch_cha = string.Empty;
                                    ch_unit = unit.Substring(i, 1);
                                    nzero++;
                                }
                            }
                        }
                    }
                }

                if (i == (j - 11) || i == (j - 3))
                {
                    ch_unit = unit.Substring(i, 1);
                }

                result += ch_cha + ch_unit;

                if (i == j - 1 && ms_unit == "0")
                {
                    result += '整';
                }
            }

            return result;
        }

        /// <summary>
        /// get the pinyin of chinese characters
        /// </summary>
        /// <param name="characters">chinese characters</param>
        /// <returns>result</returns>
        public static string ConvertSpellings(string characters)
        {
            var getName = new string[] { "A", "Ai", "An", "Ang", "Ao", "Ba", "Bai", "Ban", "Bang", "Bao", "Bei", "Ben", "Beng", "Bi", "Bian", "Biao", "Bie", "Bin", "Bing", "Bo", "Bu", "Ba", "Cai", "Can", "Cang", "Cao", "Ce", "Ceng", "Cha", "Chai", "Chan", "Chang", "Chao", "Che", "Chen", "Cheng", "Chi", "Chong", "Chou", "Chu", "Chuai", "Chuan", "Chuang", "Chui", "Chun", "Chuo", "Ci", "Cong", "Cou", "Cu", "Cuan", "Cui", "Cun", "Cuo", "Da", "Dai", "Dan", "Dang", "Dao", "De", "Deng", "Di", "Dian", "Diao", "Die", "Ding", "Diu", "Dong", "Dou", "Du", "Duan", "Dui", "Dun", "Duo", "E", "En", "Er", "Fa", "Fan", "Fang", "Fei", "Fen", "Feng", "Fo", "Fou", "Fu", "Ga", "Gai", "Gan", "Gang", "Gao", "Ge", "Gei", "Gen", "Geng", "Gong", "Gou", "Gu", "Gua", "Guai", "Guan", "Guang", "Gui", "Gun", "Guo", "Ha", "Hai", "Han", "Hang", "Hao", "He", "Hei", "Hen", "Heng", "Hong", "Hou", "Hu", "Hua", "Huai", "Huan", "Huang", "Hui", "Hun", "Huo", "Ji", "Jia", "Jian", "Jiang", "Jiao", "Jie", "Jin", "Jing", "Jiong", "Jiu", "Ju", "Juan", "Jue", "Jun", "Ka", "Kai", "Kan", "Kang", "Kao", "Ke", "Ken", "Keng", "Kong", "Kou", "Ku", "Kua", "Kuai", "Kuan", "Kuang", "Kui", "Kun", "Kuo", "La", "Lai", "Lan", "Lang", "Lao", "Le", "Lei", "Leng", "Li", "Lia", "Lian", "Liang", "Liao", "Lie", "Lin", "Ling", "Liu", "Long", "Lou", "Lu", "Lv", "Luan", "Lue", "Lun", "Luo", "Ma", "Mai", "Man", "Mang", "Mao", "Me", "Mei", "Men", "Meng", "Mi", "Mian", "Miao", "Mie", "Min", "Ming", "Miu", "Mo", "Mou", "Mu", "Na", "Nai", "Nan", "Nang", "Nao", "Ne", "Nei", "Nen", "Neng", "Ni", "Nian", "Niang", "Niao", "Nie", "Nin", "Ning", "Niu", "Nong", "Nu", "Nv", "Nuan", "Nue", "Nuo", "O", "Ou", "Pa", "Pai", "Pan", "Pang", "Pao", "Pei", "Pen", "Peng", "Pi", "Pian", "Piao", "Pie", "Pin", "Ping", "Po", "Pu", "Qi", "Qia", "Qian", "Qiang", "Qiao", "Qie", "Qin", "Qing", "Qiong", "Qiu", "Qu", "Quan", "Que", "Qun", "Ran", "Rang", "Rao", "Re", "Ren", "Reng", "Ri", "Rong", "Rou", "Ru", "Ruan", "Rui", "Run", "Ruo", "Sa", "Sai", "San", "Sang", "Sao", "Se", "Sen", "Seng", "Sha", "Shai", "Shan", "Shang", "Shao", "She", "Shen", "Sheng", "Shi", "Shou", "Shu", "Shua", "Shuai", "Shuan", "Shuang", "Shui", "Shun", "Shuo", "Si", "Song", "Sou", "Su", "Suan", "Sui", "Sun", "Suo", "Ta", "Tai", "Tan", "Tang", "Tao", "Te", "Teng", "Ti", "Tian", "Tiao", "Tie", "Ting", "Tong", "Tou", "Tu", "Tuan", "Tui", "Tun", "Tuo", "Wa", "Wai", "Wan", "Wang", "Wei", "Wen", "Weng", "Wo", "Wu", "Xi", "Xia", "Xian", "Xiang", "Xiao", "Xie", "Xin", "Xing", "Xiong", "Xiu", "Xu", "Xuan", "Xue", "Xun", "Ya", "Yan", "Yang", "Yao", "Ye", "Yi", "Yin", "Ying", "Yo", "Yong", "You", "Yu", "Yuan", "Yue", "Yun", "Za", "Zai", "Zan", "Zang", "Zao", "Ze", "Zei", "Zen", "Zeng", "Zha", "Zhai", "Zhan", "Zhang", "Zhao", "Zhe", "Zhen", "Zheng", "Zhi", "Zhong", "Zhou", "Zhu", "Zhua", "Zhuai", "Zhuan", "Zhuang", "Zhui", "Zhun", "Zhuo", "Zi", "Zong", "Zou", "Zu", "Zuan", "Zui", "Zun", "Zuo" };

            var getValue = new int[] { -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032, -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012, -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, -16970, -16942, -16915, -16733, -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216, -16212, -16205, -16202, -16187, -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394, -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654, -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345, -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601, -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343, -13340, -13329, -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067, -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018, -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328, -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254 };

            //verify that is chinese
            var reg = new Regex("^[\u4e00-\u9fa5]$");
            var result = string.Empty;
            var mChar = characters.ToCharArray();//get chinese characters corresponding char array
            for (var j = 0; j < mChar.Length; j++)
            {
                if (reg.IsMatch(mChar[j].ToString()))
                {
                    var arr = Encoding.Default.GetBytes(mChar[j].ToString());
                    int m1 = arr[0];
                    int m2 = arr[1];
                    var asc = (m1 * 256) + m2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        result += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                result += "Zhen"; break;
                            case -8985:
                                result += "Qian"; break;
                            case -5463:
                                result += "Jia"; break;
                            case -8274:
                                result += "Ge"; break;
                            case -5448:
                                result += "Ga"; break;
                            case -5447:
                                result += "La"; break;
                            case -4649:
                                result += "Chen"; break;
                            case -5436:
                                result += "Mao"; break;
                            case -5213:
                                result += "Mao"; break;
                            case -3597:
                                result += "Die"; break;
                            case -5659:
                                result += "Tian"; break;
                            default:
                                for (var i = getValue.Length - 1; i >= 0; i--)
                                {
                                    if (getValue[i] <= asc) //determine whether the pinyin encoding of chinese characters is within the specified range
                                    {
                                        result += getName[i];
                                        break;
                                    }
                                }

                                break;
                        }
                    }
                }
                else
                {
                    result += mChar[j].ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// get the phonetic initials of Chinese characters
        /// </summary>
        /// <param name="characters">chinese characters</param>
        /// <returns>result</returns>
        public static string ConvertAlif(string characters)
        {
            var i = 0;
            var result = string.Empty;
            var gbk = Encoding.GetEncoding(936);
            var unicodeBytes = Encoding.Unicode.GetBytes(characters);
            var gbkBytes = Encoding.Convert(Encoding.Unicode, gbk, unicodeBytes);

            while (i < gbkBytes.Length)
            {
                if (gbkBytes[i] <= 127)
                {
                    result += (char)gbkBytes[i];
                    i++;
                }
                else
                {
                    var key = (ushort)((gbkBytes[i] * 256) + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    {
                        result += "A";
                    }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    {
                        result += "B";
                    }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    {
                        result += "C";
                    }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    {
                        result += "D";
                    }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    {
                        result += "E";
                    }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    {
                        result += "F";
                    }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    {
                        result += "G";
                    }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    {
                        result += "H";
                    }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    {
                        result += "J";
                    }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    {
                        result += "K";
                    }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    {
                        result += "L";
                    }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    {
                        result += "M";
                    }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    {
                        result += "N";
                    }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    {
                        result += "O";
                    }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    {
                        result += "P";
                    }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    {
                        result += "Q";
                    }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    {
                        result += "R";
                    }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    {
                        result += "S";
                    }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    {
                        result += "T";
                    }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    {
                        result += "W";
                    }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    {
                        result += "X";
                    }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    {
                        result += "Y";
                    }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    {
                        result += "Z";
                    }
                    else
                    {
                        result += "?";
                    }

                    i += 2;
                }
            }

            return result;
        }
        #endregion

    }
}
