#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include <queue>
#include "Cell.h"
#include "Maths.h"
#include <cmath>
#include "Animation.h"
#include "ScreenSize.h"

class Grid
{
public:

	Grid();

	void init(std::vector<sf::Vector2i> t_wallsPos);
	void draw(sf::RenderWindow& window, bool t_debugMode);
	void neighbours(int t_row, int t_col);

	void refreshImpassableCells();
	std::vector<Cell> getCell();

	void update(float dt);
	void smokeBombReaction(sf::Vector2f t_smokeBombPosition);

	bool cellsNearEachother(int A, int B);

	static int getCellID(sf::Vector2f t_position);
	sf::Vector2f getCellRowAndCol(sf::Vector2f t_position);

	bool gridMouseCheck(sf::Vector2f t_mousePos, sf::RenderWindow& t_window);
	sf::Vector2i gridMouseCheck(sf::Vector2f t_mousePos, sf::RenderWindow& t_window, bool itemSelected);

	void calculateNeighboursDistance(int t_row , int t_col);

	void debugMode(sf::RenderWindow& t_window);

	static sf::Vector2f getCellOrigin();
	std::vector<std::pair<Cell, int>>* getSmokeCells();
	
	void addImpassableNeighbours(int t_row, int t_col);

	bool smokeChange = false;

private:
	sf::Vector2i mouseCellCollisionLoop(sf::Vector2f t_mousePos, sf::RenderWindow& t_window);

	bool smokeBombActive = false;
	const int TIME_LEFT_FOR_SMOKE_TO_PASS = 300;
	std::vector<std::pair<Cell,int>> smokeCells;
	std::vector<int> m_originalWalls;

	sf::RectangleShape grid[ScreenSize::ROWS][ScreenSize::COLUMNS];

	std::vector<Cell> m_cells;

	const int SURROUNDING_NEIGHBOURS{ 9 };

	sf::Font IDFont;
	sf::Text IDText;

	Animation fireAnimation;
	sf::Sprite fireSprite;
	sf::Texture fireTexture;

	const float ANIM_SPEED{ 200.0f };
	const float SPRITESHEET_ROW{ 10.0f };
	const float SPRITESHEET_COL{ 10.0f };
	const int WIDTH{ 66 };
	const int HEIGHT{ 66 };
};
