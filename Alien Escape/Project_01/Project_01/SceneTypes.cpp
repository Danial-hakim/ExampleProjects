#include "SceneTypes.h"

Scenes SceneTypes::currentScene = Scenes::MENU;
Scenes SceneTypes::previousScene = Scenes::MENU;

void SceneTypes::updateScene(Scenes newScene)
{
	previousScene = currentScene;
	currentScene = newScene;
}
