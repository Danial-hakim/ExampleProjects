#pragma once
#include "ItemStorage.h"
#include "Camera.h"
#include "CollisionHandler.h"
#include "LevelLoader.h"


class LevelEditorItems : public ItemStorage
{
private:

	const static int ITEM_AMOUNT = 6;
	sf::Vector2f START_DIST;

	const int SELECTION_OFFSET = -30;

	sf::Vector2f selectionStartDistance;

	int distanceBetweenItems;

	int selectedItemSpot = 0;
	int previousItemSpot = 0;

	sf::Vector2f itemSize;
	Item* equippedItem;

	// Design
	sf::RectangleShape selection;
	sf::RectangleShape equippedItemHighlight;

	Camera* viewObserver;

	int selectedStorageSpot = 0;

	bool mouseOverItem(int index, sf::Vector2f t_mousePos);

	void checkIfObjectIsOnBoard(Item* t_item);

public:

	bool haveItemEquipped();

	void placeDownItem(sf::Vector2i t_pos);

	void init(ObjectLayoutData t_objects);

	LevelEditorItems(Camera* t_camera);

	void handleKeyInventoryInput(sf::Vector2f& t_mousePos, sf::Event& event);

	void render(sf::RenderWindow& t_window) override;

	std::vector<Item*> getAllItems();
};

