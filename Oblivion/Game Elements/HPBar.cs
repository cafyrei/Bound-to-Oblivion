using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Text.RegularExpressions;
using System;

namespace Oblivion
{
    public class HPBar
    {
        Texture2D background;
        Texture2D midground;
        Texture2D avatar;
        Texture2D profile;
        Vector2 position;
        float _maxHealth = 100f;
        float currentValue;
        Rectangle part;

        //Properties
        public float MaxValue { get => _maxHealth; set => _maxHealth = value; }
        public float CurrentValue { get => currentValue; set => currentValue = value; }

        public HPBar(ContentManager Content)
        {
            // Texture Declaration
            background = Content.Load<Texture2D>("UIs/HP_Background");
            midground = Content.Load<Texture2D>("UIs/HP_MiddleGround");
            avatar = Content.Load<Texture2D>("UIs/HP_Avatar");
            profile = Content.Load<Texture2D>("UIs/HP_Profile");

            // Lifeline
            currentValue = _maxHealth;

            // Movement
            position = new Vector2(10, 10);
            part = new(0, 0, midground.Width, midground.Height);
        }

        public void Update(float _value)
        {
            currentValue = _value;
            part.Width = (int)(currentValue / _maxHealth * midground.Width);
            Console.WriteLine("Part Width : " + part.Width);
        }

        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(background, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(midground, position, part, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(avatar, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(profile, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
        }
    }
}