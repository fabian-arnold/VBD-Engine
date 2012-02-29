#region --- Using directives ---

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using vbdetlevvb_engine.Rendering.Camera;
using vbdetlevvb_engine.Rendering.VBO;
using vbdetlevvb_engine.Rendering.Mesh;
using vbdetlevvb_engine.Rendering.Terrain;
using vbdetlevvb_engine.Logging;

#endregion

namespace vbdetlevvb_engine
{
    
    public class Window : GameWindow
    {
        private bool wireframe = true;
        public Rendering.Camera.Camera camera;


        //Shape testquad;
        Terrainnew terrain;
        public Logger logger;
        public delegate void ChangedEventHandler(Window sender);
        public event ChangedEventHandler update;

        public Window() : base(800, 600)
        {
            logger = new Logger(this);
            camera = new BasicCamera(this);
            terrain = new Terrainnew(this);
            
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            terrain.OnLoad();
            camera.OnLoad(Height, Width);
            string version = GL.GetString(StringName.Version);
            int major = (int)version[0];
            int minor = (int)version[2];
            if (major <= 1 && minor < 5)
            {
                System.Windows.Forms.MessageBox.Show("You need at least OpenGL 1.5 to run the Application. Aborting.", "VBOs not supported",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                this.Exit();
            }

            GL.ClearColor(System.Drawing.Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            Keyboard.KeyUp += KeyUp;
            
           
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            camera.DoResize(Height, Width);
           
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            logger.Update();
            terrain.OnUpdate();
            Title = "FPS: " + (1 / e.Time).ToString("0");
            if (Keyboard[OpenTK.Input.Key.Escape])
                this.Exit();

            

            
        }

        private void KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.F3)
            {
                //this.wireframe = !wireframe;
                //Console.WriteLine("Toggle Wireframe: " + wireframe);
            } 
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //update(this);


            if (wireframe)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            camera.Update();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = camera.lookatMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            terrain.OnRender();

            

            SwapBuffers();
        }

        public override void Dispose()
        {
            base.Dispose();
            terrain.OnDispose();
        }

    }
}
