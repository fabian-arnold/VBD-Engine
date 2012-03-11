
using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;

using Gl = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;

namespace vbdetlevvb_engine.Physic
{
   


        using Box2DXMath = Box2DX.Common.Math;
        using SysMath = System.Math;

        // This class implements debug drawing callbacks that are invoked
        // inside World.Step.
        public class OpenGLDebugDraw : DebugDraw
        {
                public override void DrawPolygon(Vec2[] vertices, int vertexCount, Color color)
                {
                    Gl.Color3(color.R, color.G, color.B);
                        Gl.Begin(BeginMode.LineLoop);
                        for (int i = 0; i < vertexCount; ++i)
                        {
                                Gl.Vertex2(vertices[i].X, vertices[i].Y);
                        }
                        Gl.End();                     
                }

                public override void DrawSolidPolygon(Vec2[] vertices, int vertexCount, Color color)
                {
                    Gl.Enable(EnableCap.Blend);
                    Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                       
                        Gl.Color4(0.5f * color.R, 0.5f * color.G, 0.5f * color.B, 0.5f);
                        Gl.Begin(BeginMode.TriangleFan);
                        for (int i = 0; i < vertexCount; ++i)
                        {
                                Gl.Vertex2(vertices[i].X, vertices[i].Y);
                        }
                        Gl.End();
                        Gl.Disable(EnableCap.Blend);

                        Gl.Color4(color.R, color.G, color.B, 1.0f);
                        Gl.Begin(BeginMode.LineLoop);
                        for (int i = 0; i < vertexCount; ++i)
                        {
                                Gl.Vertex2(vertices[i].X, vertices[i].Y);
                        }
                        Gl.End();
                }

                public override void DrawCircle(Vec2 center, float radius, Color color)
                {
                        float k_segments = 16.0f;
                        float k_increment = 2.0f * Box2DX.Common.Settings.Pi / k_segments;
                        float theta = 0.0f;
                        Gl.Color3(color.R, color.G, color.B);
                        Gl.Begin(BeginMode.LineLoop);
                        for (int i = 0; i < k_segments; ++i)
                        {
                                Vec2 v = center + radius * new Vec2((float)SysMath.Cos(theta), (float)SysMath.Sin(theta));
                                Gl.Vertex2(v.X, v.Y);
                                theta += k_increment;
                        }
                        Gl.End();
                }

                public override void DrawSolidCircle(Vec2 center, float radius, Vec2 axis, Color color)
                {
                        float k_segments = 16.0f;
                        float k_increment = 2.0f * Box2DX.Common.Settings.Pi / k_segments;
                        float theta = 0.0f;
                        Gl.Enable(EnableCap.Blend);
                        Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                        Gl.Color4(0.5f * color.R, 0.5f * color.G, 0.5f * color.B, 0.5f);
                        Gl.Begin(BeginMode.TriangleFan);
                        for (int i = 0; i < k_segments; ++i)
                        {
                                Vec2 v = center + radius * new Vec2((float)SysMath.Cos(theta), (float)SysMath.Sin(theta));
                                Gl.Vertex2(v.X, v.Y);
                                theta += k_increment;
                        }
                        Gl.End();
                        Gl.Disable(EnableCap.Blend);
                   

                        theta = 0.0f;
                        Gl.Color4(color.R, color.G, color.B, 1.0f);
                        Gl.Begin(BeginMode.LineLoop);
                        for (int i = 0; i < k_segments; ++i)
                        {
                                Vec2 v = center + radius * new Vec2((float)SysMath.Cos(theta), (float)SysMath.Sin(theta));
                                Gl.Vertex2(v.X, v.Y);
                                theta += k_increment;
                        }
                        Gl.End();

                        Vec2 p = center + radius * axis;
                        Gl.Begin(BeginMode.Lines);
                        Gl.Vertex2(center.X, center.Y);
                        Gl.Vertex2(p.X, p.Y);
                        Gl.End();
                }

                public override void DrawSegment(Vec2 p1, Vec2 p2, Color color)
                {
                        Gl.Color3(color.R, color.G, color.B);
                        Gl.Begin(BeginMode.Lines);
                        Gl.Vertex2(p1.X, p1.Y);
                        Gl.Vertex2(p2.X, p2.Y);
                        Gl.End();
                }

                public override void DrawXForm(XForm xf)
                {
                        Vec2 p1 = xf.Position, p2;
                        float k_axisScale = 0.4f;
                        Gl.Begin(BeginMode.Lines);

                        Gl.Color3(1.0f, 0.0f, 0.0f);
                        Gl.Vertex2(p1.X, p1.Y);
                        p2 = p1 + k_axisScale * xf.R.Col1;
                        Gl.Vertex2(p2.X, p2.Y);

                        Gl.Color3(0.0f, 1.0f, 0.0f);
                        Gl.Vertex2(p1.X, p1.Y);
                        p2 = p1 + k_axisScale * xf.R.Col2;
                        Gl.Vertex2(p2.X, p2.Y);

                        Gl.End();
                }

                public static void DrawSegment(Vec2 p1, Vec2 p2, Color color, params object[] p)
                {
                    Gl.Color3(color.R, color.G, color.B);
                        Gl.Begin(BeginMode.Lines);
                        Gl.Vertex2(p1.X, p1.Y);
                        Gl.Vertex2(p2.X, p2.Y);
                        Gl.End();
                }

                public static void DrawPoint(Vec2 p, float size, Color color)
                {
                        Gl.PointSize(size);
                        Gl.Begin(BeginMode.Points);
                        Gl.Color3(color.R, color.G, color.B);
                        Gl.Vertex2(p.X, p.Y);
                        Gl.End();
                        Gl.PointSize(1.0f);
                }

               // static FTFont sysfont;

            //    static Tao.Platform.Windows.SimpleOpenGlControl openGlControl;
                private static bool sIsTextRendererInitialized = false;
               /* public static void InitTextRenderer(Tao.Platform.Windows.SimpleOpenGlControl openGlCtrl)
                {
                        openGlControl = openGlCtrl;

                        try
                        {
                                int Errors = 0;
                                // CREATE FONT
                                sysfont = new FTFont("FreeSans.ttf", out Errors);
                                // INITIALISE FONT AS A PER_CHARACTER TEXTURE MAPPED FONT
                                sysfont.ftRenderToTexture(12, 196);
                                // SET the sample font to align CENTERED
                                sysfont.FT_ALIGN = FTFontAlign.FT_ALIGN_LEFT;
                                sIsTextRendererInitialized = true;
                        }
                        catch (Exception)
                        {
                                sIsTextRendererInitialized = false;
                        }
                }*/

                public static void DrawString(int x, int y, string str)
                {
                        /*if (sIsTextRendererInitialized)
                        {
                                Gl.glMatrixMode(Gl.GL_PROJECTION);
                                Gl.glPushMatrix();
                                Gl.glLoadIdentity();

                                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                                Gl.glPushMatrix();
                                Gl.glLoadIdentity();

                                float xOffset = -0.95f + (float)x / ((float)openGlControl.Width / 2f);
                                float yOffset = 0.95f - (float)y / ((float)openGlControl.Height / 2f);
                                // Offset the font on the screen
                                Gl.glTranslatef(xOffset, yOffset, 0);

                                Gl.glColor3f(0.9f, 0.6f, 0.6f);
                                // Scale the font
                                Gl.glScalef(0.0035f, 0.0035f, 0.0035f);

                                // Begin writing the font
                                sysfont.ftBeginFont();
                                sysfont.ftWrite(str);
                                // Stop writing the font and restore old OpenGL parameters
                                sysfont.ftEndFont();

                                Gl.glPopMatrix();
                                Gl.glMatrixMode(Gl.GL_PROJECTION);
                                Gl.glPopMatrix();
                                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                        }*/
                }

                public static void DrawAABB(AABB aabb, Color c)
                {
                        Gl.Color3(c.R, c.G, c.B);
                        Gl.Begin(BeginMode.LineLoop);
                        Gl.Vertex2(aabb.LowerBound.X, aabb.LowerBound.Y);
                        Gl.Vertex2(aabb.UpperBound.X, aabb.LowerBound.Y);
                        Gl.Vertex2(aabb.UpperBound.X, aabb.UpperBound.Y);
                        Gl.Vertex2(aabb.LowerBound.X, aabb.UpperBound.Y);
                        Gl.End();
                }
        }
}
