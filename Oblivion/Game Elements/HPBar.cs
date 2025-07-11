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
        Texture2D undermidground;
        Texture2D midground;
        Texture2D avatar;
        Texture2D profile;
        Vector2 position;
        float maxValue;
        float currentValue;
        Rectangle part;

        //Properties
        public float MaxValue { get => maxValue; set => maxValue = value; }
        public float CurrentValue { get => currentValue; set => currentValue = value; }

        public HPBar(ContentManager Content, float _maxHealth)
        {
            background = Content.Load<Texture2D>("UIs/HP_Background");
            undermidground = Content.Load<Texture2D>("UIs/HP_Undermidground");
            midground = Content.Load<Texture2D>("UIs/HP_MiddleGround");
            avatar = Content.Load<Texture2D>("UIs/HP_Avatar");
            profile = Content.Load<Texture2D>("UIs/HP_Profile");
            maxValue = _maxHealth;
            currentValue = _maxHealth;
            position = new Vector2(10, 10);
            part = new(0, 0, midground.Width, midground.Height);
        }

        public void Update(float value)
        {
            currentValue = value;
            part.Width = (int)(currentValue / maxValue * midground.Width);
        }

        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(background, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(undermidground, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(midground, position, part, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(avatar, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            _spritebatch.Draw(profile, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
        }
    }
}