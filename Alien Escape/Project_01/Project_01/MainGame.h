#pragma once
#include "SFML/Graphics.hpp"
#include "Player.h"
#include "Inventory.h"
#include "Grid.h"
#include "CollisionHandler.h"
#include "MapItems.h"
#include "AlienHearing.h"
#include "GridRenderer.h"
#include "AstartPathFinding.h"
#include "SFML/Audio.hpp"

class MainGame
{
public:

	MainGame(sf::RenderWindow& t_window);

	void init(sf::Font& gameFont, LevelData* t_level, int level);

	void update(double dt);
	void render(sf::RenderWindow& t_window);

	void processGameEvents(sf::Event& event);

	void restart();
	void refreshEntities();

private:

	bool debugMode;

	LevelData* levelData;
	Player player;
	Grid grid;
	Alien alien;
	MapItems itemsOnFloor;
	CollisionHandler collisionHandler;
	GridRenderer gridRenderer;
	Pathfinder pathFinder;


	int currentLevel;

	sf::Music music;
};

