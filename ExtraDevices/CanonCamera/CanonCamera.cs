using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using ExtraLibrary.InputOutput;
using EDSDKLib;

namespace ExtraDevices.CanonCamera
{
    public class CanonCamera
    {
        private SDKHandler cameraHandler;

        public event SDKHandler.ProgressHandler ProgressChanged;
        public event SDKHandler.CameraAddedHandler CameraAdded;
        public event SDKHandler.StreamUpdate LiveViewUpdated;

        public event CameraVideoUpdated VideoUpdated;       

        public CanonCamera()
        {
            try
            {
                this.cameraHandler = new SDKHandler();
                this.cameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(this.CameraHandler_CameraAdded);
                this.cameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(this.CameraHandler_LiveViewUpdated);
                this.cameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(this.CameraHandler_ProgressChanged);
                this.cameraHandler.CameraHasShutdown += new EventHandler(this.CameraHandler_CameraHasShutdown);
            }
            catch (Exception exception)
            {

            }
        }


        public void Dispose()
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.Dispose();
            }
        }


        public string MainCameraName
        {
            get
            {
                string cameraName = null;
                if ( this.cameraHandler != null && cameraHandler.MainCamera != null)
                {
                    cameraName = this.cameraHandler.MainCamera.Info.szDeviceDescription;
                }
                return cameraName;
            }
        }
        
        private void CameraHandler_CameraAdded()
        {
            if (this.CameraAdded != null)
            {
                this.CameraAdded();
            }
        }

        private void CameraHandler_LiveViewUpdated(Stream image)
        {
            try
            {
                if (this.cameraHandler != null && this.cameraHandler.IsLiveViewOn)
                {
                    using (WrappingStream wrappingStream = new WrappingStream(image))
                    {
                        BitmapImage evfImage = this.CreateImageFromStream(image, wrappingStream);
                        if (this.VideoUpdated != null)
                        {
                            this.VideoUpdated(evfImage);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }

        private BitmapImage CreateImageFromStream(Stream imageStream, WrappingStream wrappingStream)
        {
            imageStream.Position = 0;
            BitmapImage evfImage = new BitmapImage();
            evfImage.BeginInit();
            evfImage.StreamSource = wrappingStream;
            evfImage.CacheOption = BitmapCacheOption.OnLoad;
            evfImage.EndInit();
            evfImage.Freeze();

            return evfImage;
        }
        
        private void CameraHandler_ProgressChanged(int progress)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(progress);
            }
        }

        private void CameraHandler_CameraHasShutdown(object sender, EventArgs eventArgs)
        {
            
        }

        public List<Camera> GetCameraList()
        {
            List<Camera> list = null;
            if (this.cameraHandler != null)
            {
                list = this.cameraHandler.GetCameraList();
            }
            return list;
        }

        public void OpenSession(Camera camera)
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.OpenSession(camera);
            }
        }

        public void CloseSession()
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.CloseSession();
            }
        }

        public void SetAperturePriority(string value)
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV(value));
            }
        }

        public void SetShutterPriority(string value)
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV(value));
            }
        }
        
        public void SetISO(string value)
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO(value));
            }
        }
        
        public void SetWhiteBalance(uint value)
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, value); 
            }
        }

        public void TakePhoto()
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.TakePhoto();
            }
        }
        
        public void StartVideo()
        {
            if (this.cameraHandler != null && !this.cameraHandler.IsLiveViewOn)
            {
                this.cameraHandler.StartLiveView();
            }
        }

        public void StopVideo()
        {
            if (this.cameraHandler != null)
            {
                this.cameraHandler.StopLiveView();
            }
        }

        public List<int> GetAperturePriorityList()
        {
            List<int> aperturePriorityList = null;
            if (this.cameraHandler != null)
            {
                aperturePriorityList =  this.cameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
            }

            return aperturePriorityList;
        }

        public List<int> GetShutterPriorityList()
        {
            List<int> shutterPriorityList = null;
            if (this.cameraHandler != null)
            {
                shutterPriorityList = this.cameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
            }

            return shutterPriorityList;
        }


        public List<int> GetISOList()
        {
            List<int> isoList = null;
            if (this.cameraHandler != null)
            {
                isoList = this.cameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);
            }

            return isoList;
        }

        public uint GetMode()
        {
            uint mode = 0;
            if (this.cameraHandler != null)
            {
                mode = this.cameraHandler.GetSetting(EDSDK.PropID_AEMode);
            }

            return mode;
        }

        public List<string> GetWhiteBalanceList()
        {
            List<string> whiteBalanceList = new List<string>() 
            {
                "Auto",
                "Daylight",
                "Cloudy",
                "Tungsten",
                "Fluorescent",
                "Strobe",
                "White Paper",
                "Shade"
            };

            return whiteBalanceList;
        }
    }
}
