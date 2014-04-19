using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoApp.Storage
{
    public class ImageProcessor
    {
        private static int StaticTileWidth = 640;
        //private static int TileHeight = 480;
        public Image m_Image;
        private string m_FilePrfix;
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Levels { get; set; }
        public int TotalTiles { get; set; }
        public double AspectRatio { get; set; }
        public double MegaPixels { get; set; }

        public ImageProcessor(string imageFileUri, string filePrefix)
        {
            m_FilePrfix = filePrefix;

            Uri uri = new Uri(imageFileUri);
            WebRequest request = HttpWebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            using (stream)
            {
                m_Image = Image.FromStream(stream);
            }

            Width = m_Image.Width;
            Height = m_Image.Height;

            AspectRatio = (double)Width / (double)Height;

            MegaPixels = (double)(Width * Height) / 1000000;

            TileWidth = StaticTileWidth;
            TileHeight = (int)((double)TileWidth / AspectRatio);

            // Set the number of levels.
            int tiles = Width / TileWidth;
            Levels = (int)Math.Log(tiles, 2) + 1;

            // Calculate the number of tiles
            int tileCount = 0;
            for (int level = 0; level < Levels; level++)
            {
                int levelTiles = (int)Math.Pow(2, level);
                tileCount += levelTiles * levelTiles;
            }
            TotalTiles = tileCount;
        }

        public Image GetImageTile(int level, int x, int y)
        {
            int tiles = (int)Math.Pow(2, level);
            int sourceWidth = Width / tiles;
            int sourceHeight = Height / tiles;
            Rectangle rect = new Rectangle(x * sourceWidth, y * sourceHeight, sourceWidth, sourceHeight);

            Image tileImage = CreateTile(rect);

            return tileImage;
        }

        public Image CreateTile(Rectangle rectSource)
        {
            Rectangle rectDesc = new Rectangle(0, 0, TileWidth, TileHeight);
            Image tile = new Bitmap(TileWidth, TileHeight);
            Graphics g = Graphics.FromImage(tile);
            g.DrawImage(m_Image, rectDesc, rectSource, GraphicsUnit.Pixel);

            return tile;
        }

    }
}
