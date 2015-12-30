﻿using System;
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
    /// <summary>
    /// An enumeration for the collision side
    /// </summary>
    public enum EnemiesCollisionSide
    {
        None,
        Bottom,
        Top,
        Left,
        Right
    }
}