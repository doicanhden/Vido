using System.Data;
using System.Windows;
using System.Windows.Input;
namespace Vido.Parking.Ui.Wpf.ViewModels
{
  public class LaneManagementViewModel
  {
    private MainViewModel mainViewModel;
    private ICommand saveCommand = null;
    private ICommand exitCommand = null;

    public LaneManagementViewModel(MainViewModel mainViewModel)
    {
      this.mainViewModel = mainViewModel;
      this.LaneConfigs = mainViewModel.Settings.LaneConfigs;
    }

    public DataTable LaneConfigs { get; set; }

    public ICommand SaveCommand
    {
      get
      {
        return (saveCommand ?? (saveCommand =
          new Commands.RelayCommand(SaveExecute, SaveCanExecute)));
      }
    }
    private void SaveExecute(object obj)
    {
      this.mainViewModel.Settings.Save();

      var view = obj as Window;
      view.Close();
    }

    private bool SaveCanExecute(object obj)
    {
      foreach (DataRow dr in LaneConfigs.Rows)
      {
        foreach (DataColumn col in LaneConfigs.Columns)
        {
          if (string.IsNullOrWhiteSpace(dr[col.ColumnName].ToString()))
          {
            return (false);
          }
        }
      }
      return (true);
    }

    public ICommand ExitCommand
    {
      get
      {
        return (exitCommand ?? (exitCommand =
          new Commands.RelayCommand(ExitExecute)));
      }
    }

    private void ExitExecute(object obj)
    {
      var view = obj as Window;
      view.Close();
    }
  }
}
