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
            new Rectangle(0,0,32,32), //1 GRASS Left Side Block
            new Rectangle(32,0,32,32), //2 GRASS Middle Side Block
            new Rectangle(64,0,32,32), //3 GRASS Right Side Block
            new Rectangle(0,32,32,32), //4 MID GRASS LEFT
            new Rectangle(32,32,32,32), //5 MID GRASS Middle
            new Rectangle(64,32,32,32), //6 MID GRASS RIGHT
            new Rectangle(0,64,32,32), //7 DARK LEFT
            new Rectangle(32,64,32,32), //8 DARK MIDDLE
            new Rectangle(64,64,32,32), //9 DARK RIGHT
            new Rectangle(0,96,32,32), //10 REVERSE GRASS LEFT
            new Rectangle(32,96,32,32), //11 REVERSE GRASS MIDDLE
            new Rectangle(64,96,32,32), //12 REVERSE GRASS RIGHT
            new Rectangle(96,0,32,32), //13 CORNER UPPER LEFT
            new Rectangle(96,32,32,32), //14 CORNER UPPER RIGHT
            new Rectangle(96,64,32,32), //15 CORNER LOWER LEFT
            new Rectangle(96,96,32,32) //16 CORNER LOWER RIGHT
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
                        if(value < 17)
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
        Atlas = Content.Load<Texture2D>("Platform/Platform Tiles");
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