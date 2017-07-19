using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ForCheng.CodeMaker
{
    /// <summary>
    /// 生成图片验证码的类
    /// </summary>
    public class CheckCode
    {

        #region Private fields

        private int _length;

        #endregion

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codeType">验证码类型</param>
        /// <param name="length">验证码长度</param>
        public CheckCode(CodeType codeType, int length)
        {
            CodeType = codeType;
            Length = length;
        }

        #endregion

        #region Public properties
 
        /// <summary>
        /// 验证长度
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (value > 0)
                {
                    _length = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(Length),value,"值应该大于0.");
                }
            }
        }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public CodeType CodeType { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// 获取一个新的验证码
        /// </summary>
        /// <returns>string为验证码的值，byte[]为验证码图像的字节流</returns>
        public Tuple<string, byte[]> NewCheckCode()
        {
            var value = CreateValue(CodeType, _length);
            var bytes = CreateGraphic(value);
            return new Tuple<string, byte[]>(value, bytes);
        }

        #endregion


        #region static methods
        /// <summary>
        /// 生成验证码字符串
        /// </summary>
        /// <param name="codeType">验证码的类型</param>
        /// <param name="length">验证码的长度</param>
        /// <returns></returns>
        public static string CreateValue(CodeType codeType, int length)
        {
            //设置模板
            var checkCodeTemplate = string.Empty;
            if ((codeType & CodeType.Number) != CodeType.None)
            {
                checkCodeTemplate += "1234567890";
            }
            if ((codeType & CodeType.LowerChar) != CodeType.None)
            {
                checkCodeTemplate += "abcdefghijklmnopqrstuvwxyz";
            }
            if ((codeType & CodeType.UpperChar) != CodeType.None)
            {
                checkCodeTemplate += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            var checkCodeStr = string.Empty;
            var n = checkCodeTemplate.Length - 1;
            var random = new Random(unchecked((int)DateTime.Now.Ticks)); //设置随机值生成器

            //生成随机验证码
            for (var i = 0; i < length; i++)
            {
                var beginSeek = random.Next(0, n);
                checkCodeStr += checkCodeTemplate.Substring(beginSeek, 1);
            }
            return checkCodeStr;
        }

        /// <summary>
        /// 生成验证码图像数据
        /// </summary>
        /// <param name="value">验证码字符串</param>
        /// <returns></returns>
        public static byte[] CreateGraphic(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = string.Empty;
            }
            
            var font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
            var imageWidth = (value.Length + 2) * font.Size;
            var imageHeight = font.Height + 6;
            var image = new Bitmap((int)imageWidth, imageHeight);
            var g = Graphics.FromImage(image);
            try
            {
                var random = new Random(unchecked((int)DateTime.Now.Ticks)); //生成随机生成器
                g.Clear(Color.White); //清空图片背景色

                //画图片的干扰线
                for (var i = 0; i < 25; i++)
                {
                    var x1 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var x2 = random.Next(image.Width);
                    var y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                
                var rect = new Rectangle(0, 0, image.Width, image.Height);
                var brush = new LinearGradientBrush(rect, Color.Blue, Color.DarkRed, 1.2f, true);
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(value, font, brush, rect, format);
                
                //画图片的前景干扰点
                for (var i = 0; i < 100; i++)
                {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);  
                
                //保存图片数据
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    return stream.ToArray();
                }
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion



    }
}
