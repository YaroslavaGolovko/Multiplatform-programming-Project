using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Services
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }
        public static TextBlock MainTextBlock { get; set; }
        public static Button BtnBack { get; set; }
        public static void ShowMessage()
        {
            MessageBox.Show("Данная функция находится в разработке!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static byte[] SelectImage(Image image)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();

            byte[] BinaryData = new byte[] { };

            if (FileDialog.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(FileDialog.FileName));
                BinaryData = System.IO.File.ReadAllBytes(FileDialog.FileName);
            }
            return BinaryData;
        }

        public static bool CheckInputData(string name, string cost)
        {
            try
            {
                int sum = Int32.Parse(cost);
                if (name.Length < 8)
                    return false;
                if (sum <= 0)
                return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
