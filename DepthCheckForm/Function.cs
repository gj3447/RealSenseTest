using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Intel.RealSense;


namespace DepthCheckForm
{
    public static class Function
    {
        public static string get_test_usb_type()
        {
             using (var ctx = new Context())
             {
                 DeviceList devices = ctx.QueryDevices();
                 if (devices.Count == 0)
                 {
                     return "❌ RealSense 카메라가 연결되지 않았습니다.";
                 }
             
                 foreach (var device in devices)
                 {
                     string name = device.Info[CameraInfo.Name];
                     string usbType = device.Info[CameraInfo.UsbTypeDescriptor];
             
                     return $"📷 장치: {name}"+$"🔌 USB 버전: {usbType}";
                 }
             }
            return null;
        }
        public static Bitmap get_depth_bmp()
        {
            using (var pipeline = new Pipeline())
            {
                var config = new Config();
                config.EnableStream(Intel.RealSense.Stream.Depth, 640, 480, Format.Z16, 30);
                pipeline.Start(config);

                while(true)
                {
                    using (var frames = pipeline.WaitForFrames())
                    {
                        using (var depthFrame = frames.DepthFrame)
                        {
                            if (depthFrame != null)
                            {
                                Bitmap depthBitmap = DepthFrameToBitmap(depthFrame);

                                return depthBitmap;
                            }
                        }
                    }
                }
            }
        }

        public static Bitmap get_color_bmp()
        {
            using (var pipeline = new Pipeline())
            {
                var config = new Config();
                config.EnableStream(Intel.RealSense.Stream.Color, 640, 480, Format.Bgr8, 30);
                pipeline.Start(config);

                while (true)
                {
                    using (var frames = pipeline.WaitForFrames())
                    {
                        using (var colorFrame = frames.ColorFrame)
                        {
                            if (colorFrame != null)
                            {
                                Bitmap depthBitmap = FrameToBitmap(colorFrame);

                                return depthBitmap;
                            }
                        }
                    }
                }
            }
        }

        static Bitmap FrameToBitmap(VideoFrame frame)
        {
            Bitmap bitmap = new Bitmap(frame.Width, frame.Height, PixelFormat.Format24bppRgb);
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, frame.Width, frame.Height),
                ImageLockMode.WriteOnly, bitmap.PixelFormat);

            frame.CopyTo(bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        // Depth 프레임을 Bitmap으로 변환 (16비트 데이터 -> 8비트 변환)
        static Bitmap DepthFrameToBitmap(DepthFrame frame)
        {
            int width = frame.Width;
            int height = frame.Height;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // 컬러맵 설정 (Grayscale)
            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < 256; i++) palette.Entries[i] = Color.FromArgb(i, i, i);
            bitmap.Palette = palette;

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte[] depthData = new byte[width * height];
            ushort[] depthFrameData = new ushort[width * height];

            frame.CopyTo(depthFrameData);

            // Depth 데이터를 8비트로 스케일링
            for (int i = 0; i < depthFrameData.Length; i++)
            {
                depthData[i] = (byte)(depthFrameData[i] >> 5); // 16비트 -> 8비트 변환
            }

            System.Runtime.InteropServices.Marshal.Copy(depthData, 0, bitmapData.Scan0, depthData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
