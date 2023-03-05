#include"MainMenuh.h"

MainMenu::MainMenu(sf::RenderWindow& t_window) : Screen(t_window)
{
}

void MainMenu::init()
{
	sf::String Texts[] = { "Alien Escape", "Play", "Instructions", "Level Editor" , "Exit"};
	initialise(4, Texts);
}

void MainMenu::firstButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::LEVEL_SELECT);
}

void MainMenu::secondButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::TUTORIAL);
}

void MainMenu::thirdButtonPress(int& t_lvlChecker)
{
	SceneTypes::updateScene(Scenes::LEVEL_EDITOR);
	t_lvlChecker = 2;
}

void MainMenu::fourthButtonPress(int& t_lvlChecker)
{
	view.getWindowRef().close();
}
