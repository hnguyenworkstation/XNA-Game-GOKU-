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
    public class EnemiesCollisionInfor
    {
        #region Variables
        Vector2 firstVelocity;
        Rectangle firstRectangle;
        bool firstOutofBounds;

        Vector2 secondVelocity;
        Rectangle secondRectangle;
        bool secondOutofBounds;
        #endregion

        #region Constructor
        /// <summary>
        /// Making a collision object
        /// </summary>
        /// <param name="initialVelocity">velocity before bounding</param>
        /// <param name="initialRectangle"></param>
        /// <param name="firstBounds"></param>
        /// <param name="finalVelocity">velocity after bounding</param>
        /// <param name="finalRectangle"></param>
        /// <param name="finalBounds"></param>
        public EnemiesCollisionInfor(Vector2 firstVelocity,
                                     Rectangle firstRectangle,
                                     bool firstOutofBounds,

                                     Vector2 secondVelocity,
                                     Rectangle secondRectangle,
                                     bool secondOutofBounds)
        {
            this.firstRectangle = firstRectangle;
            this.firstVelocity = firstVelocity;
            this.firstOutofBounds = firstOutofBounds;
            this.secondOutofBounds = secondOutofBounds;
            this.secondRectangle = secondRectangle;
            this.secondVelocity = secondVelocity;
        }

        #endregion

        #region GetSet Values

        /// <summary>
        /// get and set value for initial velocity
        /// </summary>
        public Vector2 getFirstVelocity
        {
            get { return firstVelocity; }
            set { firstVelocity = value; }
        }

        /// <summary>
        /// get and set value for initial velocity
        /// </summary>
        public Vector2 getSecondVelocity
        {
            get { return secondVelocity; }
            set { secondVelocity = value; }
        }

        /// <summary>
        /// get and set the first draw rectangle
        /// </summary>
        public Rectangle getFirstRectangle
        {
            get { return firstRectangle; }
            set { firstRectangle = value; }
        }

        /// <summary>
        /// get and set the first draw rectangle
        /// </summary>
        public Rectangle getSecondRectangle
        {
            get { return secondRectangle; }
            set { secondRectangle = value; }
        }

        /// <summary>
        /// Gets and sets whether or not the first draw rectangle is out of bounds
        /// </summary>
        public bool firstOutBounds
        {
            get { return firstOutofBounds; }
            set { firstOutofBounds = value; }
        }

        /// <summary>
        /// Gets and sets whether or not the final draw rectangle is out of bounds
        /// </summary>
        public bool SecondOutBounds
        {
            get { return secondOutofBounds; }
            set { secondOutofBounds = value; }
        }

        #endregion
    }
}
