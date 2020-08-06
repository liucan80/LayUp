using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxplc_comm
{
   public static class CustomMath
    {
       /// <summary>
       /// int转化为byte数组
       /// </summary>
       /// <param name="integer"></param>
       /// <param name="resultSize"></param>
       /// <returns></returns>
        public static byte[] ToBinaryBits(this int integer, int resultSize )
        {
            var result = new byte[resultSize];

            var binaryString = Convert.ToString(integer, 2);
            // 先把string转成char数组，再将char数组中每一个值转为byte（需要减去char类型0的值，即48）。
            var binaryBits = binaryString.ToCharArray().Select(i => (byte)(i - 48)).ToArray();
            var binarySize = binaryBits.Length;

            if (binarySize > resultSize)
            {
                throw new ArgumentException("设置的结果数组容量不足。");
            }

            Array.Copy(binaryBits, 0, result, resultSize - binaryBits.Length, binaryBits.Length);
            return result;
        }

    }
}
