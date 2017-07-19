using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ForCheng.CodeMaker;

namespace TestWpfApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {

        #region Private fields
        private readonly CheckCode _checkCode;
        private string _checkCodeValue;
        private bool _canTextChanged;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            _checkCode = new CheckCode(CodeType.All, 5);
            RefreshCheckCode();
            _canTextChanged = true;
        }

        #endregion

        #region Private event
        private void RefreshBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshCheckCode();
        }

        private void CheckBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = string.Equals(CheckTbx.Text, _checkCodeValue, StringComparison.CurrentCultureIgnoreCase) ? "输入验证码正确" : "输入验证码错误";
            MessageBox.Show(result);
        }

        private void Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_canTextChanged)
            {
                var text = Tbx.Text;
                int len;
                if (int.TryParse(text, out len) && len > 0)
                {
                    _checkCode.Length = len;
                }
                else
                {
                    _canTextChanged = false;
                    Tbx.Text = _checkCode.Length.ToString();
                    _canTextChanged = true;
                }
            }

        }

        #endregion

        #region Private methods
        private void RefreshCheckCode()
        {
            var newCode = _checkCode.NewCheckCode();
            _checkCodeValue = newCode.Item1;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(newCode.Item2);
            bitmap.EndInit();
            TargetImage.Source = bitmap;
            TargetImage.Width = bitmap.Width;
            TargetImage.Height = bitmap.Height;
        }

        #endregion








    }
}
