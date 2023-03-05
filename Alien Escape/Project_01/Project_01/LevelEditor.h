#pragma once
#include "SFML/Graphics.hpp"
#include "GridRenderer.h"
#include "Camera.h"
#include "ScreenSize.h"
#include "CollisionHandler.h"
#include "LevelEditorItems.h"
#include "SceneTypes.h"

class LevelEditor
{
public:

	LevelEditor(sf::RenderWindow& t_window, LevelData& t_layout);

	void init(LevelData* t_level);

	void update(double dt);
	void render(sf::RenderWindow& t_window);

	void processGameEvents(sf::Event& event);


private:

	LevelData level;

	void saveChanges();


	int centreXPos = 0;

	Grid m_grid;
	//GridRenderer m_gridRenderer;
	LevelData& layout;
	Camera editorsView;
	LevelEditorItems items;

	sf::RectangleShape sideMenu;
	sf::Sprite menuButton;
	sf::Sprite backButton;
	sf::Sprite saveButton;

	static const int icons = 3;
	sf::Sprite menuIconSprites[icons];
	sf::Texture menuIconTextures[icons];

	sf::Texture buttonTexture;
};

