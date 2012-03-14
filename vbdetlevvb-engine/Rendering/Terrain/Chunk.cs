using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using ClipperLib;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using vbdetlevvb_engine.Interfaces;
using vbdetlevvb_engine.Rendering.Mesh;
using ClippingPolygon = System.Collections.Generic.List<ClipperLib.IntPoint>;
using ClippingPolygons = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
namespace vbdetlevvb_engine.Rendering.Terrain
{
    class Chunk
    {
        Vector2 ChunkPosition = Vector2.Zero;
        Logging.Logger log;
        List<VertexPosition[]> shape;
        vbdetlevvb_engine.Rendering.Camera.BasicCamera camera;
        Window window;
        List<ChunkDrawer> drawer;
        public Chunk(ref Window window, Vector2 position)
        {
            log = window.logger;
            camera = (vbdetlevvb_engine.Rendering.Camera.BasicCamera)window.camera;
            this.window = window;
            ChunkPosition = position;
            window.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            window.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);
            drawer = new List<ChunkDrawer>();
            drawer.Add(new ChunkDrawer(ref window.logger));
            shape = new List<VertexPosition[]>();
        }
        bool Erase = false;
        void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Erase = false;
            }
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Erase = true;
            }
        }

        public void OnUpdate()
        {
            if (Erase)
            {
                Vector2 mouse = new Vector2(window.Mouse.X, window.ClientRectangle.Height - window.Mouse.Y);
                mouse *= 1 / (camera.zoom / 100f);
                mouse /= 100;
                Vector2 pos = camera.pos;
                Vector2.Add(ref mouse, ref pos, out mouse);

                DoCircleClipping(mouse, 0.25f);
            }
        }
        Bitmap bitmap = new Bitmap("data/Textures/dirt.png");
        int texture;
        public void OnLoad()
        {

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);


            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);



            shape.Add(new VertexPosition[4]);
            shape[0][0] = new VertexPosition(3 + ChunkPosition.X, 3 + ChunkPosition.Y, 0);
            shape[0][1] = new VertexPosition(3 + ChunkPosition.X, 0 + ChunkPosition.Y, 0);
            shape[0][2] = new VertexPosition(0 + ChunkPosition.X, 0 + ChunkPosition.Y, 0);
            shape[0][3] = new VertexPosition(0 + ChunkPosition.X, 3 + ChunkPosition.Y, 0);
            drawer[0].BufferVertices(shape[0]);
        }

        static float accuracy = 1000;
        public void DoCircleClipping(Vector2 pos, float radius)
        {
            List<VertexPosition[]> shape_old = shape;
            bool isCleared = false;
            int sc = 0;
            for (sc = 0; sc < shape_old.Count; sc++)
            {
                ClippingPolygons subj = new ClippingPolygons(1);
                subj.Add(new ClippingPolygon(shape_old[sc].Length));
                foreach (VertexPosition point in shape_old[sc])
                {
                    subj[0].Add(new IntPoint((int)((point.Position.X) * accuracy), (int)((point.Position.Y) * accuracy)));
                }

                ClippingPolygons clip = new ClippingPolygons(1);
                clip.Add(new ClippingPolygon());
                for (int alpha = 0; alpha < 360; alpha += 10)
                {
                    clip[0].Add(new IntPoint((int)(((Math.Sin((alpha) * Math.PI / 180.0) * radius) + pos.X) * accuracy), (int)(((Math.Cos((alpha) * Math.PI / 180.0) * radius) + pos.Y) * accuracy)));
                    //log.Log(pos.ToString());
                }

                ClippingPolygons solution = new ClippingPolygons();

                Clipper c = new Clipper();
                c.AddPolygons(subj, PolyType.ptSubject);
                c.AddPolygons(clip, PolyType.ptClip);

                if (c.Execute(ClipType.ctDifference, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd))
                {
                    if (!isCleared)
                    {
                        shape = new List<VertexPosition[]>();
                        drawer.Clear();
                        isCleared = true;
                    }
                    
                    for (int f = 0; f < solution.Count; f++)
                    {

                        shape.Add(new VertexPosition[solution[f].Count]);
                        drawer.Add(new ChunkDrawer(ref log));


                        for (int i = 0; i < solution[f].Count; i++)
                        {

                            shape[shape.Count-1][i] = new VertexPosition(solution[f][i].X / accuracy, solution[f][i].Y / accuracy, 0);
                        }
                        drawer[shape.Count-1].BufferVertices(shape[shape.Count - 1]);
                    }

                }
            }

        }

        public void OnRender()
        {
            //IntPtr tess = Glu.NewTess();
            //Glu.TessCallback(tess, TessCallback.TessBegin, 
            //Glu.BeginPolygon(tess);

            //Glu.EndPolygon(tess);
            for (int i = 0; i < drawer.Count; i++)
            {
                drawer[i].OnDraw();
            }
        }

        public void OnDispose()
        {
            GL.DeleteTextures(1, ref texture);

            OnDispose();
        }

    }
}
