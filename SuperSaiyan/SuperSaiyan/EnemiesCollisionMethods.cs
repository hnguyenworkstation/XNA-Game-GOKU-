using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SuperSaiyan
{
    public static class EnemiesCollisionMethods
    {
        #region Public Collision Checking Methods

        /// <summary>
        /// Checking if the currentDrawRectangle is free for collision of not (checking the area around the currentDrawRectangle).
        /// </summary>
        /// <param name="currentDrawRectangle"></param>
        /// <param name="OtherRectangles"></param>
        /// <returns></returns>
        public static bool IsCollisionFree(Rectangle currentDrawRectangle, List<Rectangle> OtherRectangles)
        {
            foreach(Rectangle subRectangle in OtherRectangles)
            {
                currentDrawRectangle.Intersects(subRectangle);
                return false;
            }
            // if there is no Intersection -> return true == free Collision
            return true;
        }

        /// <summary>
        /// Check where and which direction should the enemies collide to
        /// </summary>
        /// <param name="timeStep"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="firstObjectVelocity"></param>
        /// <param name="secondObjectVelocity"></param>
        /// <param name="firstObjectRectangle"></param>
        /// <param name="secondObjectRectangle"></param>
        /// <returns></returns>
        public static EnemiesCollisionInfor CheckCollisions(int timeStep, int screenWidth, int screenHeight,
                                                            Vector2 firstObjectVelocity, Vector2 secondObjectVelocity,
                                                            Rectangle firstObjectRectangle, Rectangle secondObjectRectangle)
        {
            Rectangle initialFirstObject;
            Rectangle initialSecondObject;
            Rectangle finalfirstObject;
            Rectangle finalSecondObject;
            Rectangle collisionRectangle;

            //checking if they are overlap
            bool detectedCollision = firstObjectRectangle.Intersects(secondObjectRectangle);
            if (detectedCollision)
            {
                // getting the same size of the rectangle
                finalfirstObject.Width = firstObjectRectangle.Width;
                finalfirstObject.Height = firstObjectRectangle.Height;
                finalSecondObject.Width = secondObjectRectangle.Width;
                finalSecondObject.Height = secondObjectRectangle.Height;

                // Getting the information right at the moment they overlap
                float firstXSpeed = (firstObjectVelocity.X * timeStep);
                float firstYSpeed = (firstObjectVelocity.Y * timeStep);
                initialFirstObject.X = (int)(firstObjectRectangle.X - firstXSpeed);
                initialFirstObject.Y = (int)(firstObjectRectangle.Y - firstYSpeed);
                initialFirstObject.Width = firstObjectRectangle.Width;
                initialFirstObject.Height = firstObjectRectangle.Height;

                float secondXSpeed = (secondObjectRectangle.X * timeStep);
                float secondYSpeed = (secondObjectRectangle.Y * timeStep);
                initialSecondObject.X = (int)(secondObjectRectangle.X - secondXSpeed);
                initialSecondObject.Y = (int)(secondObjectRectangle.Y - secondYSpeed);
                initialSecondObject.Width = secondObjectRectangle.Width;
                initialSecondObject.Height = secondObjectRectangle.Height;

                // Constantly, the time step is 60 frames per second, time will increament by the power of 2
                int timeIncrement = timeStep / 2;
                int collisionDeltaT = timeStep; // rate of change of time
                int DeltaT = timeIncrement; // rate of change of time at a certain moment

                while(timeIncrement > 0)
                {
                    // Move the objects
                    firstXSpeed = firstObjectVelocity.X * DeltaT;
                    firstYSpeed = firstObjectVelocity.Y * DeltaT;
                    secondXSpeed = secondObjectVelocity.X * DeltaT;
                    secondYSpeed = secondObjectVelocity.Y * DeltaT;

                    // Updating current Location for the objects
                    finalfirstObject.X = (int)(initialFirstObject.X + firstXSpeed);
                    finalfirstObject.Y = (int)(initialFirstObject.Y + firstYSpeed);
                    finalSecondObject.X = (int)(initialSecondObject.X + secondXSpeed);
                    finalSecondObject.Y = (int)(initialSecondObject.Y + secondYSpeed);

                    // decreasing the time increament 
                    timeIncrement /= 2;

                    // check if they bound again or not
                    detectedCollision = finalfirstObject.Intersects(finalSecondObject);

                    if (detectedCollision)
                    {   // if the collision happens again
                        collisionDeltaT = DeltaT;
                        DeltaT -= timeIncrement;

                        Rectangle.Intersect(ref finalfirstObject, ref finalSecondObject, out collisionRectangle);
                    } else
                    {
                        DeltaT += timeIncrement;
                    }
                }

                // Start the Collision
                int collisionStartTime = collisionDeltaT;
                firstXSpeed = firstObjectVelocity.X * collisionStartTime;
                firstYSpeed = firstObjectVelocity.Y * collisionStartTime;
                secondXSpeed = secondObjectVelocity.X * collisionStartTime;
                secondYSpeed = secondObjectVelocity.Y * collisionStartTime;

                finalfirstObject.X = (int)(initialFirstObject.X + firstXSpeed);
                finalfirstObject.Y = (int)(initialFirstObject.Y + firstYSpeed);
                finalSecondObject.X = (int)(initialSecondObject.X + secondXSpeed);
                finalSecondObject.Y = (int)(initialSecondObject.Y + secondYSpeed);

                // Check the side of the collision
                Rectangle collisionIntersection = Rectangle.Intersect(finalfirstObject, finalSecondObject);
                EnemiesCollisionSide possibleSide = checkCollisionSide(finalSecondObject, collisionIntersection, 
                                                                       firstObjectVelocity, secondObjectVelocity);

                // Let the object move over the time Step
                int preCollisionTime = collisionStartTime - 1;
                int postCollisionTime = timeStep - preCollisionTime;

                EnemiesCollisionInfor enemiesBounce = getBounceObjects(firstObjectVelocity, initialFirstObject,
                                                                       secondObjectVelocity, initialSecondObject,
                                                                       preCollisionTime, postCollisionTime, possibleSide);
                enemiesBounce.firstOutBounds = IsOutOfBounds(firstObjectRectangle, screenWidth, screenHeight);
                enemiesBounce.SecondOutBounds = IsOutOfBounds(secondObjectRectangle, screenWidth, screenHeight);

                return enemiesBounce;

            } else
            {
                // no collision happens -> no enemiesCollisionInfor
                return null;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// check the sides possible
        /// </summary>
        /// <param name="checkRectangle"></param>
        /// <param name="collisionRectangle"></param>
        /// <param name="firstVelocity"></param>
        /// <param name="secondVelocity"></param>
        /// <returns></returns>
        private static EnemiesCollisionSide checkCollisionSide(Rectangle checkRectangle, Rectangle collisionRectangle,
                                                               Vector2 firstVelocity, Vector2 secondVelocity)
        {
            List<EnemiesCollisionSide> sides = getCollisionSides(checkRectangle, collisionRectangle);
            // if the collision happens only one side (top/bot, left/right), simply return the side
            if (sides.Count == 1)
            {
                return sides[0];
            } else if (sides.Count == 2)
            {
                // checking the correct side
                EnemiesCollisionSide TopBotCollisionSide;
                if (sides.Contains(EnemiesCollisionSide.Top))
                {
                    TopBotCollisionSide = EnemiesCollisionSide.Top;
                }
                else
                {
                    TopBotCollisionSide = EnemiesCollisionSide.Bottom;
                }

                EnemiesCollisionSide LeftRightCollisionside;
                if (sides.Contains(EnemiesCollisionSide.Left))
                {
                    LeftRightCollisionside = EnemiesCollisionSide.Left;
                }
                else
                {
                    LeftRightCollisionside = EnemiesCollisionSide.Right;
                }

                // Check the relative to the axis of two velocities
                float velocityRelativeX = firstVelocity.X - secondVelocity.X;
                float velocityRelativeY = firstVelocity.Y - secondVelocity.Y;

                // get the ratio to the axis
                float xRatio = collisionRectangle.Width / Math.Abs(velocityRelativeX);
                float yRatio = collisionRectangle.Height / Math.Abs(velocityRelativeY);

                if (yRatio < xRatio) // xRatio greater than yRatio, higher percent that the enemies bound left or right
                {
                    // check if it is bounding top or bottom if possile
                    if ((velocityRelativeY < 0 && TopBotCollisionSide == EnemiesCollisionSide.Top) ||
                        (velocityRelativeY > 0 && TopBotCollisionSide == EnemiesCollisionSide.Bottom))
                    {
                        return LeftRightCollisionside;
                    }
                    else
                    {
                        // top and bottom is possible, the Y velocity is greater than X velocity.
                        return TopBotCollisionSide;
                    }
                }
                else // xRatio less than yRatio, higher percent that the enemies bound top or bottom
                {
                    // check left or right if possible
                    if ((velocityRelativeX > 0 && LeftRightCollisionside == EnemiesCollisionSide.Right) ||
                        (velocityRelativeX < 0 && LeftRightCollisionside == EnemiesCollisionSide.Left))
                    {
                        // it's been collistion top or bottom
                        return TopBotCollisionSide;
                    }
                    else
                    {
                        // left or right possible, x velocity greater than y velocity
                        return LeftRightCollisionside;
                    }
                }
            }
            else // 3 collision sides could happen
            {
                if(sides.Contains(EnemiesCollisionSide.Top) && sides.Contains(EnemiesCollisionSide.Bottom))
                {
                    if (sides.Contains(EnemiesCollisionSide.Left))
                    {
                        return EnemiesCollisionSide.Left;
                    }
                    else
                    {
                        return EnemiesCollisionSide.Right;
                    }
                }
                else
                {
                    // must be colliding with both left and right
                    if (sides.Contains(EnemiesCollisionSide.Top))
                    {
                        return EnemiesCollisionSide.Top;
                    }
                    else
                    {
                        return EnemiesCollisionSide.Bottom;
                    }
                }
            }
        }

        /// <summary>
        /// Check possible side that the collision happens
        /// </summary>
        /// <param name="checkRectangle">The current checking Rectangle</param>
        /// <param name="CollisionRectangle">The collision rectangle that used to compare with others</param>
        /// <returns></returns>
        private static List<EnemiesCollisionSide> getCollisionSides(Rectangle checkRectangle, Rectangle CollisionRectangle)
        {
            List<EnemiesCollisionSide> sides = new List<EnemiesCollisionSide>();
            if(CollisionRectangle.Left == checkRectangle.Left)
            {
                sides.Add(EnemiesCollisionSide.Left);
            }
            if (CollisionRectangle.Right == checkRectangle.Right)
            {
                sides.Add(EnemiesCollisionSide.Right);
            }
            if (CollisionRectangle.Top == checkRectangle.Top)
            {
                sides.Add(EnemiesCollisionSide.Top);
            }
            if (CollisionRectangle.Bottom == checkRectangle.Bottom)
            {
                sides.Add(EnemiesCollisionSide.Bottom);
            }

            return sides;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstObjectVelocity"></param>
        /// <param name="firstObjectRectangle"></param>
        /// <param name="secondObjectVelocity"></param>
        /// <param name="secondObjectRectangle"></param>
        /// <param name="preCollisionTime"></param>
        /// <param name="postCollisionTime"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private static EnemiesCollisionInfor getBounceObjects(Vector2 firstObjectVelocity, Rectangle firstObjectRectangle,
                                                              Vector2 secondObjectVelocity, Rectangle secondObjectRectangle,
                                                              int preCollisionTime, int postCollisionTime, EnemiesCollisionSide side)
        {
            // make a temp variables for the initial speeds
            float firstSpeed = firstObjectVelocity.Length();
            float secondSpeed = secondObjectVelocity.Length();

            // get the properties in order to move the object forward
            Rectangle newFirstRectangle = moveObjects(firstObjectVelocity, firstObjectRectangle, preCollisionTime);
            Rectangle newSecondRectangle = moveObjects(secondObjectVelocity, secondObjectRectangle, preCollisionTime);

            // Change the velocity
            Vector2 newFirstVelocity;
            Vector2 newSecondVelocity;
            getNewVelocities(firstObjectVelocity, secondObjectVelocity, side, out newFirstVelocity, out newSecondVelocity);

            // move the object forward
            moveObjects(newFirstVelocity, newFirstRectangle, postCollisionTime);
            moveObjects(newSecondVelocity, newSecondRectangle, postCollisionTime);

            // If the objects are still colliding
            MoveCollidingObjects(newFirstVelocity, newFirstRectangle, newSecondVelocity, newSecondRectangle,
                                 out newFirstRectangle, out newSecondRectangle);
            return new EnemiesCollisionInfor(newFirstVelocity, newFirstRectangle, false, newSecondVelocity, newSecondRectangle, false);

        }

        /// <summary>
        /// Get the new Rectangle for the object
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="ObjectRectangle"></param>
        /// <param name="timeDuration"></param>
        /// <returns></returns>
        private static Rectangle moveObjects(Vector2 velocity, Rectangle ObjectRectangle, int timeDuration)
        {
            // copy the current rectangle
            Rectangle newRectangle = new Rectangle(ObjectRectangle.X, ObjectRectangle.Y, ObjectRectangle.Width, ObjectRectangle.Height);
            newRectangle.X = (int)(newRectangle.X + velocity.X * timeDuration);
            newRectangle.Y = (int)(newRectangle.Y + velocity.Y * timeDuration);

            return newRectangle;
        }

        /// <summary>
        /// Get the new velocity for each object after collision
        /// </summary>
        /// <param name="firstVelocity"></param>
        /// <param name="secondVelocity"></param>
        /// <param name="side"></param>
        /// <param name="newFirstVelocity"></param>
        /// <param name="newSecondVelocity"></param>
        private static void getNewVelocities(Vector2 firstVelocity, Vector2 secondVelocity, EnemiesCollisionSide side,
                                            out Vector2 newFirstVelocity, out Vector2 newSecondVelocity)
        {
            switch (side)
            {
                case EnemiesCollisionSide.Top:
                    if (firstVelocity.Y > 0 && secondVelocity.Y > 0)
                    {
                        // first object caught up to second object, change the first y velocity
                        newFirstVelocity = new Vector2(firstVelocity.X, -1 * firstVelocity.Y);
                        newSecondVelocity = secondVelocity;
                    }
                    else if (firstVelocity.Y < 0 && secondVelocity.Y < 0)
                    {
                        // second object caught up to first object, change the second y velocity
                        newFirstVelocity = firstVelocity;
                        newSecondVelocity = new Vector2(secondVelocity.X, -1 * secondVelocity.Y);
                    }
                    else
                    {
                        // normal collision, change both velocities of two objects
                        newFirstVelocity = new Vector2(firstVelocity.X, -1 * firstVelocity.Y);
                        newSecondVelocity = new Vector2(secondVelocity.X, -1 * secondVelocity.Y);
                    }
                    break;
                case EnemiesCollisionSide.Bottom:
                    if (firstVelocity.Y > 0 && secondVelocity.Y > 0)
                    {
                        // second object caught up to first object, change the second y velocity
                        newFirstVelocity = firstVelocity;
                        newSecondVelocity = new Vector2(secondVelocity.X, -1 * secondVelocity.Y);
                    }
                    else if (firstVelocity.Y < 0 && secondVelocity.Y < 0)
                    {
                        // first object caught up to second object, change the first y velocity
                        newFirstVelocity = new Vector2(firstVelocity.X, -1 * firstVelocity.Y);
                        newSecondVelocity = secondVelocity;
                    }
                    else
                    {
                        // normal collision, change both velocities of two objects
                        newFirstVelocity = new Vector2(firstVelocity.X, -1 * firstVelocity.Y);
                        newSecondVelocity = new Vector2(secondVelocity.X, -1 * secondVelocity.Y);
                    }

                    break;
                case EnemiesCollisionSide.Left:
                    if (firstVelocity.X > 0 && secondVelocity.X > 0)
                    {
                        // first object caught up to second object, change the first x velocity
                        newFirstVelocity = new Vector2(firstVelocity.X * -1, firstVelocity.Y);
                        newSecondVelocity = secondVelocity;
                    }
                    else if (firstVelocity.X < 0 && secondVelocity.X < 0)
                    {
                        // second object caught up to first object, change the second x velocity
                        newFirstVelocity = firstVelocity;
                        newSecondVelocity = new Vector2(secondVelocity.X * -1, secondVelocity.Y);
                    }
                    else
                    {
                        // normal collision, change both velocities of two objects
                        newFirstVelocity = new Vector2(firstVelocity.X * -1, firstVelocity.Y);
                        newSecondVelocity = new Vector2(secondVelocity.X * -1, secondVelocity.Y);
                    }
                    break;
                case EnemiesCollisionSide.Right:
                    if (firstVelocity.X > 0 && secondVelocity.X > 0)
                    {
                        // second object caught up to first object, change the second x velocity
                        newFirstVelocity = firstVelocity;
                        newSecondVelocity = new Vector2(secondVelocity.X * -1, secondVelocity.Y);
                    }
                    else if (firstVelocity.X < 0 && secondVelocity.X < 0)
                    {
                        // first object caught up to second object, change the first x velocity
                        newFirstVelocity = new Vector2(firstVelocity.X * -1, firstVelocity.Y);
                        newSecondVelocity = secondVelocity;
                    }
                    else
                    {
                        // normal collision, change both velocities of two objects
                        newFirstVelocity = new Vector2(firstVelocity.X * -1, firstVelocity.Y);
                        newSecondVelocity = new Vector2(secondVelocity.X * -1, secondVelocity.Y);
                    }
                    break;
                default:
                    newFirstVelocity = firstVelocity;
                    newSecondVelocity = secondVelocity;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstVelocity"></param>
        /// <param name="firstRectangle"></param>
        /// <param name="secondVelocity"></param>
        /// <param name="secondRectangle"></param>
        /// <param name="newFirstRectangle"></param>
        /// <param name="newSecondRectangle"></param>
        private static void MoveCollidingObjects(Vector2 firstVelocity, Rectangle firstRectangle,
                                                 Vector2 secondVelocity, Rectangle secondRectangle,
                                                 out Rectangle newFirstRectangle, out Rectangle newSecondRectangle)
        {
            // check the relative ratio of each object
            float firstObjectSpeedSquared = firstVelocity.LengthSquared();
            float secondObjectSpeedSquared = secondVelocity.LengthSquared();
            float firstRatio = firstObjectSpeedSquared / (firstObjectSpeedSquared + secondObjectSpeedSquared);
            float secondRatio = 1 - firstRatio;

            // intersection
            Rectangle intersection = Rectangle.Intersect(firstRectangle, secondRectangle);

            int distance = intersection.Width + intersection.Height;
            newFirstRectangle = MoveOneObjectForward(firstVelocity, firstRectangle, distance * firstRatio);
            newSecondRectangle = MoveOneObjectForward(secondVelocity, secondRectangle, distance * secondRatio);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        private static Vector2 GetSideNormalCollision(EnemiesCollisionSide side)
        {
            switch (side)
            {
                case EnemiesCollisionSide.Top: return new Vector2(0, -1);
                case EnemiesCollisionSide.Bottom: return Vector2.UnitY;
                case EnemiesCollisionSide.Left: return Vector2.UnitX;
                case EnemiesCollisionSide.Right: return new Vector2(-1, 0);
                default: return Vector2.Zero;
            }
        }

        /// <summary>
        /// Moves the given object along the given velocity for a distance
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="rectangle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private static Rectangle MoveOneObjectForward(Vector2 velocity, Rectangle rectangle, float distance)
        {
            return new Rectangle((int)(rectangle.X + distance * velocity.X),
                                 (int)(rectangle.Y + distance * velocity.Y),
                                 rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Check if it collide out of the window or not
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <returns></returns>
        private static bool IsOutOfBounds(Rectangle rectangle, int screenWidth, int screenHeight)
        {
            return rectangle.Left < 0 || rectangle.Right > screenWidth
                || rectangle.Top < 0 || rectangle.Bottom > screenHeight;
        }

        #endregion
    }
}
