namespace DepthCheckForm
{
    partial class PictureBoxForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            label = new Label();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(11, 16);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(768, 683);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // label
            // 
            label.AutoSize = true;
            label.Location = new Point(11, 702);
            label.Name = "label";
            label.Size = new Size(39, 15);
            label.TabIndex = 1;
            label.Text = "label1";
            // 
            // button1
            // 
            button1.Location = new Point(938, 43);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnCapture_Click;
            // 
            // button2
            // 
            button2.Location = new Point(992, 35);
            button2.Name = "button2";
            button2.Size = new Size(8, 8);
            button2.TabIndex = 3;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // PictureBoxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1070, 716);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label);
            Controls.Add(pictureBox);
            Name = "PictureBoxForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private Label label;
        private Button button1;
        private Button button2;
    }
}
