using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class ImgUtil 
{
    //根据路径获取图片
    public static Image getImage(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            return Image.FromStream(fs);
        }
    }
    //保存图片到指定路径
    public static void saveIamge(string filePath,Image image)
    {
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
        byte[] data = ImageToBytes(image);
        MemoryStream m = new MemoryStream(data);
        m.WriteTo(fs);
        Image result = Image.FromStream(fs);
        m.Close();
        fs.Close();
    }
    //图片转字节
    public static byte[] ImageToBytes(Image image)
    {
        ImageFormat format = image.RawFormat;
        using (MemoryStream ms = new MemoryStream())
        {
            if (format.Equals(ImageFormat.Jpeg))
            {
                image.Save(ms, ImageFormat.Jpeg);
            }
            else if (format.Equals(ImageFormat.Png))
            {
                image.Save(ms, ImageFormat.Png);
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                image.Save(ms, ImageFormat.Bmp);
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                image.Save(ms, ImageFormat.Gif);
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                image.Save(ms, ImageFormat.Icon);
            }
            byte[] buffer = new byte[ms.Length];
            //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
    //根据网络路径获取图片并保存
    public Image DownloadImage(string imageWebPath,string imageLocalPath)
    {
        using (System.Net.WebClient wc = new System.Net.WebClient())
        {
            if (Directory.Exists(imageLocalPath))
            {
                Directory.Delete(imageLocalPath);
            }
            wc.Headers.Add("User-Agent", "Chrome");
            FileStream fs=new FileStream(imageLocalPath, FileMode.OpenOrCreate);
            byte[] data = wc.DownloadData(imageWebPath);
            MemoryStream m = new MemoryStream(data);
            m.WriteTo(fs);
            Image result=Image.FromStream(fs);
            m.Close();
            fs.Close();
            return result;
        }
    }
    //将图片转为Base64字符串
    public string imgToString(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return Convert.ToBase64String(byData);
        }
    }
    //将Base64字符串转为图片
    public static Image stringToImage(string buffer)
     {
             MemoryStream ms = new MemoryStream(Convert.FromBase64String(buffer));
             Image image = Image.FromStream(ms);
             return image;
      }
    //生成缩略图并保存到指定路径
    public static void makeThumbnail(Image originalImage,List<Point> imageList,string filePath)
    {
        int index=filePath.LastIndexOf('.');
        string head = filePath.Substring(0, index);
        string tail = filePath.Substring(index, filePath.Length);
        foreach (Point point in imageList)
        {
            string savePath = head + "_" + point.X + "_" + point.Y + tail;
            Image thumbnail = makeThumbnail(originalImage, point.X, point.Y, thumbnailModel.HighWidth);
            saveIamge(savePath, thumbnail);
        }
    }
    //生成BMP的图
    public static Image getBmp(Image originalImage)
    {
        return makeThumbnail(originalImage, originalImage.Width, originalImage.Height, thumbnailModel.HighWidth);
    }
    //生成缩略图(bmp格式)
    public static Image makeThumbnail(Image originalImage, int width, int height, thumbnailModel mode)
    {
        int towidth = width;
        int toheight = height;

        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;

        switch (mode)
        {
            case thumbnailModel.HighWidth: //指定高宽缩放（可能变形）   
                break;
            case thumbnailModel.Width: //指定宽，高按比例   
                toheight = originalImage.Height * width / originalImage.Width;
                break;
            case thumbnailModel.Hight: //指定高，宽按比例  
                towidth = originalImage.Width * height / originalImage.Height;
                break;
            case thumbnailModel.Default: //指定高，宽按比例  
                if (ow <= towidth && oh <= toheight)
                {
                    x = -(towidth - ow) / 2;
                    y = -(toheight - oh) / 2;
                    ow = towidth;
                    oh = toheight;
                }
                else
                {
                    if (ow > oh)//宽大于高
                    {
                        x = 0;
                        y = -(ow - oh) / 2;
                        oh = ow;
                    }
                    else//高大于宽
                    {
                        y = 0;
                        x = -(oh - ow) / 2;
                        ow = oh;
                    }
                }
                break;
            case thumbnailModel.Auto:
                if (originalImage.Width / originalImage.Height >= width / height)
                {
                    if (originalImage.Width > width)
                    {
                        towidth = width;
                        toheight = (originalImage.Height * width) / originalImage.Width;
                    }
                    else
                    {
                        towidth = originalImage.Width;
                        toheight = originalImage.Height;
                    }
                }
                else
                {
                    if (originalImage.Height > height)
                    {
                        toheight = height;
                        towidth = (originalImage.Width * height) / originalImage.Height;
                    }
                    else
                    {
                        towidth = originalImage.Width;
                        toheight = originalImage.Height;
                    }
                }
                break;
            case thumbnailModel.Cut: //指定高宽裁减（不变形）   
                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * height / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }
                break;
            default:

                break;
        }

        //新建一个bmp图片  
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //新建一个画板  
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //设置高质量插值法  
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //设置高质量,低速度呈现平滑程度  
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //清空画布并以透明背景色填充  
        g.Clear(System.Drawing.Color.White);

        //在指定位置并且按指定大小绘制原图片的指定部分  
        g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                    new System.Drawing.Rectangle(x, y, ow, oh),
                    System.Drawing.GraphicsUnit.Pixel);

        return bitmap;
    }
    public enum thumbnailModel
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）   
        /// </summary>
        HighWidth,

        /// <summary>
        /// 指定宽，高按比例   
        /// </summary>
        Width,

        /// <summary>
        /// 默认  全图不变形   
        /// </summary>
        Default,

        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        Hight,

        /// <summary>
        /// 指定高宽裁减（不变形）？？指定裁剪区域
        /// </summary>
        Cut,

        /// <summary>
        /// 自动 原始图片按比例缩放
        /// </summary>
        Auto
    }
}
