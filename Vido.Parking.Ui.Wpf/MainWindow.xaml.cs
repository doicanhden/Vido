using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vido.Parking.Ui.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      var dt = new ViewModels.LaneViewModel()
      {
        LaneCode = "0001",
        Message = "Xin Chào",
        UserData = "59S121893",
        CardID = "0123456"
      };
      dt.BackImageSaved = System.Drawing.Bitmap.FromFile(@"F:\Khanh\Desktop\Hình0299.jpg");
      dt.FrontImageSaved = System.Drawing.Bitmap.FromFile(@"F:\Khanh\Desktop\Hình0299.jpg");
      dt.FrontImageCamera = System.Drawing.Bitmap.FromFile(@"F:\Khanh\Desktop\report.jpg");
      dt.BackImageCamera = System.Drawing.Bitmap.FromFile(@"F:\Khanh\Desktop\report.jpg");
      TestLane1.DataContext = dt;
    }
  }
}
