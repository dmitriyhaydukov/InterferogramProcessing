using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using ExtraLibrary.Geometry3D;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraLibrary.InputOutput {
    public class SerializationManager {
        //--------------------------------------------------------------------------------------
        //Сериализация объекта в файл
        public static void SerializeObjectToFile( object obj, string fileName ) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream( fileName, FileMode.Create );
            binaryFormatter.Serialize( stream, obj );
            stream.Close();
        }
        //--------------------------------------------------------------------------------------
        //Десериализация объекта из файла
        public static object DeserializeObjectFromFile(string fileName) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream( fileName, FileMode.Open, FileAccess.Read );
            object obj = binaryFormatter.Deserialize( stream );
            stream.Close();
            return obj;
        }
        //--------------------------------------------------------------------------------------
        //Сериализация 3D точек в файл
        public static void SerializePointsToTextFile( Point3D[] points, string fileName ) {
            FileStream fileStream = new FileStream( fileName, FileMode.Create );
            StreamWriter writer = new StreamWriter( fileStream );

            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                string text = ( index + 1 ).ToString() + ". " + point.ToString();
                writer.WriteLine( text );
            }
            writer.Close();
            fileStream.Close();
        }
        //---------------------------------------------------------------------------------------
        //Сериализация векторов в файл
        public static void SerializeVectorsToTextFile( RealVector[] vectors, string fileName ) {
            FileStream fileStream = new FileStream( fileName, FileMode.Create );
            StreamWriter writer = new StreamWriter( fileStream );

            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector vector = vectors[ index ];
                string text = ( index + 1 ).ToString() + ". " + vector.ToString();
                writer.WriteLine( text );
            }
            writer.Close();
            fileStream.Close();
        }
        //---------------------------------------------------------------------------------------
        
        //---------------------------------------------------------------------------------------
    }
}
