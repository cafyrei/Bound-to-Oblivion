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
            new Rectangle(0,128,32,32), // 33 TOP-LEFT LAMP
            new Rectangle(32,128,32,32), // 34 TOP-RIGHT LAMP
            new Rectangle(0,160,32,32), // 35 BOTTOM-LEFT LAMP
            new Rectangle(32,160,32,32), // 36 BOTTOM-RIGHT LAMP
            new Rectangle(64,128,32,32), // 37 TOP-LEFT TREE STUMP
            new Rectangle(96,128,32,32), // 38 TOP-RIGHT TREE STUMP
            new Rectangle(64,160,32,32), // 39 BOTTOM-LEFT TREE STUMP
            new Rectangle(96,160,32,32), // 40 BOTTOM-RIGHT TREE STUMP
            new Rectangle(192,256,32,32), // 41 BOTTOM-RIGHT WHITE BANNER
            new Rectangle(224,256,32,32), // 42 BOTTOM-RIGHT WHITE BANNER
            new Rectangle(192,288,32,32), // 43 BOTTOM-RIGHT WHITE BANNER
            new Rectangle(224,288,32,32), // 44 BOTTOM-RIGHT WHITE BANNER
            new Rectangle(0,256,32,32), // 45 TOP-LEFT TOMBSTONE
            new Rectangle(32,256,32,32), // 46 TOP-RIGHT TOMBSTONE
            new Rectangle(0,288,32,32), // 47 BOTTOM-LEFT TOMBSTONE
            new Rectangle(32,288,32,32), // 48 BOTTOM-RIGHT TOMBSTONE
            new Rectangle(128,128,32,32), // 49 TOP-LEFT SHRINE
            new Rectangle(160,128,32,32), // 50 TOP-RIGHT SHRINE
            new Rectangle(128,160,32,32), // 51 BOTTOM-LEFT SHRINE
            new Rectangle(160,160,32,32), // 52 BOTTOM-RIGHT SHRINE
            new Rectangle(192,192,32,32), // 53 TOP-LEFT GARGOYLE
            new Rectangle(224,192,32,32), // 54 TOP-RIGHT GARGOYLE
            new Rectangle(192,224,32,32), // 55 BOTTOM-LEFT GARGOYLE
            new Rectangle(225,224,32,32), // 56 BOTTOM-RIGHT GARGOYLE
            new Rectangle(0,320,32,32), // 57 TOP-LEFT RED FLAG
            new Rectangle(32,320,32,32), // 58 TOP-RIGHT RED FLAG
            new Rectangle(0,352,32,32), // 59 BOTTOM-LEFT RED FLAG
            new Rectangle(32,352,32,32), // 60 BOTTOM-RIGHT RED FLAG
            new Rectangle(192,128,32,32), // 61 TOP-LEFT PILLAR
            new Rectangle(224,128,32,32), // 62 TOP-RIGHT PILLAR
            new Rectangle(192,160,32,32), // 63 BOTTOM-LEFT PILLAR
            new Rectangle(224,160,32,32), // 64 BOTTOM-RIGHT PILLAR

            new Rectangle(256,160,32,32), // 65 STAIRS TOP MIDDLE
            new Rectangle(288,160,32,32), // 66 STAIRS TOP RIGHT
            new Rectangle(320,160,32,32), // 67 STAIRS MIDDLE LEFT
            new Rectangle(352,160,32,32), // 68 STAIRS MIDDLE MID
            new Rectangle(384,160,32,32), // 69 STAIRS MIDDLE RIGHT
            new Rectangle(416,160,32,32), // 70 STAIRS BOTTOM LEFT

            new Rectangle(256,128,32,32), // 71 STAIRS BOTTOM MIDDLE
            new Rectangle(288,128,32,32), // 72 STAIRS BOTTOM RIGHT
            new Rectangle(320,128,32,32), // 73 STAIRS MIDDLE LEFT
            new Rectangle(352,128,32,32), // 74 STAIRS MIDDLE MID
            new Rectangle(384,128,32,32), // 75 STAIRS MIDDLE RIGHT
            new Rectangle(416,128,32,32), // 76

            new Rectangle(256,96,32,32), // 77
            new Rectangle(288,96,32,32), // 78
            new Rectangle(320,96,32,32), // 79
            new Rectangle(352,96,32,32), // 80
            new Rectangle(384,96,32,32), // 81
            new Rectangle(416,96,32,32), // 82

            new Rectangle(256,64,32,32), // 83
            new Rectangle(288,64,32,32), // 84
            new Rectangle(320,64,32,32), // 85
            new Rectangle(352,64,32,32), // 86
            new Rectangle(384,64,32,32), // 87
            new Rectangle(416,64,32,32), // 88

            new Rectangle(320,32,32,32), // 89
            new Rectangle(352,32,32,32), // 90
            new Rectangle(384,32,32,32), // 91
            new Rectangle(416,32,32,32), // 92

            new Rectangle(320,0,32,32), // 93
            new Rectangle(352,0,32,32), // 94
            new Rectangle(384,0,32,32), // 95
            new Rectangle(416,0,32,32), // 96

            //GRASS
            new Rectangle(0,384,32,32), // 97
            new Rectangle(32,384,32,32), // 98
            new Rectangle(64,384,32,32) //99
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
    
    public float? GetPlatformYAtWorldX(float worldX)
{
    int tileX = (int)(worldX / TILESIZE);
    float? bestY = null;

    foreach (var kvp in collision.Keys)
    {
        if ((int)kvp.X == tileX)
        {
            float tileY = kvp.Y * TILESIZE;
            if (bestY == null || tileY < bestY)
                bestY = tileY;
        }
    }

    return bestY;
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