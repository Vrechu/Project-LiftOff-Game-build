using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GXPEngine;

class Menu : Sprite
{
    private MyGame _myGame;
    private Sprite _highScoreBoard;
    private Sprite _nextButton;

    public Menu(float px, float py, MyGame myGame) : base("WallPaperFatt2.png")
    {
        x = px;
        y = py;
        _myGame = myGame;
        
        AddChild(_highScoreBoard = new Sprite("HighScoreBoard.png"));
        _highScoreBoard.SetOrigin(_highScoreBoard.width / 2, 0);
        _highScoreBoard.SetXY(width/2 - 600, -100);

        AddChild(_nextButton = new Sprite("Nextbutton.png"));
        _nextButton.SetOrigin(_nextButton.width / 2, _nextButton.height / 2);
        _nextButton.SetXY(width / 2 + 50, height / 2);
        AddChild(new HUD(game.width, game.height, _myGame));
    }

    void Update()
    {
        PressKey();
    }

    private void PressKey()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            _myGame._screenState = MyGame.ScreenState.CUTSCENE;
            _myGame.loadScreens();
        }        
    }
}

