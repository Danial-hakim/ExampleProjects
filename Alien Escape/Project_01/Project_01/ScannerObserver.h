#pragma once
#include "SFML/Graphics.hpp"

class ScannerObserver
{
public:
	virtual void drawItemsForScanner(sf::RenderWindow& t_window) = 0;
	virtual std::vector<int> getItemIDs() = 0;
	virtual Item* pickUpItem(sf::Vector2f currentPos) = 0;
	virtual std::vector<Item*> getItems() = 0;
};