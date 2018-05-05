using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canon.Eos.Framework;
using Canon.Eos.Framework.Eventing;
//using Emgu.CV;
//using Emgu.CV.Structure;
//using Emgu.Util;
using System.Threading;
using System.IO;

public delegate void ImageReceived( Image newImage );

namespace Interferometry.DeviceControllers {
    class ImageGetter
    {
        public event ImageReceived imageReceived;

        private readonly FrameworkManager _manager;
        EosCamera camera;
        bool singleShotInProgress = false;
        bool cameraLoaded = false;


//Birth
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageGetter()
        {
            _manager = new FrameworkManager();

            this.loadFunction();
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



//Public Methods
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void getImage()
        {
            if (singleShotInProgress == true)
            {
                return;
            }

            singleShotInProgress = true;
            this.safeCall(() => { if (camera != null)  camera.TakePicture(); }, ex => MessageBox.Show(ex.ToString()));
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




//Inner Methods
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadFunction()
        {
            this.StartUp();
            if (cameraLoaded == false)
            {
                this.loadCameras();
                cameraLoaded = true;
            }
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void StartUp()
        {
            this.safeCall(() => { _manager.LoadFramework(); }, ex => { MessageBox.Show(ex.ToString());});
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadCameras()
        {
            foreach (var _camera in _manager.GetCameras())
            {
                _camera.PictureTaken += this.imageTaken;
                camera = _camera;
            }
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void safeCall(Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(object sender, EosImageEventArgs e)
        {
            if (singleShotInProgress == true)
            {
                singleShotInProgress = false;
            }

            //изображение получено
            imageReceived(e.GetImage());
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
