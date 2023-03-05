#pragma once
#include "ThrowableItem.h"
#include "Grid.h"

class SmokeBomb : public ThrowableItem
{
private:

	Grid* gridRef;

public:

	SmokeBomb(Grid* t_gridRef);
	SmokeBomb(sf::Vector2f t_position, Grid* t_gridRef);

	void useItem();
	void update();

};

