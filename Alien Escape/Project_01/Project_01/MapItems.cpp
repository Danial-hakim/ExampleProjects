#include "MapItems.h"

std::vector<Item*> MapItems::getItems()
{
	return storedItems;
}

void MapItems::init(std::vector<sf::Vector2f> t_objectsPos, std::vector<ItemType> t_objectTypes)
{
	storedItems.clear();

	for (int i = 0; i < t_objectsPos.size(); i++)
	{
		switch (t_objectTypes[i])
		{
		case ItemType::SMOKE_BOMB:
			addItem(new SmokeBomb(ScreenSize::getActualPosition(t_objectsPos[i].x, t_objectsPos[i].y), gridRef));
			break;
		case ItemType::BOOK:
			addItem(new Book("", ScreenSize::getActualPosition(t_objectsPos[i].x, t_objectsPos[i].y)));
			break;
		case ItemType::NOISE_MAKER:
			addItem(new NoiseMaker(ScreenSize::getActualPosition(t_objectsPos[i].x, t_objectsPos[i].y)));
			break;
		case ItemType::SCANNER:
			addItem(new Scanner(ScreenSize::getActualPosition(t_objectsPos[i].x, t_objectsPos[i].y), this));
			break;

		default:
			break;
		}
	}
}

MapItems::MapItems(Alien* t_alien, Grid* t_grid)
	: alien_ref(t_alien), gridRef(t_grid)
{
	scannerIcon.setFillColor(sf::Color::Yellow);
	scannerIcon.setRadius((ScreenSize::s_height / ScreenSize::COLUMNS) / 2.0f);
	scannerIcon.setOrigin(scannerIcon.getRadius(), scannerIcon.getRadius());
}

void MapItems::drawItemsForScanner(sf::RenderWindow& t_window)
{
	sf::Vector2f temp;

	for (Item* indexItem : storedItems)
	{
		temp = indexItem->getBody().getPosition();
		scannerIcon.setPosition(temp);
		t_window.draw(scannerIcon);
	}

	scannerIcon.setPosition(alien_ref->getPos());
	t_window.draw(scannerIcon);
}

std::vector<int> MapItems::getItemIDs()
{
	std::vector<int> itemIDs;
	int id;

	for (Item* item : storedItems)
	{
		id = gridRef->getCellID(item->getMapPos());

		itemIDs.push_back(id);
	}

	return itemIDs;
}

Item* MapItems::pickUpItem(sf::Vector2f currentPos)
{
	std::vector<int> itemIDs = getItemIDs();
	int playerID = gridRef->getCellID(currentPos);

	for (Item* currentItem : storedItems)
	{
		if (gridRef->cellsNearEachother(playerID, gridRef->getCellID(currentItem->getMapPos())))
		{
			storedItems.erase(std::remove(storedItems.begin(),
				storedItems.end(), currentItem), storedItems.end());

			return currentItem;
		}
	}
	return nullptr;
}

Grid* MapItems::getGridRef()
{
	return gridRef;
}
