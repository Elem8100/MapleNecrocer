using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace MonoGame.SpriteEngine;
public static class TextureLoader
{

    public static Texture2D LoadTexture(String FilePath, GraphicsDevice device)
    {
        Texture2D Texture;
        FileStream titleStream = File.OpenRead(FilePath);
        Texture = Texture2D.FromStream(device, titleStream);
        titleStream.Close();
        Microsoft.Xna.Framework.Color[] Buffer = new Microsoft.Xna.Framework.Color[Texture.Width * Texture.Height];
        Texture.GetData(Buffer);
        for (int i = 0; i < Buffer.Length; i++)
            Buffer[i] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(Buffer[i].R, Buffer[i].G, Buffer[i].B, Buffer[i].A);
        Texture.SetData(Buffer);
        return Texture;
    }
    public static void LoadTextures(this Dictionary<string, Texture2D> ImageLib, string Dir, GraphicsDevice device)
    {
        DirectoryInfo Folder = new DirectoryInfo(Dir);
        foreach (FileInfo File in Folder.GetFiles())
        {
            if (File.Extension == ".png" || File.Extension == ".jpg")
            {
                  ImageLib.Add(File.Name, LoadTexture(File.FullName, device));
            
            }
        }
    }

}


