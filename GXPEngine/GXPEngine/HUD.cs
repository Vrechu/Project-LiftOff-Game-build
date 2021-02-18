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

        private AnimationSprite _healthIndicator;

        public HUD(int width, int height) : base(width, height, false)
        {
            

            AddChild(_healthIndicator = new AnimationSprite("heart_complete.png", 1,4,4));
            _healthIndicator.SetScaleXY(1.5f, 1.5f);
            _healthIndicator.SetXY(width - _healthIndicator.width- 30, 30);

            font = new Font(FontFamily.GenericSansSerif, 30f);
            leftAlignment = new StringFormat();
            leftAlignment.Alignment = StringAlignment.Near;
            rightAlignment = new StringFormat();
            rightAlignment.Alignment = StringAlignment.Far;
        }

        public void Update()
        {
            graphics.Clear(Color.Transparent);
            graphics.DrawString("Highscore: " + GameManager.Singleton._highScore, font, Brushes.White, 50, 100, leftAlignment);
            graphics.DrawString("Score: " + GameManager.Singleton._playerScore, font, Brushes.White, 50, 50, leftAlignment);
            UpdateHealthIndicator();
        }

        private void UpdateHealthIndicator()
        {
            _healthIndicator.SetFrame(3 - GameManager.Singleton._playerHealth);
        }
    }
}
