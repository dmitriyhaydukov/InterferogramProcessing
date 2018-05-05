using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDSDKLib;

namespace ExtraDevices.CanonCamera
{
    public class CanonCameraSettingsHelper
    {
        private static Dictionary<int, string> whiteBalanceDictionary;
        
        static CanonCameraSettingsHelper()
        {
            CanonCameraSettingsHelper.whiteBalanceDictionary = CanonCameraSettingsHelper.CreateWhiteBalanceDictionary();
        }

        private static Dictionary<int, string> CreateWhiteBalanceDictionary()
        {
            Dictionary<int, string> whiteBalanceDictionary = new Dictionary<int, string>();
            
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Auto, "Auto");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Daylight, "Daylight");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Tangsten, "Tangsten");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Fluorescent, "Fluorescent");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Strobe, "Strobe");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_WhitePaper, "WhitePaper");
            whiteBalanceDictionary.Add(EDSDK.WhiteBalance_Shade, "Shade");

            return whiteBalanceDictionary;
        }
        
        public static Dictionary<int, string> GetWhiteBalanceDictionary()
        {
            return CanonCameraSettingsHelper.whiteBalanceDictionary;
        }
    }
}
