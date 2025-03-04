using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Intel.RealSense;

namespace RealSenseTest
{
    public static class RealSenseFunctions
    {
        /// <summary>
        /// Depth 스트림에서 단일 프레임을 캡처하여 Bitmap으로 반환합니다.
        /// </summary>
        public static Bitmap GetDepthBitmap()
        {
            using (Pipeline pipeline = new Pipeline())
            {
                var config = new Config();
                config.EnableStream(Intel.RealSense.Stream.Depth, 640, 480, Format.Z16, 30);
                pipeline.Start(config);

                using (FrameSet frames = pipeline.WaitForFrames())
                using (DepthFrame depthFrame = frames.DepthFrame)
                {
                    if (depthFrame == null)
                        throw new Exception("Depth 프레임을 가져올 수 없습니다.");

                    // DepthFrame을 Bitmap(8bpp 그레이스케일)으로 변환
                    return ConvertDepthFrameToBitmap(depthFrame);
                }
            }
        }

        /// <summary>
        /// 캡처한 Depth 이미지를 지정된 파일명으로 PNG 파일로 저장합니다.
        /// </summary>
        public static void SaveDepthBitmap(string filename)
        {
            Bitmap bmp = GetDepthBitmap();
            bmp.Save(filename, ImageFormat.Png);
        }

        /// <summary>
        /// Color 스트림에서 단일 프레임을 캡처하여 Bitmap으로 반환합니다.
        /// </summary>
        public static Bitmap GetColorBitmap()
        {
            using (Pipeline pipeline = new Pipeline())
            {
                var config = new Config();
                config.EnableStream(Intel.RealSense.Stream.Color, 640, 480, Format.Rgb8, 30);
                pipeline.Start(config);

                using (FrameSet frames = pipeline.WaitForFrames())
                using (VideoFrame colorFrame = frames.ColorFrame)
                {
                    if (colorFrame == null)
                        throw new Exception("Color 프레임을 가져올 수 없습니다.");

                    // Color VideoFrame을 Bitmap(24bpp RGB)으로 변환
                    return ConvertColorFrameToBitmap(colorFrame);
                }
            }
        }

        /// <summary>
        /// 캡처한 Color 이미지를 지정된 파일명으로 PNG 파일로 저장합니다.
        /// </summary>
        public static void SaveColorBitmap(string filename)
        {
            Bitmap bmp = GetColorBitmap();
            bmp.Save(filename, ImageFormat.Png);
        }

        // ── 내부 헬퍼 함수 ──

        /// <summary>
        /// DepthFrame 데이터를 8비트 그레이스케일 Bitmap으로 변환합니다.
        /// (깊이 값은 최대 10,000mm 까지 정규화)
        /// </summary>
        private static Bitmap ConvertDepthFrameToBitmap(DepthFrame depthFrame)
        {
            int width = depthFrame.Width;
            int height = depthFrame.Height;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // 그레이스케일 팔레트 설정 (0~255)
            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp.Palette = palette;

            // Depth 데이터를 ushort 배열로 복사
            ushort[] depthData = new ushort[width * height];
            depthFrame.CopyTo(depthData);

            // 0~255 범위로 정규화한 byte 배열 생성
            byte[] pixelData = new byte[width * height];
            const int maxDepth = 10000; // 최대 깊이 (mm); 필요에 따라 조정
            for (int i = 0; i < depthData.Length; i++)
            {
                int depthVal = depthData[i];
                int normalized = Math.Min(depthVal, maxDepth) * 255 / maxDepth;
                pixelData[i] = (byte)normalized;
            }

            // Bitmap 데이터에 pixelData 복사
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// VideoFrame(Color) 데이터를 24비트 RGB Bitmap으로 변환합니다.
        /// </summary>
        private static Bitmap ConvertColorFrameToBitmap(VideoFrame colorFrame)
        {
            int width = colorFrame.Width;
            int height = colorFrame.Height;
            byte[] colorData = new byte[width * height * 3];
            colorFrame.CopyTo(colorData);

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(colorData, 0, bmpData.Scan0, colorData.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}