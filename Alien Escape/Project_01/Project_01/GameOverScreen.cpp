#include"GameOverScreen.h"

GameOverScreen::GameOverScreen(sf::RenderWindow& t_window) : Screen(t_window)
{
}

void GameOverScreen::init()
{
	sf::String Texts[] = { "You have lost..","Back to Menu" };

	initialise(1, Texts);

	if (!bgTexture.loadFromFile("./Assets/Sprites/lose.jfif"))
	{
		std::cout << "Error with lose image" << std::endl;
	}
	bgSprite.setTexture(bgTexture);
	bgSprite.setScale(SPRITE_SCALEDOWN);
}

void GameOverScreen::firstButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MENU);
}

void GameOverScreen::secondButtonPress(int& t_lvlChecker)
{
	/////////////////////////////////////
}

void GameOverScreen::thirdButtonPress(int& t_lvlChecker)
{
	/////////////////////////////////////
}
