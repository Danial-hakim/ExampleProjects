#pragma once
// Items
#include "Scanner.h"
#include "NoiseMaker.h"
#include "Book.h"
#include "SmokeBomb.h"
#include "AlienPlacer.h"
#include "PlayerPlacer.h"

#include "Grid.h"


class ItemStorage
{
protected:
	std::vector<Item*> storedItems;
	std::vector<std::vector<Item*>::iterator> toBeDeleted;

	std::vector<Item*>::iterator findItemInStorage(Item* checkedItemAgainst, bool checkIfInInventory);
	void deleteItem(Item* itemToDelete);

public:

	ItemStorage();

	bool update();
	virtual void virtualUpdate() {};
	virtual void render(sf::RenderWindow& t_window);
	void addItem(Item* newItem);
	Item* getItem(ItemType type);
};