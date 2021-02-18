using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class HUD : Canvas
    {
        Font ingameFont;
        Font menuFont;
        StringFormat leftAlignment;
        StringFormat rightAlignment;

        private AnimationSprite _healthIndicator;
        MyGame _myGame;

        public HUD(int width, int height, MyGame myGame) : base(width, height, false)
        {
            _myGame = myGame;

            AddChild(_healthIndicator = new AnimationSprite("heart_complete.png", 1, 4, 4));
            _healthIndicator.SetScaleXY(1.5f, 1.5f);
            _healthIndicator.SetXY(width - _healthIndicator.width - 30, 30);

            ingameFont = new Font(FontFamily.GenericSansSerif, 30f);
            leftAlignment = new StringFormat();
            leftAlignment.Alignment = StringAlignment.Near;
            rightAlignment = new StringFormat();
            rightAlignment.Alignment = StringAlignment.Far;

            menuFont = new Font(FontFamily.GenericSansSerif, 60f);            
        }

        public void Update()
        {
            graphics.Clear(Color.Transparent);
            switch (_myGame._screenState)
            {
                case MyGame.ScreenState.INGAME:
                    {
                        graphics.DrawString("" + GameManager.Singleton._highScore, ingameFont, Brushes.White, 360, 70, leftAlignment);
                        graphics.DrawString("" + GameManager.Singleton._playerScore, ingameFont, Brushes.White, 100, 70, leftAlignment);
                        UpdateHealthIndicator();
                        break;
                    }
                case MyGame.ScreenState.MENU:
                    {
                        graphics.DrawString("" + GameManager.Singleton._highScore, menuFont, Brushes.White, width/2-40, 130, leftAlignment);
                        _healthIndicator.alpha = 0;
                        break;
                    }
            }
            
            
        }

        private void UpdateHealthIndicator()
        {
            _healthIndicator.SetFrame(3 - GameManager.Singleton._playerHealth);
        }
    }
}
