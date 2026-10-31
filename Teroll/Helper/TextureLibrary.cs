using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Teroll.Helper
{
    public static class TextureLibrary
    {
        public static Texture2D Platform { get; set; }

        static TextureLibrary() 
        { 
            Platform = Game1.Get.Content.Load<Texture2D>("Tiles\\slice03_03");
        }
    }
}
