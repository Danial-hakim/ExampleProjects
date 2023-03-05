#include "CollisionHandler.h"
#include "tinyc2.h"

float CollisionHandler::distance(sf::Vector2f t_vector_1, sf::Vector2f t_vector_2)
{
	return std::sqrt((t_vector_1.x - t_vector_2.x) * (t_vector_1.x - t_vector_2.x) + (t_vector_1.y - t_vector_2.y) * (t_vector_1.y - t_vector_2.y));
}

bool CollisionHandler::player_wall(Grid& t_grid, Player& t_player)
{
	//get player from centre to edge radius 
	//get the top left of the cell	
	//get the top of the cell 
	//set player pos to be within range of maximum n minimum
	//if distance between the player n cell is more than the player radius to edge , do smtg 

	float playerRadius = t_player.getRadius();
	std::vector<Cell> cell = t_grid.getCell();

	for (Cell tempCell: cell)
	{
		sf::FloatRect cellRect = { tempCell.getPosition().x - ( cellWidth / 2 ), tempCell.getPosition().y - ( cellHeight / 2 ) ,
								   cellWidth,cellHeight };

		float closestX = clamp(t_player.getPos().x, cellRect.left, cellRect.left + cellRect.width);
		float closestY = clamp(t_player.getPos().y, cellRect.top, cellRect.top + cellRect.height);

		float rangeFromPlayerX = t_player.getPos().x - closestX;
		float rangeFromPlayerY = t_player.getPos().y - closestY;

		float distanceSquared = (rangeFromPlayerX * rangeFromPlayerX) + (rangeFromPlayerY * rangeFromPlayerY);

		if (tempCell.getPassable() == false && distanceSquared  < (playerRadius * playerRadius))
		{
			/*std::cout << "collision" << std::endl;
			std::cout << tempCell.id() << std::endl;*/
			return true;
		}
	}
		
	return false;
}

bool CollisionHandler::mouse_cell(sf::Vector2f t_rectanglePos, sf::Vector2f t_rectangleSize, sf::Vector2f t_mousePos)
{
	sf::RectangleShape temp;


	temp.setSize(sf::Vector2f(t_rectangleSize.x / 1.2, t_rectangleSize.y / 1.2));

	temp.setPosition(t_rectanglePos);
	return temp.getGlobalBounds().contains(t_mousePos);
}

bool CollisionHandler::mouse_cell(sf::Vector2f t_rectanglePos, sf::FloatRect t_bounds, sf::Vector2f t_mousePos)
{
	t_bounds.left = t_rectanglePos.x;
	t_bounds.top = t_rectanglePos.y;

	return t_bounds.contains(t_mousePos);
}

float CollisionHandler::clamp(float t_pos, float t_minPos, float t_maxPos)
{
	if (t_pos < t_minPos)
	{
		t_pos = t_minPos;
	}

	if (t_pos > t_maxPos)
	{
		t_pos = t_maxPos;
	}
	return t_pos;
}
