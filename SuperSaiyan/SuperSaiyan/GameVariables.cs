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
    public static class GameVariables
    {
        // Screen Resolution
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 600;

        // Maximum number of enemies
        public const int MAX_NUM_SUPERMEN = 10;

        // Aways
        public const int KILLED_SUPERMAN_AWARD = 100;

        // Constant Spawn Location
        public const int SPAWN_LOCATION_MIN = 100;

        // Goku Abilities
        public const int GOKU_WEAPON_COOLDOWN_TIME = 400;
        public const int GOKU_WEAPON_DAMAGE = 100;
        public const int GOKU_FLYING_SPEED = 10;

        //Superman Abilities
        public const float SUPERMAN_MIN_SPEED = 0.1f;
        public const float SUPERMAN_MAX_SPEED = 0.2f;

        public const int SUPERMAN_MIN_SHOOTING_DELAY = 500; // 500 miliseconds
        public const int SUPERMAN_RANGE_SHOOTING_DELAY = 1000; // 1000 miliseconds
        public const int SUPERMAN_DAMAGES_HIMSELF = 10;
        public const int SUPERMAN_SHOOTING_DAMAGES = 20;
        // Weapon Properties
        public const float GOKU_WEAPON_SPEED = 0.4f;
        public const int GOKU_WEAPON_DISPLAY_ABOVE = 20;

        public const int SUPERMAN_WEAPON_DISPLAY_BELOW = 20;
        public const float SUPERMAN_WEAPON_SPEED = 0.3f;

        // Explosion frames of animation
        public const int EXPLOSION_FRAMES = 9;
        public const int EXPLOSION_FRAMES_PER_ROW = 3;
        public const int EXPLOSION_ROWS = 3;
        public const int EXPLOSION_HAPPENING_TIMES = 10;

        // Displayment of String
        public const string SCORE = "Score: ";
        public static readonly Vector2 SCORE_DISPLAY_LOCATION = new Vector2(35, 35);
        public const string HEALTH = "Health: ";
        public static readonly Vector2 HEALTH_DISPLAY_LOCATION = new Vector2 (35,70);

        
    }
}
