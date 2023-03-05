#pragma once
#include "Item.h"
class PlayerPlacer : public Item
{
public:
	PlayerPlacer();
	PlayerPlacer(sf::Vector2f t_newPos);
	void useItem();
	void update();
	void changeSize(sf::Vector2f t_size) override;

private:
	const int WIDTH{ 82 };
	const int HEIGHT{ 80 };
	const float SPRITESHEET_ROW{ 8.0f };
	const float SPRITESHEET_COL{ 18.0f };
};

