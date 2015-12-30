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
    class Goku
    {
        #region Variables
        // Graphics and Display information

        Texture2D GokuSprite;
        Rectangle GokuRectangle;

        // Goku State Health -> If Goku Health <= 0. we lose the game
        int GokuHealth = 1000;

        // Goku Abilities
        bool canShoot = true;
        int cooldownTime = 0;

        // Sound effect
        SoundEffect shootSound;
        SoundEffect getHit;

        #endregion

        #region GokuObject
        public Goku(ContentManager contentManager, string spriteName, int x, int y, SoundEffect shootSound, SoundEffect hitSound)
        {
            LoadContent(contentManager, spriteName, x, y);
            this.shootSound = shootSound; // assign the shootSound to the object
            getHit = hitSound; // assign the hitSound to getHit
        }

        #endregion

        #region PublicProperties
        /// <summary>
        /// Gets the collision rectangle for Goku
        /// </summary>
        public Rectangle getRectangle
        {
            get { return GokuRectangle; }
        }

        /// <summary>
        /// gets and sets goku's health
        /// </summary>
        public int health
        {
            get { return GokuHealth; }
            set
            {
                if(value < 0)
                {
                    GokuHealth = 0;
                } else if(value > 1000)
                {
                    GokuHealth = 1000;
                }
                else
                {
                    GokuHealth = value;
                }
            }
        }
        #endregion

        #region GokuProperties

        // Working - Setting X Location of Goku position
        private int X
        {
            get { return GokuRectangle.Center.X; }
            set
            {
                GokuRectangle.X = value - GokuRectangle.Width / 2;
                
                /* clamp to keep Goku character always stays inside the main windows. */
                if(GokuRectangle.X < 0) { GokuRectangle.X = 0; }
                else if (GokuRectangle.X > GameVariables.SCREEN_WIDTH - GokuRectangle.Width) {
                         GokuRectangle.X = GameVariables.SCREEN_WIDTH - GokuRectangle.Width; }

            }
        }

        // Working - Setting Y Location of Goku position
        private int Y
        {
            get { return GokuRectangle.Center.Y; }
            set
            {
                GokuRectangle.Y = value - GokuRectangle.Height / 2;

                //Make sure that the Goku Character doesn't fly out of the Screen
                if(GokuRectangle.Y < 0) { GokuRectangle.Y = 0; }
                else if (GokuRectangle.Y > GameVariables.SCREEN_HEIGHT - GokuRectangle.Height)
                { GokuRectangle.Y = GameVariables.SCREEN_HEIGHT - GokuRectangle.Height; }
            }
        }
            
        #endregion

        #region Public Functions

        /// <summary>
        /// Updates the Goku's position to exact location of the Mouse point
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="mouse">the current state of the mouse</param>
        public void Update(GameTime gameTime, MouseState mouse)
        {
            // If health is still positive, the game should be running its functions still
            if(GokuHealth > 0)
            {
                // Keep the character at the mouse arrow (right at the center of the character)
                GokuRectangle.X = mouse.X - GokuRectangle.Width / 2;
                GokuRectangle.Y = mouse.Y - GokuRectangle.Height / 2;

                /* Making sure that the Goku character does not fly out of the screen */
                // X Location
                if ( GokuRectangle.X < 0)
                {
                    GokuRectangle.X = 0;
                } else if (GokuRectangle.X + GokuRectangle.Width > GameVariables.SCREEN_WIDTH)
                {
                    GokuRectangle.X = GameVariables.SCREEN_WIDTH - GokuRectangle.Width;
                }

                // Y Location
                if (GokuRectangle.Y < 0)
                {
                    GokuRectangle.Y = 0;
                }
                else if (GokuRectangle.Y + GokuRectangle.Height > GameVariables.SCREEN_HEIGHT)
                {
                    GokuRectangle.Y = GameVariables.SCREEN_HEIGHT - GokuRectangle.Width;
                }


                // Allow Goku to shoot anytime he is alive
                if(GokuHealth > 0 && mouse.LeftButton == ButtonState.Pressed && canShoot == true)
                {
                    Weapon gokuWeapon = new Weapon(WeaponType.GokuWeapon, Game1.GetWeapon(WeaponType.GokuWeapon),
                                                   GokuRectangle.Center.X, GokuRectangle.Center.Y - GameVariables.GOKU_WEAPON_DISPLAY_ABOVE,
                                                   -GameVariables.GOKU_WEAPON_SPEED); // Goku's weapon is shot from bottom to the top -> velocity goes negative direction
                    // Now add the weapon to the game
                    Game1.AddWeapon(gokuWeapon);

                    if(!Game1.IsLosing)
                    // Play sound
                    shootSound.Play();

                    // Add cooldown time for the weapon
                    canShoot = false;
                }

                if (!canShoot)
                {
                    cooldownTime += gameTime.ElapsedGameTime.Milliseconds;
                    if(cooldownTime >= GameVariables.GOKU_WEAPON_COOLDOWN_TIME || mouse.LeftButton == ButtonState.Released)
                    {
                        canShoot = true;
                        cooldownTime = 0;
                    }
                }
            } 
        }

        /// <summary>
        /// Draws Goku
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GokuSprite, GokuRectangle, Color.White);
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load Content for the main Character
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="spriteName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {
            // load content for Goku displayment
            GokuSprite = contentManager.Load<Texture2D>(spriteName);
            GokuRectangle = new Rectangle(x - GokuSprite.Width / 2, y - GokuRectangle.Height / 2, GokuSprite.Width / 3, GokuSprite.Height / 3);
        }
        #endregion
    }
}
