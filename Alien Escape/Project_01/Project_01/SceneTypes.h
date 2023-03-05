#pragma once

enum class Scenes
{
	MAIN_GAME,
	MENU,
	LEVEL_SELECT,
	GAME_OVER,
	LEVEL_EDITOR,
	IN_GAME_MENU,
	WIN_SCREEN,
	LOSE_SCREEN,
	TUTORIAL
};

class SceneTypes
{
public:
	static Scenes currentScene;
	static Scenes previousScene;
	static void updateScene(Scenes newScene);
};