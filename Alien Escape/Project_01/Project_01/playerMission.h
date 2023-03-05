#pragma once
#include "ItemStorage.h"
#include "SFML/Graphics.hpp"
#include "SceneTypes.h"



class playerMission
{
public:
	playerMission(ScannerObserver* t_mapItemsRef, Camera* t_camera, Grid* t_grid);
	void init(sf::Font* t_font);

	void update(sf::Vector2f t_playerPos);

	void render(sf::RenderWindow& t_window);
	void renderEscape(sf::RenderWindow& t_window);

	void restart();

private:

	const int GOAL_TEXT_Y_OFFSET = 50;
	bool gotAllBooks = false;

	sf::Text goalText;
	sf::Font* goalFont;

	sf::Texture escapeTexture;
	sf::Sprite escapeSprite;

	Grid* gridRef;
	ScannerObserver* mapItemRef;
	Camera* viewObserver;
};

