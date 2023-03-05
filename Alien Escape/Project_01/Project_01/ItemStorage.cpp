#include "ItemStorage.h"

std::vector<Item*>::iterator ItemStorage::findItemInStorage(Item* checkedItemAgainst, bool checkIfInInventory)
{
	bool inventoryCheck = true;

	if (checkIfInInventory)
	{
		inventoryCheck = checkedItemAgainst->inInventory();
	}

	auto iter = storedItems.begin();

	for (Item* item : storedItems)
	{

		if (item == checkedItemAgainst && inventoryCheck)
		{
			break;
		}

		iter++;
	}

	return iter;
}

void ItemStorage::deleteItem(Item* itemToDelete)
{
	auto iter = findItemInStorage(itemToDelete, false);

	if (itemToDelete != nullptr)
	{
		delete itemToDelete;
		storedItems.erase(iter);
		itemToDelete = nullptr;
	}
}

ItemStorage::ItemStorage()
{

}

bool ItemStorage::update()
{
	bool collectedAllItems = false;

	for (Item* indexItem : storedItems)
	{
		indexItem->update();

		if (indexItem->shouldBeDeleted())
		{
			deleteItem(indexItem);
		}
	}



	return collectedAllItems;
}

void ItemStorage::render(sf::RenderWindow& t_window)
{
	for (Item* indexItem : storedItems)
	{
		indexItem->renderOnGrid(t_window);
	}
}

void ItemStorage::addItem(Item* newItem)
{
	storedItems.push_back(newItem);
}

Item* ItemStorage::getItem(ItemType type)
{
	Item* pointer = nullptr;

	for (Item* indexItem : storedItems)
	{
		if (indexItem->getItemType() == type && indexItem->inInventory())
		{
			pointer = indexItem;
		}
	}

	return pointer;
}
