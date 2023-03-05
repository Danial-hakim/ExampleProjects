#pragma once
#include "ItemStorage.h"
#include "ScannerObserver.h"
#include "Alien.h"

class MapItems : public ItemStorage, public ScannerObserver
{
private:
	sf::CircleShape scannerIcon;
	Alien* alien_ref;
	Grid* gridRef;
	std::vector<int> getItemIDs();

public:
	std::vector<Item*> getItems();
	void init(std::vector<sf::Vector2f> t_objectsPos, std::vector<ItemType> t_objectTypes);
	MapItems(Alien* t_alien, Grid *t_grid);
	void drawItemsForScanner(sf::RenderWindow& t_window);
	Item* pickUpItem(sf::Vector2f currentPos);
	Grid* getGridRef();
};

