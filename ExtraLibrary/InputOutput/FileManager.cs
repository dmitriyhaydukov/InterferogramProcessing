using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.InputOutput {
    public class FileManager {
        //----------------------------------------------------------------------------------------------
        //Полные имена файлов
        public static string[] GetFullFileNames( string directoryPath, string[] fileNames ) {
            string[] fullFileNames = new string[ fileNames.Length ];
            for ( int index = 0; index < fileNames.Length; index++ ) {
                string fileName = fileNames[ index ];
                string fullFileName = directoryPath + fileName;
                fullFileNames[ index ] = fullFileName;
            }
            return fullFileNames;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
