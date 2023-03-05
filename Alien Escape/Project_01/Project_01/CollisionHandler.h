#pragma once

#include "Grid.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include "Player.h"
#include "Cell.h"

class CollisionHandler
{
public:

	static float distance(sf::Vector2f t_vector_1, sf::Vector2f t_vector_2);
	bool player_wall(Grid& t_grid, Player& t_player);
	static bool mouse_cell(sf::Vector2f t_rectanglePos, sf::Vector2f t_rectangleSize, sf::Vector2f t_mousePos);
	static bool mouse_cell(sf::Vector2f t_rectanglePos, sf::FloatRect t_bounds, sf::Vector2f t_mousePos);

private:

	float clamp(float t_pos, float t_minPos, float t_maxPos);
};


