using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GXPEngine;

class Menu : Sprite
{
    private MyGame _mygame;
    private Button _button;

    public Menu(float px, float py, MyGame myGame) : base("checkers.png")
    {
        x = px;
        y = py;
        SetOrigin(width / 2, height / 2);
        SetScaleXY(2, 2);
        AddChild(_button = new Button(0, 0));
        _mygame = myGame;
    }

    void Update()
    {
        ClickButton();
        PressKey();
    }
        
    //on button  click start the game
    private void ClickButton()
    {
        if (Input.GetMouseButtonDown(0)
            && _button.HitTestPoint(Input.mouseX, Input.mouseY)) // if mouse over button and clicked start level
        {
            _mygame._screenState = MyGame.ScreenState.CUTSCENE;
            _mygame.loadScreens();
        }
    }

    private void PressKey()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            _mygame._screenState = MyGame.ScreenState.CUTSCENE;
            _mygame.loadScreens();
        }        
    }
}

