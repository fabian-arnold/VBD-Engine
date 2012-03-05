        using System;
using System.Collections.Generic;
using System.Xml;
using OpenTK.Graphics.OpenGL; //for triangulating paths with the GLU tesselator
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK.Graphics;
using OpenTK;
namespace vbdetlevvb_engine.Rendering.Mesh
{
    class ConcaveTriangulator
    {


        // Some of this code is lifted from the OpenTK tesselation sample:
        // Define the signatures for the callback functions, and declare the callbacks.
        delegate void BeginCallbackDelegate(int mode);
        delegate void EndCallbackDelegate();
        delegate void VertexCallbackDelegate(IntPtr v);
        delegate void EdgeFlagCallbackDelegate(int flag);
        delegate void ErrorCallbackDelegate(int code);
        unsafe delegate void CombineCallbackDelegate(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)]double[] coordinates,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]double*[] vertexData,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]float[] weight,
            double** dataOut);
 
        static OpenTK.GLControl glControl = null;
 
        // Triangulate
        // "Input" is a list of Vector2's that hold the point data to be triangulated.
        //(Vector2 is not shown but is a structure that has two floating points members X and Y).
        // "VertsPerContour" is an array that determines where to break up the contours in the
        //list of points in Input. VertsPerContour must sum to the number of entries in Input.
        //
        //Note: Outer contours must be given in counter-clockwise order while inner contours
        //      (holes) must be given in clock-wise order.
        //
        //Returns true on successfull triangulation, false on failure or error
        //
        unsafe static public bool Triangulate(Vector2[] Input, int[] VertsPerContour)
        {
            InvalidPolygon = false;
            Indices = new List<int>();
 
            IntPtr tess = IntPtr.Zero;
 
            // create tesselation object
            if (glControl == null)
            {
                glControl = new OpenTK.GLControl();
            }
 
            tess = Glu.NewTess();
 
            // register tesselation callbacks
            Glu.TessCallback(tess, TessCallback.TessBegin, new BeginCallbackDelegate(BeginCallback));
            Glu.TessCallback(tess, TessCallback.TessEdgeFlag, new EdgeFlagCallbackDelegate(EdgeCallback));
            Glu.TessCallback(tess, TessCallback.TessVertex, new VertexCallbackDelegate (VertexCallback));
            Glu.TessCallback(tess, TessCallback.TessEnd, new EndCallbackDelegate(EndCallback));
            Glu.TessCallback(tess, TessCallback.TessCombine, new CombineCallbackDelegate(CombineCallback));
            Glu.TessCallback(tess, TessCallback.TessError, new ErrorCallbackDelegate(ErrorCallback));
 
            //copy input into a linear array of floats
            double[] Vertices = new double[3 * Input.Length];
            for(int v=0;v<Input.Length;v++)
            {
                Vertices[v*3+0] = Input[v].X;
                Vertices[v*3+1] = Input[v].Y;
                Vertices[v*3+2] = 0.0;
            }
 
            // begin polygon
            Glu.TessBeginPolygon(tess, IntPtr.Zero);
 
            // go through the contours
            int CurrentContour = 0;
            int VertsThisContour = 0;
 
            for(int v=0;v<Input.Length;v++)
            {
                if(v == 0)
                    Glu.TessBeginContour(tess);
 
                // pass the corresponding vertex to the tesselator object
                double[] VertsToPass = new double[3];
                VertsToPass[0] = Vertices[v * 3 + 0];
                VertsToPass[1] = Vertices[v * 3 + 1];
                VertsToPass[2] = Vertices[v * 3 + 2];
                Glu.TessVertex(tess,VertsToPass,v);
 
                if(InvalidPolygon)
                    break;
 
                VertsThisContour++;
 
                if(VertsThisContour >= VertsPerContour[CurrentContour])
                {
                    VertsThisContour = 0;
                    Glu.TessEndContour(tess);
 
                    CurrentContour++;
 
                    if(CurrentContour < (long)VertsPerContour.Length)
                    {
                        Glu.TessBeginContour(tess);
                    }
                }
            }
 
            if(InvalidPolygon)
            {
                // destroy the tesselation object
                Glu.DeleteTess(tess);
                tess = IntPtr.Zero;
 
                return false; //error in polygon definition
            }
            else
            {
                // end polygon
                Glu.TessEndPolygon(tess);
 
                // destroy the tessellation object
                Glu.DeleteTess(tess);
                tess = IntPtr.Zero;
 
                //The Indices object is now valid.
                return true;
            }
        }
 
        //After a successful call to Triangulate, this public list holds the indices
        //for the given input.
        public static List<int> Indices;
        static bool InvalidPolygon;
 
        //GLU callback functions
        static void BeginCallback(int type)
        {
        }
 
        static void EdgeCallback(int flag)
        {
        }
 
        static void VertexCallback(IntPtr vertexIndex)
        {
            unsafe
            {
                Indices.Add(*((int*)vertexIndex));
            }
        }
 
        static void EndCallback()
        {
        }
 
        unsafe static void CombineCallback(double[] coords, double*[] vertexData, float[] weight, double** outData)
        {
            //This means the polygon is self-intersecting.
            //See the OpenTK tesselation example for a working version of the
            //combine callback.
            InvalidPolygon = true;
        }
 
        static void ErrorCallback(int errno)
        {
            //some error ocurred, mark this triangulation as invalid
            InvalidPolygon = true;
        }
 
    }
}
    