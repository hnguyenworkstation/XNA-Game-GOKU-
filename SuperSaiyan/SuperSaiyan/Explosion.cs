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
    class Explosion
    {
        #region Variables
        // displayment variables
        Texture2D explosionSprite;
        Rectangle explosionRectangle;
        Rectangle explosionSourceRectangle;

        // sound of the explosion
        SoundEffect explosionSound;

        // properties
        int frameWidth;
        int frameHeight;

        // track the animation
        int currentFrame;
        int elapsedFrameTime = 0;

        // status
        bool happening = false;
        bool finished = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Create the object for an explosion
        /// </summary>
        /// <param name="explosionSprite"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Explosion(Texture2D explosionSprite, int x, int y)
        {
            currentFrame = 0; // start at the smallest explosion

            // Initialize the object
            Initialize(explosionSprite);
            makeItExplode(x,y);
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Check and update the animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (happening)
            {
                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if(elapsedFrameTime > GameVariables.EXPLOSION_HAPPENING_TIMES) // when the explosion frame finishes displaying
                {
                    elapsedFrameTime = 0;
                    // move to the next frame
                    if(currentFrame < GameVariables.EXPLOSION_FRAMES - 1)
                    {
                        currentFrame++;
                        getExplosionFrame(currentFrame);
                    }
                    // if the whole animation is finished
                    else
                    {
                        happening = false;
                        finished = true;
                    }
                }
            }
        }

        public SoundEffect Sound
        {
            get { return explosionSound; }
            set { explosionSound = value; }
        }

        /// <summary>
        /// performing the animation
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (happening)
            {
                spriteBatch.Draw(explosionSprite, explosionRectangle, explosionSourceRectangle, Color.White);
            }
        }

        /// <summary>
        /// Check the status of an explosion
        /// </summary>
        public bool Finish
        {
            get { return finished; }
        }

        /// <summary>
        /// get the Rectangler of the explosion
        /// </summary>
        public Rectangle getRectangle
        {
            get { return explosionRectangle; }
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// Initializing the initial explosion and animation
        /// </summary>
        /// <param name="explosionSprite"></param>
        private void Initialize(Texture2D explosionSprite)
        {
            // load the animation strip
            this.explosionSprite = explosionSprite;

            // get the frame size of an explosion
            frameWidth = explosionSprite.Width / GameVariables.EXPLOSION_FRAMES_PER_ROW;
            frameHeight = explosionSprite.Height / GameVariables.EXPLOSION_ROWS;

            // Initial displayment of the explosion
            explosionRectangle = new Rectangle(0,0, frameWidth, frameHeight); // first displayment at the location
            explosionSourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight); // make the frame bigger by overriding another rectangle
        }

        /// <summary>
        /// Make the normal explode
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void makeItExplode(int x, int y)
        {
            // Initialize the values
            happening = true;
            elapsedFrameTime = 0;
            currentFrame = 0;

            // Set the location
            explosionRectangle.X = x - explosionRectangle.Width / 2;
            explosionRectangle.Y = y - explosionRectangle.Height / 2;

            // get the frame at the certain time when the explosion happens
            getExplosionFrame(currentFrame);
        }

        /// <summary>
        /// choose the source frame from the image file which correspond to the current frame
        /// </summary>
        /// <param name="currentFrame"></param>
        private void getExplosionFrame(int currentFrame)
        {
            // get the source of the frame from the "png" image
            explosionSourceRectangle.X = (currentFrame % GameVariables.EXPLOSION_FRAMES_PER_ROW) * frameWidth;
            explosionSourceRectangle.Y = (currentFrame / GameVariables.EXPLOSION_FRAMES_PER_ROW) * frameHeight;
        }


        #endregion


    }
}
