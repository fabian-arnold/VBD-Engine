/*
 * (c)by Fabian Arnold
 */
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace vbdetlevvb_engine.Camera
{
    public class BasicCamera: Camera
    {
        GameWindow gw;
        public BasicCamera(GameWindow gw)
        {
            this.gw = gw;
            gw.Keyboard.KeyDown += KeyDown;
            gw.Keyboard.KeyUp += KeyUp;
        }
        Matrix4 perpective;
        Matrix4 lookat;
        public Vector2 pos;
        Vector2 movement;
        float speed= 10;
        public override void DoResize(int Height, int Width)
        {
            float aspect_ratio = Width / (float)Height;
            perpective = Matrix4.Identity; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
            GL.Ortho( 0, Width, 0, Height, 200, -200 );
        }

        public override OpenTK.Matrix4 perpectiveMatrix()
        {
            return perpective;
        }

        public override OpenTK.Matrix4 lookatMatrix()
        {
            return lookat;
        }
        public float zoom = 200;
        public override void Update()
        {
            //lookat = Matrix4.LookAt(new Vector3(pos.X,pos.Y, 10), new Vector3(pos.X,pos.Y,0), Vector3.UnitY);
            lookat = Matrix4.Identity;
            //Matrix4.CreateOrthographicOffCenter( -1, 1, -1, 1, 0.01f, 100 );
            
            lookat *= Matrix4.CreateTranslation( -pos.X, -pos.Y, -1 );
            lookat *= Matrix4.Scale(zoom);

            pos += (movement*speed);
        }
        private void KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Up)
            {
                movement -= new Vector2(0, 0.01f);
            }
            if (e.Key == OpenTK.Input.Key.Down)
            {
                movement -= new Vector2(0, -0.01f);
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                movement -= new Vector2(-0.01f, 0.0f);
            }
            if (e.Key == OpenTK.Input.Key.Right)
            {
                movement -= new Vector2(0.01f, 0.0f);
            }
        }
        private void KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            
            if (e.Key == OpenTK.Input.Key.Up)
            {
                movement += new Vector2(0, 0.01f);
            }
            if (e.Key == OpenTK.Input.Key.Down)
            {
                movement += new Vector2(0, -0.01f);
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                movement += new Vector2(-0.01f, 0.0f);
            }
            if (e.Key == OpenTK.Input.Key.Right)
            {
                movement += new Vector2(0.01f, 0.0f);
            }

        }

        public override void OnLoad(int Height, int Width)
        {
            
            lookat = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);
        }
    }
}
