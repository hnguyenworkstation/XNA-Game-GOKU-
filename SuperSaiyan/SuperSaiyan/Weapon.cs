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
    public class Weapon
    {
        #region Variables
        // Displayment variables
        Texture2D WeaponSprite;
        Rectangle WeaponRectangle;

        // Weapon properties
        WeaponType type;
        bool active = true;

        // Weapon Velocity
        float verticalVelocity; // it can only be moving up or down

        #endregion

        #region Weapon Object

        /// <summary>
        /// Making a constructor for the weapon
        /// </summary>
        /// <param name="type">Goku's weapon or Superman's weapon</param>
        /// <param name="WeaponSprite">Image of the Weapon</param>
        /// <param name="x"> x Location </param>
        /// <param name="y"> y Location </param>
        /// <param name="verticalVelocity"></param>
        public Weapon(WeaponType type, Texture2D WeaponSprite, int x, int y, float verticalVelocity)
        {
            this.type = type;
            this.WeaponSprite = WeaponSprite;
            this.verticalVelocity = verticalVelocity;
            WeaponRectangle = new Rectangle(x - WeaponSprite.Width / 6, y - WeaponSprite.Height / 6, WeaponSprite.Width / 5, WeaponSprite.Height / 5);
        }

        #endregion

        #region Weapon Properties

        /// <summary>
        /// Gets and Sets the status of the weapon
        /// </summary>
        public bool Action
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets and Sets type of weapon
        /// </summary>
        public WeaponType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets and sets whether or not the weapon is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets and Sets Rectangle of the weapon
        /// </summary>
        public Rectangle wRectangle
        {
            get { return WeaponRectangle; }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// update the position of the weapon every miliseconds
        /// telling and updating the status of the weapon
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // make the weapon move along the vertical line of the screen
            WeaponRectangle.Y += (int)(verticalVelocity * gameTime.ElapsedGameTime.Milliseconds);

            //timer concept
            

            // remove the weapon fires
            if(WeaponRectangle.Bottom < 0 || WeaponRectangle.Top > GameVariables.SCREEN_HEIGHT)
            {
                active = false;
            }
        
        }

        /// <summary>
        /// Draw the weapon at the certain time
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(WeaponSprite, WeaponRectangle, Color.White);
        }
        #endregion
    }
}
