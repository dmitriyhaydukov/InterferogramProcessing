using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ExtraWPF {
    //Инфрмация о свойстве зависимости
    public class DependencyPropertyInfo {
        //---------------------------------------------------------------------------------------
        public string PropertyName {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        public Type DataType {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        public Type OwnerDataType {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        public PropertyChangedCallback PropertyChangedHandler {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        public DependencyPropertyInfo() {
            //--
        }
        //---------------------------------------------------------------------------------------
        public DependencyPropertyInfo(
            string propertyName,
            Type dataType,
            Type ownerDataType,
            PropertyChangedCallback propertyChangedHandler
        ) {
            this.PropertyName = propertyName;
            this.DataType = dataType;
            this.OwnerDataType = ownerDataType;
            this.PropertyChangedHandler = propertyChangedHandler;
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
    }
}
