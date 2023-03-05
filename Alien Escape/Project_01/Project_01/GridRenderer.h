#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include <queue>
#include "Cell.h"
#include "ScreenSize.h"
#include "Grid.h"

class GridRenderer
{
public:

	void init(Grid* t_grid);
	void draw(sf::RenderWindow& t_window);
	void setUpSprite();
	void updateGrid(sf::Vector2f t_clickedPos);

private:

	int checkWallType(Cell& t_cell, std::vector<Cell> cell);
	int getRotation(int t_cellID, int t_neighbourID);
	bool isCornerWall(int t_cellID);
	int threeWayRotation(int t_cellID);
	void setupGrounds();

	std::vector<Cell> cell;


	sf::Sprite gridSprite[ScreenSize::ROWS * ScreenSize::COLUMNS];
	sf::Sprite* groundSprite;

	sf::Texture groundTexture;

	Grid* grid;

	sf::Texture standAloneWall;
	sf::Texture endWall;
	sf::Texture straightWall;
	sf::Texture threeWayWall;
	sf::Texture junctionWall;
	sf::Texture cornerWallTex;
	sf::Vector2f originOffset;
	sf::Vector2f groundOffset;

	int lastRotation = 0;
	int rotation = 0;

	bool test{ false };
};
