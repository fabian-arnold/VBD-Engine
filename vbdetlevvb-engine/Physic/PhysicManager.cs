using System;
using System.Collections.Generic;
using System.Text;


using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using OpenTK;

namespace vbdetlevvb_engine.Physic
{
    public class PhysicManager
    {
        public static AABB worldAABB = new AABB()
        {
            LowerBound = new Vec2(-200.0f, -100.0f),
            UpperBound = new Vec2(200.0f, 200.0f)
        };
        public static World world = new World(worldAABB, new Vec2(0, -9.81f), true);

        public PhysicManager()
        {


            // Define the ground body.
            BodyDef groundBodyDef = new BodyDef();
            groundBodyDef.Position.Set(0.0f, -10.0f);

            // Call the body factory which  creates the ground box shape.
            // The body is also added to the world.
            Body groundBody = world.CreateBody(groundBodyDef);

            // Define the ground box shape.
            PolygonDef groundShapeDef = new PolygonDef();

            // The extents are the half-widths of the box.
            groundShapeDef.SetAsBox(50.0f, 10.0f);

            // Add the ground shape to the ground body.
            groundBody.CreateShape(groundShapeDef);

            

            // Define the dynamic body. We set its position and call the body factory.
            BodyDef bodyDef = new BodyDef();
            bodyDef.Position.Set(0.0f, 4.0f);
            Body body = world.CreateBody(bodyDef);

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1.0f, 1.0f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1.0f;

            // Override the default friction.
            shapeDef.Friction = 0.3f;

            // Add the shape to the body.
            body.CreateShape(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();

            // Prepare for simulation. Typically we use a time step of 1/60 of a
            // second (60Hz) and 10 iterations. This provides a high quality simulation
            // in most game scenarios.
            
            world.SetDebugDraw(new OpenGLDebugDraw());
           


        }


        public void UpdatePhysic(float delta)
        {
         
            int velocityIterations = 8;
            int positionIterations = 1;

            world.Step(delta, velocityIterations, positionIterations);

           

            
        }
    }
}
