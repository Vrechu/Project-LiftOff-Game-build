using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Cutscene : Sprite
    {
        private MyGame _mygame;
        private Sprite _nextButton;

        public Sprite[] images = new Sprite[]
        {
            new Sprite("Comic 1.png"),
            new Sprite("Comic 2.png")
        };

        private int currentImage = 0;

        public Cutscene(MyGame myGame) : base("checkers.png")
        {
            _mygame = myGame;
            _nextButton = new Sprite("NextButton.png");
            _nextButton.scale = 0.25f;
            _nextButton.SetXY(game.width - _nextButton.width - 50, game.height - _nextButton.height - 50);
            AddChild(_nextButton);
            SetImage(currentImage);
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
            SetChildIndex(_nextButton, 9999);
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
