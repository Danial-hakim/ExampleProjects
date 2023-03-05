#pragma once
#include "SFML/Graphics.hpp"

extern float cellHeight;
extern float cellWidth;

struct ScreenSize
{
public:
	static float s_width;

	static float s_height;

	const static int ROWS = 30;
	const static int COLUMNS = 25;


	static sf::Vector2f getActualPosition(int t_row, int t_col)
	{
		return sf::Vector2f((t_row * cellWidth) + cellWidth / 2.0f,
			(t_col * cellHeight) + cellHeight / 2.0f);
	}

	static sf::Vector2f getActualPosition(sf::Vector2f t_pos)
	{
		return sf::Vector2f((t_pos.x * cellWidth) + cellWidth / 2.0f,
			(t_pos.y * cellHeight) + cellHeight / 2.0f);
	}

	static sf::Vector2f getActualPosition(int t_id);

	static void updateWidthAndHeight(sf::Vector2u currentScreenSize)
	{	
		s_width = static_cast<float>(currentScreenSize.x);
		s_height = static_cast<float>(currentScreenSize.y);
	}
};

