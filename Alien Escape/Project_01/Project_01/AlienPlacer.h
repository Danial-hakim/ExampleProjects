#pragma once
#include "Item.h"
class AlienPlacer : public Item
{
public:
	AlienPlacer();
	AlienPlacer(sf::Vector2f t_newPos);
	void useItem();
	void update();
	void changeSize(sf::Vector2f t_size) override;

private:
	const int WIDTH{ 64 };
	const int HEIGHT{ 64 };
	const float SPRITESHEET_ROW{ 2.0f };
	const float SPRITESHEET_COL{ 12.0f };
};


