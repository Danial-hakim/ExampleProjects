#include "LevelSelectMenu.h"

LevelSelectMenu::LevelSelectMenu(sf::RenderWindow& t_window) : Screen(t_window)
{
}

void LevelSelectMenu::init()
{
	sf::String Texts[] = { "Alien Escape","Original Level", "Custom Level" , "Back" };

	initialise(3, Texts);
}

void LevelSelectMenu::firstButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MAIN_GAME);
	t_lvlChecker = 1;
}

void LevelSelectMenu::secondButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MAIN_GAME);
	t_lvlChecker = 2;
}

void LevelSelectMenu::thirdButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::MENU);
}
