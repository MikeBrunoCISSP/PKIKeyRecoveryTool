using System;
using System.Drawing;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    public partial class PleaseWait : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        Bitmap AnimatedImage = new Bitmap(@"wait.gif");
        bool currentlyAnimating = false;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public PleaseWait()
        {
            InitializeComponent();
        }

        public void AnimateImage()
        {
            if (!currentlyAnimating)
            {
                ImageAnimator.Animate(AnimatedImage, new EventHandler(OnFrameChanged));
                currentlyAnimating = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AnimateImage();
            ImageAnimator.UpdateFrames();
            e.Graphics.DrawImage(AnimatedImage, new Point(190, 100));
        }

        private void OnFrameChanged(object o, EventArgs e)
        {
            Invalidate();
        }
    }
}
