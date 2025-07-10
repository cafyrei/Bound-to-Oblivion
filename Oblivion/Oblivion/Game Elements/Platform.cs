using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;

public class Platform
{
    private Dictionary<Vector2, int> tileMap;
    public List<Rectangle> textureStore;

    int TILESIZE = 32;

    public Dictionary<Vector2, Rectangle> collision;

    Texture2D Atlas;
    public Platform(string map, ContentManager content, GraphicsDevice graphicsDevice)
    {
        LoadContent(content, graphicsDevice);
        (tileMap, collision) = LoadMap(map);
        textureStore = new()
        {
            new Rectangle(64,48,16,16),
            new Rectangle(192,32,16,16)
        };
    }

    (Dictionary<Vector2, int>, Dictionary<Vector2, Rectangle>) LoadMap(string filepath)
    {
        Dictionary<Vector2, int> result = new();
        Dictionary<Vector2, Rectangle> collision = new();
        StreamReader reader = new(filepath);

        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++)
            {
                if (int.TryParse(items[x], out int value))
                {
                    if (value > 0)
                    {
                        result[new Vector2(x, y)] = value;
                        //if (value < NUM of platform tiles)
                        collision[new Vector2(x,y)] = new Rectangle(x * TILESIZE, y * TILESIZE, TILESIZE, TILESIZE);
                    }
                }
            }
            y++;
        }
        return (result, collision);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void LoadContent(ContentManager Content, GraphicsDevice graphics)
    {
        Atlas = Content.Load<Texture2D>("Platform/sprite sheet 16x16px seamless textures");
    }

    public void Draw(SpriteBatch _spritebatch)
    {
        foreach (var item in tileMap)
        {
            Rectangle dest = new Rectangle((int)item.Key.X * TILESIZE, (int)item.Key.Y * TILESIZE, TILESIZE, TILESIZE);
            Rectangle src = textureStore[item.Value - 1];
            _spritebatch.Draw(Atlas, dest, src, Color.White);
        }
    }
}