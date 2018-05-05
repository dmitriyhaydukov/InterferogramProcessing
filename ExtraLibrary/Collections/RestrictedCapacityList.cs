using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Collections {
    //Список с ограниченной емкостью
    //При добавлении элемента и превышении емкости - первый элемент удаляется
    public class RestrictedCapacityList<T> {
        private List<T> list;
        private int capacity;
        //-------------------------------------------------------------------------------------------
        //Емкость
        public int Capacity {
            get {
                return this.capacity;
            }
            private set {
                this.capacity = value;
            }
        }

        //-------------------------------------------------------------------------------------------
        public RestrictedCapacityList( int capacity ) {
            this.Capacity = capacity;
            this.list = new List<T>();
        }
        //-------------------------------------------------------------------------------------------
        //Добавление элемента
        public void AddItem( T item ) {
            if ( this.list.Count == this.Capacity ) {
                this.list.RemoveAt( 0 );
            }
            this.list.Add( item );
        }
        //-------------------------------------------------------------------------------------------
        //Удаление элемента
        public void RemoveItem( T item ) {
            this.list.Remove( item );
        }
        //-------------------------------------------------------------------------------------------
        public T GetItem( int index ) {
            return this.list[ index ];
        }
        //-------------------------------------------------------------------------------------------
        //Индексатор
        public T this[ int index ] {
            get {
                return this.list[ index ];
            }
            set {
                this.list[ index ] = value;
            }
        }
        //-------------------------------------------------------------------------------------------
    }
}
