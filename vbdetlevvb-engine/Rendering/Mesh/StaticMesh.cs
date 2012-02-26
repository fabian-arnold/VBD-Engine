using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using vbdetlevvb_engine.Rendering.Mesh;
using vbdetlevvb_engine.Rendering.VBO;
using vbdetlevvb_engine.Rendering.Camera;


namespace vbdetlevvb_engine.Rendering.Mesh
{

    class StaticMesh
    {
        VertexPosition[] vertices;
        short[] elements;
        Vector2[][] triangles;
        Vbo vbo = new Vbo();

        public StaticMesh()
        {
        } 
        
        public virtual void Load( Vector2[][] triangles)
        {
            this.triangles = triangles;
            GenerateThread();
            vbo.BufferVertices<VertexPosition>(ref vertices);
            
            //vbo = VBOHelper.LoadStaticVBO( vertices, elements );
            
        }

        public virtual void GenerateThread()
        {
            System.DateTime renderbegin = System.DateTime.Now;
            vertices = new VertexPosition[ triangles.Length * 3 ];
            elements = new short[ triangles.Length * 3 ];
            for( int i = 0; i < ( triangles.Length * 3 ); )
            {
                short c = ( short ) ( i / 3 );
                vertices[i] = new VertexPosition(triangles[c][0]);
                i++;
                vertices[i] = new VertexPosition(triangles[c][1]);
                i++;
                vertices[i] = new VertexPosition(triangles[c][2]);
                i++;
               

            }

            for( short a = 0; a < triangles.Length * 3; a++ )
            {

                elements[ a ] = ( short ) a;

            }

           
            TimeSpan delta = System.DateTime.Now - renderbegin;
            Console.WriteLine( "Mesh generation time: " + delta.Milliseconds );
        }

        public virtual void Draw()
        {
            UpdateMesh();

            vbo.Draw();
        }

        public virtual void UpdateMesh()
        {
        }
       
       
    }
}
