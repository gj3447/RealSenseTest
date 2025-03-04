using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Intel.RealSense;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DepthCheckForm
{
    public partial class PictureBoxForm : Form
    {
        private Pipeline pipeline;

        private Bitmap Depth_bmp;
        private Bitmap RGB_bmp;

        private int frameWidth = 640;
        private int frameHeight = 480;
        public PictureBoxForm()
        {
            InitializeComponent();  // Form 디자이너에서 pictureBox와 btnCapture 버튼이 있다고 가정합니다.
            
        }


        // 버튼 클릭 시 Depth 이미지를 캡처하여 PictureBox에 표시하는 이벤트 핸들러
        public void btnCapture_Click(object sender, EventArgs e)
        {
            label.Text = Function.get_test_usb_type();
            Depth_bmp = Function.get_depth_bmp();
            pictureBox.Image?.Dispose();
            pictureBox.Image = Depth_bmp;
        }
    }
}
