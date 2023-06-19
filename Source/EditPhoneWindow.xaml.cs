using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.WebRequestMethods;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for EditPhoneWindow.xaml
    /// </summary>

    public partial class EditPhoneWindow : Window
    { 
        public Phone EditedPhone { get; set; }

        private string _name = "";
        private string _price = "";
        private string _manu = "";
        private string _image = "";
        private string _newImage = "";

        public EditPhoneWindow(Phone old)
        {
            InitializeComponent();

            EditedPhone = old;
            //EditedPhone = (Phone)old.Clone(); //my VS has some error with this clone
            this.DataContext = old;
            _name = old.Name;
            _price = old.Price;
            _manu = old.Manufacturer;
            _image = old.Image;
        }
        
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (_newImage == "")
            {
                EditedPhone.Image = _image;
            }
            else
            {
                EditedPhone.Image = _newImage;
            }
            MessageBox.Show("Bạn đã cập nhật thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            var result = MessageBox.Show($"Bạn thật sự muốn hủy cập nhật ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                EditedPhone.Name = _name;
                EditedPhone.Price = _price;
                EditedPhone.Manufacturer = _manu;
                EditedPhone.Image = _image;
            }
            else
            {
                MessageBox.Show("Bạn đã cập nhật thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            DialogResult = true;
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
                //    var info = new FileInfo(screen.FileName);

                //    var folder = AppDomain.CurrentDomain.BaseDirectory;
                //    var imagesFolder = $"{folder}Images";
                //    var newLocation = $"{imagesFolder}\\{info.Name}";

                //    File.Copy(screen.FileName, newLocation);
                this.Activate();
                this.Topmost = false;
                imageEdit.ImageSource = new BitmapImage(new Uri(screen.FileName));
                _newImage = screen.FileName;
            }
        }
    }
}
