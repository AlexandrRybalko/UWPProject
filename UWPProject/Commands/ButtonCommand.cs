using System;
using System.Windows.Input;

namespace UWPProject
{
    public class ButtonCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public ButtonCommand(Action execute)
            : this(execute, null)
        {
        }

        public ButtonCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }                
            
            this.execute = execute;
            this.canExecute = canExecute;
        }
    }
}
