using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSaiyan
{
    // Public Class For Superman
    public class Superman
    {
        #region Variables
        // the Status
        bool alive = true;

        // the displayment
        Texture2D SupermanSprite;
        Rectangle SupermanRectangle;

        // velocity
        Vector2 velocity = new Vector2(0, 0);

        // skill abilities
        int elapseShooting = 0;
        int weaponDelay;

        // Sound Effect
        SoundEffect shootSound;
        SoundEffect getHit;

        #endregion

        #region Object

        /// <summary>
        /// Create an Object for this class
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="spriteName"></param>
        /// <param name="x">X position in the screen</param>
        /// <param name="y">Y position in the screen</param>
        /// <param name="velocity">Velocity of the object</param>
        /// <param name="shootSound"></param>
        /// <param name="getHit"></param>
        public Superman(ContentManager contentManager, string spriteName, int x, int y, Vector2 velocity, SoundEffect shootSound, SoundEffect getHit)
        {
            LoadContent(contentManager, spriteName, x, y);
            this.velocity = velocity;
            this.shootSound = shootSound;
            this.getHit = getHit;
            weaponDelay = GetRandomDelayTime(); // get random delay time
        }

        #endregion

        #region Public Functions
        /// <summary>
        /// Get Rectangle of the superfat
        /// </summary>
        public Rectangle getRectangle
        {
            get { return this.SupermanRectangle; }
        }
        
        /// <summary>
        /// set the X location for a Superfat
        /// </summary>
        public int x
        {
            set { SupermanRectangle.X = value - SupermanRectangle.Width / 2; }
        }

        /// <summary>
        /// Get the value of Y location for the super fat
        /// </summary>
        public int y
        {
            set { SupermanRectangle.Y = value - SupermanRectangle.Height / 2; }
        }

        /// <summary>
        /// get the current Location of the object
        /// </summary>
        public Point currentLocation
        {
            get { return SupermanRectangle.Center; }
        }

        /// <summary>
        /// Gets and sets whether or not the superman is alive
        /// </summary>
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        /// <summary>
        /// Get and Set the Values for the Rectangle of Superfat
        /// </summary>
        public Rectangle sRectangle
        {
            get { return SupermanRectangle; }
            set { SupermanRectangle = value; }
        }

        /// <summary>
        /// Get and Set the values for the velocity of Superfat
        /// </summary>
        public Vector2 sVelocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Draw the Object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SupermanSprite, SupermanRectangle, Color.White);
        }

        // Update the superman object
        /// <summary>
        /// Update the properties of the object every miliseconds
        /// </summary>
        /// <param name="gametime"></param>
        public void Update(GameTime gametime)
        {
            // Move the super fat man
            SupermanRectangle.X += (int)(velocity.X * gametime.ElapsedGameTime.Milliseconds);
            SupermanRectangle.Y += (int)(velocity.Y * gametime.ElapsedGameTime.Milliseconds);

            // Check the direction and make it bound correctly
            BoundTopBottom();
            BoundLeftRight();

            // timer for actions
            elapseShooting += gametime.ElapsedGameTime.Milliseconds;
            if(elapseShooting > weaponDelay)
            {
                elapseShooting = 0;
                weaponDelay = GetRandomDelayTime();
                Weapon sWeapon = new Weapon(WeaponType.SupermanWeapon, Game1.GetWeapon(WeaponType.SupermanWeapon)
                                           ,SupermanRectangle.X, SupermanRectangle.Y + GameVariables.SUPERMAN_WEAPON_DISPLAY_BELOW, getRandomFireVelocity() );
                Game1.AddWeapon(sWeapon); // add weapon to the game 
                if (!Game1.IsLosing)
                    shootSound.Play();
            }
        }

        #endregion

        #region Private Functions
        // Load the content for a certain Superman Object
        /// <summary>
        /// Get contents for an enimies object
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="spriteName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {
            // load content for Superman displayment
            SupermanSprite = contentManager.Load<Texture2D>(spriteName);
            SupermanRectangle = new Rectangle(x - SupermanSprite.Width / 2, y - SupermanSprite.Height / 2, SupermanSprite.Width / 3, SupermanSprite.Height / 3);
        }

        // Get random Delay shooting time
        /// <summary>
        /// Make this superman delay for a certain period of times (such as any values in the domain (500-1500) miliseconds)
        /// </summary>
        private int GetRandomDelayTime()
        {
            return GameVariables.SUPERMAN_MIN_SHOOTING_DELAY + RandomGenerator.Next(GameVariables.SUPERMAN_RANGE_SHOOTING_DELAY);
        }

        /// <summary>
        /// Make the Superman automatically changes the directions when he hits the screen bot/top wall
        /// </summary>
        private void BoundTopBottom()
        {
            if(SupermanRectangle.Y < 0)
            {   
                // bound off top -> direction to the bottom wall
                SupermanRectangle.Y = 0;
                velocity.Y *= -1; // making it moving to the opposite direction
            } else if (SupermanRectangle.Y + SupermanRectangle.Height > GameVariables.SCREEN_HEIGHT)
            {
                // bound off bottom -> direction to the top wall
                SupermanRectangle.Y = GameVariables.SCREEN_HEIGHT - SupermanRectangle.Height; 
                velocity.Y *= -1; // make it moving to the opposite direction
            }
        }

        /// <summary>
        /// Make the Superman automatically changes the directions when he hits the screen left/right wall
        /// </summary>
        private void BoundLeftRight()
        {
            if(SupermanRectangle.X < 0)
            {
                // bound off left -> direction to the right wall
                SupermanRectangle.X = 0;
                velocity.X *= -1; // making it moving to the right
            } else if(SupermanRectangle.X + SupermanRectangle.Width > GameVariables.SCREEN_WIDTH)
            {
                // bound off right -> direction to the left wall
                SupermanRectangle.X = GameVariables.SCREEN_WIDTH - SupermanRectangle.Width;
                velocity.X *= -1; // making it moving to the left
            }
        }

        /// <summary>
        /// get the velocity for the fire weapon of the superman
        /// </summary>
        /// <returns></returns>
        private float getRandomFireVelocity()
        {
            if(velocity.Y > 0)
            {
                return velocity.Y + GameVariables.SUPERMAN_WEAPON_SPEED;
            } else
            {
                return GameVariables.SUPERMAN_WEAPON_SPEED;
            }
        }

        #endregion
    }
}
