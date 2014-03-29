namespace Vido.Parking.Ui.Wpf
{
  using MahApps.Metro.Controls;
  using System;
  using System.Windows;
  using System.Windows.Interop;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      IntPtr handle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
      this.DataContext = new ViewModels.MainViewModel(handle);
    }

  }
}
