using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class HUD : Canvas
    {
        Font font;
        StringFormat leftAlignment;
        StringFormat rightAlignment;

        public HUD(int width, int height) : base(width, height, false)
        {
            font = new Font(FontFamily.GenericSansSerif, 30f);
            leftAlignment = new StringFormat();
            leftAlignment.Alignment = StringAlignment.Near;
            rightAlignment = new StringFormat();
            rightAlignment.Alignment = StringAlignment.Far;
        }

        public void Update()
        {
            graphics.Clear(Color.Transparent);
            graphics.DrawString("Score: " + GameManager.Singleton._playerScore, font, Brushes.White, 50, 50, leftAlignment);
            graphics.DrawString("Lifes: " + GameManager.Singleton._playerHealth, font, Brushes.White, width - 200, 50, rightAlignment);
        }
    }
}
