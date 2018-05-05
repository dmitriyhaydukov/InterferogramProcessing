using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ExtraMVVM {
    public abstract class BaseViewModel: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        //------------------------------------------------------------------------------------------------
        protected virtual void RaisePropertyChanged( string propertyName ) {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if ( handler != null ) {
                PropertyChangedEventArgs eventArgs = new PropertyChangedEventArgs( propertyName );
                handler( this, eventArgs );
            }
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
