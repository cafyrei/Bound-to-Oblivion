using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Platform
{
    private Dictionary<Vector2, int> tileMap;
    public List<Rectangle> textureStore;

    int TILESIZE = 32;
    int TILESIZE_ENVIRONMENT = 64;
    int Tiles = 32;
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
            new Rectangle(96,96,32,32), //16 CORNER LOWER RIGHT












            new Rectangle(128,0,32,32), //17 STAGE 2 TOP LEFT
            new Rectangle(160,0,32,32), //18 STAGE 2 TOP MIDDLE
            new Rectangle(192,0,32,32), //19 STAGE 2 TOP RIGHT
            new Rectangle(128,32,32,32), //20 STAGE 2 MID LEFT
            new Rectangle(160,32,32,32), //21 STAGE 2 MID MIDDLE
            new Rectangle(192,32,32,32), //22 STAGE 2 MID RIGHT
            new Rectangle(128,64,32,32), //23 STAGE 2 BOTTOM LEFT
            new Rectangle(160,64,32,32), //24 STAGE 2 BOTTOM MIDDLE
            new Rectangle(192,64,32,32), //25 STAGE 2 BOTTOM RIGHT
            new Rectangle(128,96,32,32), //26 STAGE 2 REVERSE LEFT
            new Rectangle(160,96,32,32), //27 STAGE 2 REVERSE MIDDLE
            new Rectangle(192,96,32,32), //28 STAGE 2 REVERSE RIGHT
            new Rectangle(224,0,32,32), //29 STAGE 2 CORNER TOP LEFT
            new Rectangle(224,32,32,32), //30 STAGE 2 CORNER TOP RIGHT
            new Rectangle(224,64,32,32), //31 STAGE 2 CORNER BOTTOM LEFT
            new Rectangle(224,96,32,32), //32 STAGE 2 CORNER BOTTOM RIGHT
            new Rectangle(0,128,32,32), //33 STONE LATERN 1
            new Rectangle(0,160,32,32), //34 GRAVEYARD WITH SWORD
            new Rectangle(32,128,32,32), //35 TOMBSTONE TYPE A
            new Rectangle(32,160,32,32), //36 TREE STUMP
            new Rectangle(64,128,32,32), //37 TOMBSTONE TYPE B
            new Rectangle(64,160,32,32), //38 SHRINE
            new Rectangle(96,128,32,32), //39 STONE LATERN 2
            new Rectangle(96,160,32,32), //40 BROKEN PILLAR
            new Rectangle(128,128,32,32), //41 WHITE BANNER FACING LEFT
            new Rectangle(128,160,32,32), //42 RED CRYSTAL
            new Rectangle(160,128,32,32), //43 RED BANNER FACING RIGHT
            new Rectangle(160,160,32,32), //44 SUMMONING CIRCLE TYPE A
            new Rectangle(192,128,32,32), //45 BLUE BANNER FACING LEFT
            new Rectangle(192,160,32,32), //46 SUMMONING CIRCLE TYPE B
            new Rectangle(224,128,32,32), //47 WHITE BANNER FACING RIGHT
            new Rectangle(224,160,32,32) //48 GARGOYLE STONE
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
                        if(value <= Tiles)
                            collision[new Vector2(x,y)] = new Rectangle(x * TILESIZE, y * TILESIZE, TILESIZE, TILESIZE);
                    }
                }
            }
            y++;
        }
        return (result, collision);
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