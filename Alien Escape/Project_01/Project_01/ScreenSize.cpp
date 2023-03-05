#include "ScreenSize.h"
#include "Grid.h"

float ScreenSize::s_width = 1440;
float ScreenSize::s_height = 1200;

float cellHeight = ScreenSize::s_height / ScreenSize::COLUMNS;
float cellWidth = ScreenSize::s_width / ScreenSize::ROWS;

sf::Vector2f ScreenSize::getActualPosition(int t_id)
{
	int row;
	int col;

	col = t_id / 25;
	row = t_id % 25;

	sf::Vector2f end = sf::Vector2f(col * cellWidth, row * cellHeight) + Grid::getCellOrigin();

	return end;
}
