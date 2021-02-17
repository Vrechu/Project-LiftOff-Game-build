using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Cutscene : Sprite
    {
        private MyGame _mygame;

        public Sprite[] images = new Sprite[]
        {
            new Sprite("Comic 1.png"),
            new Sprite("Comic 2.png")
        };

        private int currentImage = 0;

        public Cutscene(MyGame myGame) : base("checkers.png")
        {
            SetImage(currentImage);
            _mygame = myGame;
        }

        public void Update()
        {
            if (Input.GetKeyDown(Key.SPACE))
            {
                NextImage();
            }
        }

        private void SetImage(int i)
        {
            AddChild(images[i]);
        }

        private void NextImage()
        {
            currentImage++;

            if (currentImage >= images.Length)
            {
                _mygame._screenState = MyGame.ScreenState.INGAME;
                _mygame.loadScreens();
            }
            else
            {
                SetImage(currentImage);
            }
        }
    }
}
