using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amarlion
{
    internal class Map
    {
        private TmxMap _tmxMap;
        private Dictionary<int, Texture2D> _tilesets;
        public Map(string filename, ContentManager Content)
        {
            _tmxMap = new TmxMap(filename);
            _tilesets = new Dictionary<int, Texture2D>(_tmxMap.Tilesets.Count);
            foreach (var ts in _tmxMap.Tilesets)
            {
                Texture2D tex = Content.Load<Texture2D>(@"Raw\"+ts.Name);
                _tilesets.Add(ts.FirstGid, tex);
            }
        }

        public void Render(SpriteBatch sp, Rectangle viewRect)
        {
            var tileWidth = _tmxMap.TileWidth;
            foreach (var layer in _tmxMap.Layers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.X < (viewRect.X / _tmxMap.TileWidth) || tile.X > ((viewRect.X + viewRect.Width) / _tmxMap.TileWidth) ||
                        tile.Y < (viewRect.Y / _tmxMap.TileHeight) || tile.Y > ((viewRect.Y + viewRect.Height )/ _tmxMap.TileHeight))
                    {
                        
                        continue;
                    }
                    int gid = tile.Gid;

                    Texture2D tileset=_tilesets.First().Value;
                    int firstGid = 0;
                    foreach (var ts in _tilesets)
                    {
                        if (ts.Key <= tile.Gid && ts.Key>firstGid)
                        {
                            firstGid = ts.Key;
                            tileset = ts.Value;
                        }
                    }

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = (tileFrame % (tileset.Width / (_tmxMap.TileWidth+2)));
                        int row = tileFrame / (tileset.Width / (_tmxMap.TileWidth+2));

                        float x = (tile.X * _tmxMap.TileWidth)-viewRect.X;
                        float y = (tile.Y  * _tmxMap.TileHeight)-viewRect.Y;

                        Rectangle tilesetRec = new Rectangle((_tmxMap.TileWidth+2) * column, (_tmxMap.TileHeight+2) * row, _tmxMap.TileWidth, _tmxMap.TileHeight);

                        sp.Draw(tileset, new Rectangle((int)x, (int)y, _tmxMap.TileWidth, _tmxMap.TileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }

    }
}
