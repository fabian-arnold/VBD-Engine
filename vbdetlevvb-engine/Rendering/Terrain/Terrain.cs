using System;
using System.Collections.Generic;
using OpenTK;
using vbdetlevvb_engine.Camera;
using vbdetlevvb_engine.Rendering.Mesh;
using vbdetlevvb_engine.Rendering.Resources;
using vbdetlevvb_engine.Rendering.VBO;
using ClipperLib;

using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Polygons = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace vbdetlevvb_engine.Rendering.Terrain
{
   
    class Terrain
    {
        #region Declearations
            Window window;
            StaticMesh brush;
            Vbo terrain;
            VertexPosition[] vertices = new VertexPosition[6 * Width * Height];
            TextureCoords[] texcoords = new TextureCoords[6 * Width * Height];
            bool delte = false;
            Vector2[][] triangles = new Vector2[ 2 * Width * Height ][];
            public static byte Width = 5;
            public static byte Height = 5;
            Texture dirt;
        #endregion

        #region LoadUp
            public Terrain(Window window )
            {
                this.window = window;
                window.Mouse.ButtonDown += MouseDown;
                window.Mouse.ButtonUp += MouseUp;
                brush = new StaticMesh();
                terrain = new Vbo();
            
            }
            public void Load()
            {

                DateTime t = System.DateTime.Now;

                dirt = new Texture("dirt.png");
                GenerateTerrain();
                GenerateThread();
                FillAndBindBuffers();
                GenerateAndLoadBrush();

                window.logger.Log("Terrain","Generated in: " + (System.DateTime.Now - t).TotalMilliseconds + " Milliseconds");
            }
        #endregion

        #region Input
        private void MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs a )
        {
            if( a.Button == OpenTK.Input.MouseButton.Left )
            {
                delte = true;
                
            }


                
        }
        private void MouseUp( object sender, OpenTK.Input.MouseButtonEventArgs a )
        {
            if( a.Button == OpenTK.Input.MouseButton.Left )
            {
                delte = false;
            }
        }
    #endregion

        Vector2[][] brushtriangles;
        private void GenerateAndLoadBrush()
        {
            float brushsize=2f;
            brushtriangles = new Vector2[36][];
            short count = 0;
            for (int alpha = 0; alpha < 360; alpha += 10)
            {
                brushtriangles[count] = new Vector2[3];

                brushtriangles[count][0] = Vector2.Zero;
                brushtriangles[count][1] = new Vector2((float)Math.Sin((alpha) * Math.PI / 180.0) * brushsize, (float)Math.Cos(alpha * Math.PI / 180.0) * brushsize);
                brushtriangles[count][2] = new Vector2((float)Math.Sin((alpha + 10) * Math.PI / 180.0) * brushsize, (float)Math.Cos((alpha + 10) * Math.PI / 180.0) * brushsize);
                count++;
            }
           /* brushtriangles[0] = new Vector2[3];
            brushtriangles[0][0] = new Vector2(0, 0);
            brushtriangles[0][1] = new Vector2(0, 1 );
            brushtriangles[0][2] = new Vector2(1, 0);*/
            brush.Load(brushtriangles);
        }

        private void GenerateTerrain()
        {
            int c2 = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {


                    triangles[c2] = new Vector2[3];
                    triangles[c2][0] = new Vector2(x, y);
                    triangles[c2][1] = new Vector2(x + 1, y);
                    triangles[c2][2] = new Vector2(x + 1, y + 1);
                    c2++;
                    triangles[c2] = new Vector2[3];
                    triangles[c2][0] = new Vector2(x, y);
                    triangles[c2][1] = new Vector2(x, y + 1);
                    triangles[c2][2] = new Vector2(x + 1, y + 1);
                    c2++;



                }
            }
        }

        private void FillAndBindBuffers()
        {
            terrain.BufferVertices<VertexPosition>(ref vertices);
            terrain.BufferTexCoords<TextureCoords>(ref texcoords); 
            
        }

 
        public void GenerateThread()
        {
            System.DateTime renderbegin = System.DateTime.Now;
            vertices = new VertexPosition[triangles.Length * 6];
            texcoords = new TextureCoords[triangles.Length * 6];
           
            for (int i = 0; i < ((triangles.Length*3)-3); )
            {
                int c = (int)(i / 3);
                vertices[i] = new VertexPosition(triangles[c][0]);
                texcoords[i] = new TextureCoords(new Vector2(0,0));
                i++;
                vertices[i] = new VertexPosition(triangles[c][1]);
                texcoords[i] = new TextureCoords(new Vector2(1, 0));
                i++;
                vertices[i] = new VertexPosition(triangles[c][2]);
                texcoords[i] = new TextureCoords(new Vector2(1, 1));
                i++;
                c++;
                vertices[ i ] = new VertexPosition( triangles[c][0] );
                texcoords[i] = new TextureCoords(new Vector2(0, 0));
                i++;
                vertices[ i ] = new VertexPosition( triangles[c][1]);
                texcoords[i] = new TextureCoords(new Vector2(0, 1));
                i++;
                vertices[i] = new VertexPosition(triangles[c][2]);
                texcoords[i] = new TextureCoords(new Vector2(1, 1));
                i++;

                
            }

            TimeSpan delta = System.DateTime.Now - renderbegin;
            window.logger.Log("Terrain","Buffergenerationtime: " + delta.Milliseconds);

            
        }

        public void Draw()
        {
            UpdateMesh();
            FillAndBindBuffers();
            dirt.bind();
            terrain.Draw();
            brush.Draw();   
        }

        public void UpdateMesh()
        {
            if( delte )
            {
                SubtractSphere();
                GenerateThread();
            }
            //Console.WriteLine( trianglesnew.Count );
        }

        #region MeshSubtraction()

        public void SubtractSphere()
        {
            List<Vector2[]> trianglesnew = new List<Vector2[]>();
            for (int i = 0; i < (triangles.Length); i++)
            {

                

                for (int a = 0; a < brushtriangles.Length; a++)
                {
                    Vector2 p1 = triangles[i][0] * 1000;   //Das 3eck von dem abgezogen wird
                    Vector2 p2 = triangles[i][1] * 1000;   //       
                    Vector2 p3 = triangles[i][2] * 1000;   //

                    Vector2 brushP1, brushP2, brushP3;
                    brushP1 = brushtriangles[a][0]*1000;
                    brushP2 = brushtriangles[a][1]*1000;
                    brushP3 = brushtriangles[a][2]*1000;


                    Polygons subj = new Polygons(1);
                    subj.Add(new Polygon(3));
                    subj[0].Add(new IntPoint((int)p1.X, (int)p1.Y));
                    subj[0].Add(new IntPoint((int)p2.X, (int)p2.Y));
                    subj[0].Add(new IntPoint((int)p3.X, (int)p3.Y));
                   
                    Polygons clip = new Polygons(1);
                    clip.Add(new Polygon(3));
                    clip[0].Add(new IntPoint((int)brushP1.X, (int)brushP1.Y));
                    clip[0].Add(new IntPoint((int)brushP2.X, (int)brushP2.Y));
                    clip[0].Add(new IntPoint((int)brushP3.X, (int)brushP3.Y));
                   

                    

                    Polygons solution = new Polygons();

                    Clipper c = new Clipper();
                    c.AddPolygons(subj, PolyType.ptSubject);
                    c.AddPolygons(clip, PolyType.ptClip);
                    if (c.Execute(ClipType.ctDifference, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd))
                    {
                        
                        for (int f = 0; f < solution.Count; f++)
                        {
                            Vector2[] tmp = new Vector2[3];

                            tmp[0] = new Vector2(solution[f][0].X / 1000f, solution[f][0].Y / 1000f);
                            tmp[1] = new Vector2(solution[f][1].X / 1000f, solution[f][1].Y / 1000f);
                            tmp[2] = new Vector2(solution[f][2].X / 1000f, solution[f][2].Y / 1000f);
                            trianglesnew.Add(tmp);
                            Console.WriteLine(solution[0].Count);
                        }
                    }
                }
            }

            triangles = trianglesnew.ToArray();
            
        }


            private static List<T> RemoveDoubleItems<T>(ref List<T> list)
            {
                List<T> newList = new List<T>();
                Dictionary<T, string> keyList = new Dictionary<T, string>();

                foreach (T item in list)
                {
                    if (!keyList.ContainsKey(item))
                    {
                        keyList.Add(item, string.Empty);
                        newList.Add(item);
                    }
                }

                return newList;
            }
            public bool Schnittpunkt(ref Vector2 line1p1,ref Vector2 line1p2,ref Vector2 line2p1,ref Vector2 line2p2, out Vector2 schnittpunkt) 
            {
                //Console.WriteLine("Schnittpunkt");
                //1. Gleichung in Form von: y=mx+n erstellen
                //n=-m*x+y
                double m1 = ( line1p1.X - line1p2.X ) / ( line1p1.Y - line1p2.Y );
           
                double m2 = ( line2p1.X - line2p2.X ) / ( line2p1.Y - line2p2.Y );
                double n1 = ((-m1) * line1p1.X) + line1p1.Y;
            
                double n2 = ( ( -m2 ) * line2p1.X ) + line2p1.Y;
                //Console.WriteLine( "n1: " + n1 + " n2: " + n2 + " m1: " + m1 + " m2: " + m2 );
                double x = ( n2 - n1 ) / ( m1 - m2 );
                double y = ((m1*n2)-(m2*n1))/(m1-m2);
                Vector2 rtvalue = new Vector2((float)x, (float)y);

                float Xl = Math.Min( line1p1.X, line1p2.X );
                float Yl = Math.Min( line1p1.Y, line1p2.Y );

                float Xm = Math.Max( line1p1.X, line1p2.X );
                float Ym = Math.Max( line1p1.Y, line1p2.Y );
                schnittpunkt = rtvalue;
                if (rtvalue.X > Xl & rtvalue.Y > Yl & rtvalue.X < Xm & rtvalue.Y < Ym)
                {
                    return true;
                }
             

                return false;
            }

            #region PointInTriangle
                public static bool isPointInTriangle(ref Vector2 A, ref Vector2 B, ref Vector2 C, ref Vector2 P)
                {
                    if (A == P | B == P | C == P)
                    {
                        return true;
                    }

                    if( ((fAB(ref P, ref A, ref B)*fBC(ref P, ref B, ref C))>0) &( (fBC(ref P, ref B, ref C)*fCA(ref P, ref C, ref A))>0))
                        return true;
                    else
                        return false;
               
                }

                static float fAB(ref Vector2 p, ref Vector2 A, ref Vector2 B)
                {
                    return (p.Y - A.Y) * (B.X - A.X) - (p.X - A.X) * (B.Y - A.Y);
                }
                static float fCA(ref Vector2 p, ref Vector2 C, ref Vector2 A)
                {
                    return (p.Y - C.Y) * (A.X - C.X) - (p.X - C.X) * (A.Y - C.Y);
                }
                static float fBC(ref Vector2 p, ref Vector2 B, ref Vector2 C)
                {
                    return (p.Y - B.Y) * (C.X - B.X) - (p.X - B.X) * (C.Y - B.Y);
                }
            #endregion
        #endregion

    }

}
