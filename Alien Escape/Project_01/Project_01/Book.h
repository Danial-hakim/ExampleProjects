#pragma once
#include "Item.h"
#include "RememberedCodes.h"

class Book : public Item
{
private:
	std::string doorUnlockCode;

public:
	Book(std::string t_code);
	Book(std::string t_code, sf::Vector2f t_position);
	void useItem();
	void update();
};

