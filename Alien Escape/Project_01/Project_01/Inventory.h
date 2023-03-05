#pragma once

// Other
#include "ItemStorage.h"
#include "ScreenSize.h"
#include <vector>
#include <array>
#include <iterator>
#include "SoundManager.h"
#include "Camera.h"


class Inventory : public ItemStorage
{
private:

	// consts for the layout of the items when inventory open
	const static int ITEM_AMOUNT = 4;
	sf::Vector2f START_DIST;

	const int SELECTION_OFFSET = 10;

	sf::Vector2f selectionStartDistance;

	int distanceBetweenItems = 0;

	sf::Vector2f ìtemSize;

	// Design
	sf::RectangleShape background;
	sf::RectangleShape selection;
	sf::RectangleShape equippedItemHighlight;
	sf::Text m_itemCount[ITEM_AMOUNT];
	sf::Font* m_font = nullptr;

	ScannerObserver* mapItemsObserver;
	Camera* viewObserver;

	int selectedStorageSpot = 0;

	bool inventoryOpen;
	Item* equippedItem;

	bool notEnoughItems(std::array<int, 4> items, int t_col);

	void moveSelectionLeft();
	void moveSelectionRight();
	void pickUpItem(sf::Vector2f playerPos);

	void renderItems(sf::RenderWindow& t_window);
	std::array<int, 4> countItems();

	void updateInventoryLocation();

public:
	Inventory(ScannerObserver* t_observer, Camera* t_camera);
	void getFont(sf::Font* t_font);
	void initialise();

	void handleKeyInventoryInput(sf::Event& event, sf::Vector2f playerPos, Direction t_angle);
	bool inventoryIsOpen();
	void equipItem();
	void useItem(sf::Vector2f playerPos, Direction t_angle);

	void render(sf::RenderWindow& t_window) override;

	void virtualUpdate() override;
};

