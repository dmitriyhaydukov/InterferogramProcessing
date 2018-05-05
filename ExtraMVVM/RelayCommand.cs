using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace ExtraMVVM {
    public class RelayCommand : ICommand {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged;
        //------------------------------------------------------------------------------------------------
        public RelayCommand( Action<object> execute, Predicate<object> canExecute ) {
            if ( execute == null ) {
                throw new ArgumentNullException( "execute" );
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        public void Execute( object parameter ) {
            this.execute( parameter );
        }
        //------------------------------------------------------------------------------------------------
        public bool CanExecute( object parameter ) {
            return this.canExecute == null ? true : this.canExecute( parameter );
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
