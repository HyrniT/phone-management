using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for AddPhoneWindow.xaml
    /// </summary>
    public partial class AddPhoneWindow : Window
    {
        private string _newImage = "";
        public Phone NewPhone { get; set; }
        public AddPhoneWindow()
        {
            InitializeComponent();
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            _newImage = $"{folder}/Images/default.png";
            imageAdd.ImageSource = new BitmapImage(new Uri(_newImage));
        }

        //private void Window_Deactivated(object sender, EventArgs e)
        //{
        //    this.Activate();
        //}

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameAdd.Text;
            string price = priceAdd.Text;
            string manu = manuAdd.Text;

            if(name=="")
            {
                name = $"Điện thoại mới";
            }
            if(price=="")
            {
                price = "0";
            }

            NewPhone = new Phone()
            {
                Name = name,
                Price = price,
                Manufacturer = manu,
                Image = _newImage
            };
            MessageBox.Show("Bạn thêm thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Do nothing
        }
        private void chooseButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CommonOpenFileDialog();
            screen.IsFolderPicker = false; // khong doc thu muc chi doc tap tin
            
            screen.Filters.Add(
               new CommonFileDialogFilter("PNG images", "*.png")
            );

            screen.Filters.Add(
               new CommonFileDialogFilter("JPEG images", "*.jpg, *.jpeg")
            );

            if (screen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.Topmost = false;
                //    var info = new FileInfo(screen.FileName);

                //    var folder = AppDomain.CurrentDomain.BaseDirectory;
                //    var imagesFolder = $"{folder}Images";
                //    var newLocation = $"{imagesFolder}\\{info.Name}";

                //    File.Copy(screen.FileName, newLocation);
                imageAdd.ImageSource = new BitmapImage(new Uri(screen.FileName));
                _newImage = screen.FileName;

                this.Activate();
                this.Topmost= true;
            }
        }
    }
}
