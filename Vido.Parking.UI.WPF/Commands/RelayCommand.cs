namespace ViDoScanner.Commands
{
  using System;
  using System.Diagnostics;
  using System.Windows.Input;
  public class RelayCommand<T>:ICommand
  {
    #region Data Members
    private readonly Action<T> execute = null;
    private readonly Predicate<T> canExecute = null;
    #endregion

    #region Constructors
    public RelayCommand(Action<T> execute)
      :this(execute, null)
    { }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");
      
      this.execute = execute;
      this.canExecute = canExecute;
    }
    #endregion

    #region ICommand Members
    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return (canExecute == null ? true : canExecute((T)parameter));
    }
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
    public void Execute(object parameter)
    {
      execute((T)parameter);
    }
    #endregion // ICommand Members
  }
}
