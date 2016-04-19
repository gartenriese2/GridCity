namespace GridCity.GUI {

    using System;
    using System.Windows.Input;

    internal class RelayCommand : ICommand {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        private readonly Action<object> executeAction;

        private readonly Predicate<object> canExecutePredicate;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }

            executeAction = execute;
            canExecutePredicate = canExecute;
        }

        //---------------------------------------------------------------------
        // Events
        //---------------------------------------------------------------------
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public bool CanExecute(object parameters) {
            return canExecutePredicate == null ? true : canExecutePredicate(parameters);
        }

        public void Execute(object parameters) {
            executeAction(parameters);
        }
    }
}
