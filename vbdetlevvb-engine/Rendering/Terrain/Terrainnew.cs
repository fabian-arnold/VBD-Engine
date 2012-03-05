using System;
using System.Collections.Generic;
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
    class Terrainnew: Polygon
        
    {

        Logging.Logger log;
        VertexPosition[] shape;
        vbdetlevvb_engine.Rendering.Camera.BasicCamera camera;
        Window window;

        public Terrainnew(Window window): 
            base(ref window.logger)
        {
            log = window.logger;
            camera = (vbdetlevvb_engine.Rendering.Camera.BasicCamera)window.camera;
            this.window = window;

        }

        public void OnUpdate()
        {

            Vector2 mouse = new Vector2(window.Mouse.X, window.ClientRectangle.Height - window.Mouse.Y);
            mouse *= 1 / (camera.zoom / 100f);
            mouse /= 100;
            Vector2 pos = camera.pos;
            Vector2.Add(ref mouse, ref pos, out mouse);
            
            DoCircleClipping(mouse, 1);
            
        }
        
        public void OnLoad()
        {
           // SetDrawMode(OpenTK.Graphics.OpenGL.BeginMode.Triangles);
            shape = new VertexPosition[4];
            shape[0] = new VertexPosition(0,0,0);
            shape[1] = new VertexPosition(10, 0, 0);
            shape[2] = new VertexPosition(10, 10, 0);
            shape[3] = new VertexPosition(0, 10, 0);
            BufferVertices(ref shape);
        }

        static float accuracy = 1000;
        public void DoCircleClipping(Vector2 pos, float radius)
        {
            ClippingPolygons subj = new ClippingPolygons(1);
            subj.Add(new ClippingPolygon(shape.Length));
            foreach(VertexPosition point in shape){
                subj[0].Add(new IntPoint((int)((point.Position.X) * accuracy), (int)((point.Position.Y) * accuracy)));
            }
                   
            ClippingPolygons clip = new ClippingPolygons(1);
            clip.Add(new ClippingPolygon());
            for (int alpha = 0; alpha < 360; alpha += 10)
            {
                clip[0].Add(new IntPoint((int)(((Math.Sin((alpha) * Math.PI / 180.0) * radius)+pos.X) * accuracy), (int)(((Math.Cos((alpha) * Math.PI / 180.0) * radius)+pos.Y) * accuracy)));
                //log.Log(pos.ToString());
            }      

            ClippingPolygons solution = new ClippingPolygons();

            Clipper c = new Clipper();
            c.AddPolygons(subj, PolyType.ptSubject);
            c.AddPolygons(clip, PolyType.ptClip);
            
            if (c.Execute(ClipType.ctDifference, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd))
            {
                
                for (int f = 0; f < solution.Count; f++)
                {
                   
                    shape = new VertexPosition[solution[f].Count];
                    for(int i = 0; i < solution[f].Count; i++){
                  
                        shape[i] = new VertexPosition(solution[f][i].X / accuracy, solution[f][i].Y / accuracy, 0);
                    }
                    
                }
            }
            BufferVertices(ref shape);
        }

        public void OnRender()
        {
            //IntPtr tess = Glu.NewTess();
            //Glu.TessCallback(tess, TessCallback.TessBegin, 
            //Glu.BeginPolygon(tess);

            //Glu.EndPolygon(tess);
            OnDraw();
        }

        public void OnDispose()
        {
            OnDispose();
        }

    }
}
