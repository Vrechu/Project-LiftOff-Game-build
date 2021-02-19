using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Cutscene : Sprite
    {
        private MyGame _mygame; //Reference to MyGame
        private Sprite _nextButton; //The "Press space to continue" button

        public Sprite[] images = new Sprite[]
        {
            new Sprite("Comic 1.png"),
            new Sprite("Comic 2.png"),
            new Sprite("Comic 3.png")
        }; //The list of sprites

        private int currentImage = 0; //The current page of the cutscene

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="myGame">Reference to MyGame</param>
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

        /// <summary>
        /// Sets the image to show
        /// </summary>
        /// <param name="i">The image/page to display</param>
        private void SetImage(int i)
        {
            AddChild(images[i]);
            SetChildIndex(_nextButton, 9999);
        }

        /// <summary>
        /// Shows the next image/page
        /// </summary>
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
