#include "WinScreen.h"

WinScreen::WinScreen(sf::RenderWindow& t_window) : Screen(t_window)
{
}

void WinScreen::init()
{
	sf::String Texts[] = { "You have Won !","Back to Menu"};

	initialise(1, Texts);

	if (!bgTexture.loadFromFile("./Assets/Sprites/win.jfif"))
	{
		std::cout << "Error with win image" << std::endl;
	}
	bgSprite.setTexture(bgTexture);
	bgSprite.setScale(SPRITE_SCALEDOWN);
}

void WinScreen::firstButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MENU);
}

void WinScreen::secondButtonPress(int& t_lvlChecker)
{
	/////////////////////////////////////
}

void WinScreen::thirdButtonPress(int& t_lvlChecker)
{
	/////////////////////////////////////
}

