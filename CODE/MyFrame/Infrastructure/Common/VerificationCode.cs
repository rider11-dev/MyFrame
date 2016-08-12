using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Common
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class VerificationCodeHelper
    {
        public static int DefaultLength = 4;

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="length">二维码长度，默认4个字符</param>
        /// <returns></returns>
        public static VerificationCode Create(int length = 4)
        {
            if (length < 1)
            {
                throw new ArgumentException("二维码长度必须大于1", "length");
            }

            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string code = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                code += validateNums[i].ToString();
            }

            return BuildImage(code);
        }

        private static VerificationCode BuildImage(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("二维码字符不正确", "code");
            }
            VerificationCode entity = new VerificationCode { Code = code };

            int width = (int)Math.Ceiling(code.Length * 12.0);
            int height = 22;
            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(width);
                    int x2 = random.Next(width);
                    int y1 = random.Next(height);
                    int y2 = random.Next(height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, width, height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(code, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(width);
                    int y = random.Next(height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, width - 1, height - 1);

                //保存图片数据
                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    entity.ImageBytes = stream.ToArray();
                }
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }

            return entity;
        }
    }

    /// <summary>
    /// 二维码信息实体类
    /// </summary>
    public struct VerificationCode
    {
        /// <summary>
        /// 二维码字符串
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 二维码图片字节数组
        /// </summary>
        public byte[] ImageBytes { get; set; }

        public bool Check()
        {
            return !string.IsNullOrEmpty(Code) && ImageBytes != null && ImageBytes.Length > 0;
        }
    }
}
