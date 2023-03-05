#include "MainGame.h"

MainGame::MainGame(sf::RenderWindow& t_window) : itemsOnFloor(&alien, &grid), player(&itemsOnFloor, t_window, &grid),
alien(&player, &pathFinder), pathFinder(&grid)
{	
	t_window.setVerticalSyncEnabled(true);
	AlienHearing::storeAlienRef(&alien);
}

void MainGame::init(sf::Font& gameFont, LevelData* t_level, int level)
{
	if (!music.openFromFile("Assets/Sound/music.wav"))
	{
		std::cout << "error with music" << std::endl;
	}
	levelData = t_level;
	currentLevel = level;

	itemsOnFloor.init(t_level->m_objects->m_objectsPos, t_level->m_objects->m_objectTypes);
	grid.init(t_level->m_floors[0].walls);
	player.init(&gameFont, t_level->m_objects[0].m_playerPos);
	gridRenderer.init(&grid);

	pathFinder.writeToFile(level);
	alien.init(t_level->m_objects->m_alienPos);
}

void MainGame::update(double dt)
{
	player.Update(collisionHandler.player_wall(grid, player), dt);
	grid.update(dt);
	alien.update(dt);
}

void MainGame::render(sf::RenderWindow& t_window)
{
	player.updateCamera();

	if (!debugMode)
	{
		gridRenderer.draw(t_window);
	}

	grid.draw(t_window, debugMode);

	itemsOnFloor.render(t_window);


	alien.Draw(t_window, debugMode);
	player.Draw(t_window);
}

void MainGame::processGameEvents(sf::Event& event)
{
	player.checkDelayedInput(event);

	if (event.type == sf::Event::KeyReleased
		&& event.key.code == sf::Keyboard::P)
	{
		debugMode = !debugMode;
	}
}

void MainGame::restart()
{
	itemsOnFloor.init(levelData->m_objects->m_objectsPos, levelData->m_objects->m_objectTypes);
	grid.init(levelData->m_floors[0].walls);
	player.init(levelData->m_objects[0].m_playerPos);
	gridRenderer.init(&grid);
	pathFinder.writeToFile(currentLevel);
	alien.init(levelData->m_objects->m_alienPos);
}

void MainGame::refreshEntities()
{
}
