using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;
using System.Collections.Generic;

namespace UserInterfaceHelping
{
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public partial class OpenGLForm : Form
    {
        bool loaded = false;

        float AngleY = 0;
        float AngleZ = 0;
        float AngleX = 0;

        List<Point3D> listOfPoints = new List<Point3D>();

        //Interface Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void addPoint(Point3D newPoint)
        {
            listOfPoints.Add(newPoint);
            glControl1.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Inner Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public OpenGLForm()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.White);
            SetupViewport();

            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            var m = Matrix4.CreatePerspectiveFieldOfView(3.1415f / 4, w / (float)h, 1, 5000);
            GL.LoadMatrix(ref m);
            GL.Viewport(0, 0, w, h);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
            {
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            var m = Matrix4.LookAt(180, 130, 130, 0, 0, 0, 0, 1, 0);
            GL.LoadMatrix(ref m);

            GL.Rotate(AngleX, 1.0, 0.0, 0.0);
            GL.Rotate(AngleY, 0.0, 1.0, 0.0);
            GL.Rotate(AngleZ, 0.0, 0.0, 1.0);            

            GL.Color3(1f, 0f, 0f);      //red - X
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1000, 0, 0);
            GL.End();

            GL.Color3(0f, 1f, 0f);      //green - Y
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1000, 0);
            GL.End();

            GL.Color3(0f, 0f, 1f);      //blue - Z
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1000);
            GL.End();


            GL.Color3(1f, 0f, 0f);


            /*GL.Begin(BeginMode.Triangles);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 50, 0);
            GL.Vertex3(20, 0, 0);
            GL.End();*/

            GL.Begin(BeginMode.Points);
            foreach (Point3D currentPoint in listOfPoints)
            {
                GL.Vertex3(currentPoint.x, currentPoint.y, currentPoint.z);
            }
            GL.End();

            glControl1.SwapBuffers();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            float delte = 1.5f;
            if (e.KeyCode == Keys.W)
            {
                AngleZ = AngleZ + delte;
            } 
            else if (e.KeyCode == Keys.S)
            {
                AngleZ = AngleZ - delte;
            }
            else if (e.KeyCode == Keys.A)
            {
                AngleY = AngleY - delte;
            }
            else if (e.KeyCode == Keys.D)
            {
                AngleY = AngleY + delte;
            }

            glControl1.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class FloatPoint3D {
        public int x;
        public int y;
        public int z;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public FloatPoint3D( int newX, int newY, int newZ ) {
            x = newX;
            y = newY;
            z = newZ;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Point3D {
        public int x;
        public int y;
        public int z;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Point3D( int newX, int newY, int newZ ) {
            x = newX;
            y = newY;
            z = newZ;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
