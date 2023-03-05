#include"InGameMenu.h"

InGameMenu::InGameMenu(sf::RenderWindow& t_window) : Screen(t_window)
{
}

void InGameMenu::init()
{
	sf::String Texts[] = { "Game is pause", "Continue", "Main Menu" };

	initialise(2, Texts);
}

void InGameMenu::firstButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MAIN_GAME);
}

void InGameMenu::secondButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MENU);
}

void InGameMenu::thirdButtonPress(int& t_lvlChecker)
{
	/////////////////////////////////////
}
