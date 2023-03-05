#include "LevelEditorItems.h"


bool LevelEditorItems::mouseOverItem(int index, sf::Vector2f t_mousePos)
{
	sf::Vector2f size = sf::Vector2f(storedItems[index]->getBody().getGlobalBounds().width, storedItems[index]->getBody().getGlobalBounds().height);
	sf::Vector2i position = viewObserver->getWindowRef().mapCoordsToPixel(storedItems[index]->getBody().getPosition() - size / 2.0f);


	return CollisionHandler::mouse_cell(sf::Vector2f(position),
		size, sf::Vector2f(t_mousePos));
}

void LevelEditorItems::checkIfObjectIsOnBoard(Item* t_item)
{
	int alreadyIn = 0;

	for (Item* item : storedItems)
	{
		if (item->getItemType() == t_item->getItemType())
		{
			alreadyIn++;

			if (alreadyIn >= 2)
			{
				item->setMapPos(t_item->getMapPos());
				delete t_item;
				break;
			}
		}
	}

	if (alreadyIn < 2)
	{
		addItem(t_item);
	}
}

bool LevelEditorItems::haveItemEquipped()
{
	return equippedItem != nullptr;
}

void LevelEditorItems::placeDownItem(sf::Vector2i t_pos)
{
	sf::Vector2f newPos = ScreenSize::getActualPosition(t_pos.x, t_pos.y);
	bool putNewItemDown = true;

	for (Item* t_item : storedItems)
	{
		if (t_item->getMapPos() == newPos)
		{
			if (equippedItem->getItemType() == t_item->getItemType())
			{
				putNewItemDown = false;
			}

			deleteItem(t_item);
			break;	
		}
	}

	if (putNewItemDown)
	{
		switch (equippedItem->getItemType())
		{
		case ItemType::SMOKE_BOMB:
			addItem(new SmokeBomb(newPos, nullptr));
			break;
		case ItemType::NOISE_MAKER:
			addItem(new NoiseMaker(newPos));
			break;
		case ItemType::SCANNER:
			addItem(new Scanner(newPos, nullptr));
			break;
		case ItemType::BOOK:
			addItem(new Book("", newPos));
			break;
		case ItemType::PLAYER_PLACER:
			checkIfObjectIsOnBoard(new PlayerPlacer(newPos));
			break;	
		case ItemType::ALIEN_PLACER:
			checkIfObjectIsOnBoard(new AlienPlacer(newPos));
			break;
		default:
			break;
		}
	}
}

void LevelEditorItems::init(ObjectLayoutData t_objects)
{
	checkIfObjectIsOnBoard(new AlienPlacer(ScreenSize::getActualPosition(t_objects.m_alienPos)));
	checkIfObjectIsOnBoard(new PlayerPlacer(ScreenSize::getActualPosition(t_objects.m_playerPos)));

	for (int i = 0; i < t_objects.m_objectTypes.size(); i++)
	{
		switch (t_objects.m_objectTypes[i])
		{
		case ItemType::SMOKE_BOMB:
			addItem(new SmokeBomb(ScreenSize::getActualPosition(t_objects.m_objectsPos[i]), nullptr));
			break;
		case ItemType::NOISE_MAKER:
			addItem(new NoiseMaker(ScreenSize::getActualPosition(t_objects.m_objectsPos[i])));
			break;
		case ItemType::SCANNER:
			addItem(new Scanner(ScreenSize::getActualPosition(t_objects.m_objectsPos[i]), nullptr));
			break;
		case ItemType::BOOK:
			addItem(new Book("", ScreenSize::getActualPosition(t_objects.m_objectsPos[i])));
			break;
		default:
			break;
		}
	}

	
}

LevelEditorItems::LevelEditorItems(Camera* t_camera) : viewObserver(t_camera)
{
	equippedItem = nullptr;

	START_DIST = sf::Vector2f(viewObserver->getTopLeftOfView().x - 600, viewObserver->getTopLeftOfView().y);
	selectionStartDistance = START_DIST;

	//distanceBetweenItems = (ScreenSize::s_width - START_DIST.x) / (4.5f * viewObserver->cameraScaleDown);
	distanceBetweenItems = (((ScreenSize::s_height / 6.0f) * 4.5f) - START_DIST.y) / (3.5f * viewObserver->cameraScaleDown);

	itemSize = sf::Vector2f(150.0f / viewObserver->cameraScaleDown, 150.0f / viewObserver->cameraScaleDown);

	addItem(new NoiseMaker());

	addItem(new Scanner(nullptr, nullptr));
	addItem(new Book(""));
	addItem(new SmokeBomb(nullptr));

	addItem(new PlayerPlacer());
	addItem(new AlienPlacer());

	selection.setFillColor(sf::Color(0, 0, 0, 0));
	selection.setOutlineColor(sf::Color::Green);
	selection.setOutlineThickness(20 / viewObserver->cameraScaleDown);
	selection.setSize(sf::Vector2f(itemSize.x + SELECTION_OFFSET, itemSize.y + SELECTION_OFFSET));
	selection.setPosition(selectionStartDistance);
	selection.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));

	equippedItemHighlight.setFillColor(sf::Color(255, 255, 255, 100));
	equippedItemHighlight.setSize(sf::Vector2f((220.0f / viewObserver->cameraScaleDown) + SELECTION_OFFSET, (220.0f / viewObserver->cameraScaleDown) + SELECTION_OFFSET));
	equippedItemHighlight.setPosition(selectionStartDistance);
	equippedItemHighlight.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));

	for (Item* t_item : storedItems)
	{
		t_item->changeSize(itemSize);
	}


	selection.setFillColor(sf::Color(0, 0, 0, 0));
	selection.setOutlineColor(sf::Color::Green);
	selection.setOutlineThickness(20);
	selection.setSize(sf::Vector2f(200.0f + SELECTION_OFFSET,
		200.0f + SELECTION_OFFSET));
	selection.setPosition(selectionStartDistance);
	selection.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));

	equippedItemHighlight.setFillColor(sf::Color(255, 255, 255, 100));
	equippedItemHighlight.setSize(sf::Vector2f(200.0f + SELECTION_OFFSET, 200.0f + SELECTION_OFFSET));
	equippedItemHighlight.setPosition(selectionStartDistance);
	equippedItemHighlight.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));
}

void LevelEditorItems::handleKeyInventoryInput(sf::Vector2f& t_mousePos, sf::Event& event)
{
	if (event.type == sf::Event::MouseMoved)
	{
		std::vector<sf::Vector2f> hovered;

		for (int i = 0; i < ITEM_AMOUNT; i++)
		{
			if (mouseOverItem(i, t_mousePos) && selection.getPosition() != storedItems[i]->getBody().getPosition())
			{
				hovered.push_back(sf::Vector2f(i, CollisionHandler::distance(t_mousePos, storedItems[i]->getBody().getPosition())));
				std::cout << storedItems[i]->getBody().getTexture()->getSize().x << ", " << storedItems[i]->getBody().getTexture()->getSize().y << std::endl;
			}
		}
		
		if (hovered.size() == 1)
		{
			selection.setPosition(storedItems[hovered[0].x]->getBody().getPosition());
			SoundManager::causeSound(NoiseTypes::MoveSelection);
			selectedItemSpot = hovered[0].x;
			selection.setOutlineColor(sf::Color::Green);

			if (hovered[0].x == 2)
			{
				int i = 0;
			}
		}

		else if (hovered.size() > 1)
		{
			sf::Vector2f closestSelection = hovered[0];
			for (sf::Vector2f tempVector : hovered)
			{
				if (closestSelection.y > tempVector.y)
				{
					closestSelection = tempVector;
				}
			}

			selection.setPosition(storedItems[closestSelection.x]->getBody().getPosition());
			SoundManager::causeSound(NoiseTypes::MoveSelection);
			selectedItemSpot = closestSelection.x;
			selection.setOutlineColor(sf::Color::Green);
		}
	}

	else if (event.type == sf::Event::MouseButtonReleased)
	{
		if (mouseOverItem(previousItemSpot, t_mousePos))
		{
			if (equippedItem == storedItems[selectedItemSpot])
			{
				equippedItem = nullptr;
			}

			else
			{
				equippedItem = storedItems[selectedItemSpot];
			}

			SoundManager::causeSound(NoiseTypes::Select);	
		}

		selection.setOutlineColor(sf::Color::Green);
	}

	else if (event.type == sf::Event::MouseButtonPressed)
	{
		if (mouseOverItem(selectedItemSpot, t_mousePos))
		{
			selection.setOutlineColor(sf::Color::Yellow);
			previousItemSpot = selectedItemSpot;
		}
	}

}

void LevelEditorItems::render(sf::RenderWindow& t_window)
{
	for (Item* t_item : storedItems)
	{
		if (!t_item->inInventory())
		{
			t_item->renderOnGrid(t_window);
		}
	}

	if (!storedItems.empty())
	{
		int itemPlacement = 0;
		Item* drawnItem;
		sf::Vector2f itemPoint;

		for (int col = 0; col < ITEM_AMOUNT; col++)
		{
			itemPoint = sf::Vector2f(START_DIST.x, START_DIST.y + (itemPlacement * distanceBetweenItems));

			drawnItem = getItem(static_cast<ItemType>(col + 1));
			drawnItem->moveBody(itemPoint);

			drawnItem->renderInInventory(t_window);

			if (equippedItem != nullptr && drawnItem->getItemType() == equippedItem->getItemType())
			{
				equippedItemHighlight.setPosition(drawnItem->getBody().getPosition());
				t_window.draw(equippedItemHighlight);
			}

			itemPlacement++;

		}
	}

	t_window.draw(selection);
}

std::vector<Item*> LevelEditorItems::getAllItems()
{
	return storedItems;
}