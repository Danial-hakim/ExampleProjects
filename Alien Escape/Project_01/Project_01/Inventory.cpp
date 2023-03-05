#include "Inventory.h"

bool Inventory::notEnoughItems(std::array<int, 4> items, int t_col)
{
	return items[t_col] <= 0;
}

void Inventory::moveSelectionLeft()
{
	if (selection.getPosition().x - distanceBetweenItems <= viewObserver->getTopLeftOfView().x)
	{
		selection.setPosition(selectionStartDistance.x + (distanceBetweenItems * 3), selectionStartDistance.y);
		
		selectedStorageSpot = 3;
	}

	else
	{
		selection.setPosition(selection.getPosition().x - distanceBetweenItems,
			selectionStartDistance.y);

			selectedStorageSpot--;
	}

	SoundManager::causeSound(NoiseTypes::MoveSelection);
}

void Inventory::moveSelectionRight()
{
	if (selection.getPosition().x + selection.getSize().x + distanceBetweenItems >= viewObserver->getBottomRightOfView().x)
	{
		selection.setPosition(selectionStartDistance);

		selectedStorageSpot = 0;
	}

	else
	{
		selection.setPosition(selection.getPosition().x + distanceBetweenItems,
			selectionStartDistance.y);

		selectedStorageSpot++;
	}
	SoundManager::causeSound(NoiseTypes::MoveSelection);
}

void Inventory::pickUpItem(sf::Vector2f playerPos)
{
	Item* pickedUpItem = mapItemsObserver->pickUpItem(playerPos);

	if (pickedUpItem != nullptr)
	{
		pickedUpItem->pickUpItem();
		pickedUpItem->changeSize(ìtemSize);

		Scanner* scanner = dynamic_cast<Scanner*>(pickedUpItem);

		if (scanner != nullptr)
		{
			scanner->setCameraRef(viewObserver);
		}

		storedItems.push_back(pickedUpItem);
	}

}

void Inventory::virtualUpdate()
{
	for (Item* indexItem : storedItems)
	{
		indexItem->update();

		if (indexItem->shouldBeDeleted())
		{
			deleteItem(indexItem);
		}
	}
}

Inventory::Inventory(ScannerObserver* t_observer, Camera* t_camera) : mapItemsObserver(t_observer)
{
	viewObserver = t_camera;

	inventoryOpen = false;
	equippedItem = nullptr;

	background.setFillColor(sf::Color(0,0,0,150));
	background.setSize(sf::Vector2f(ScreenSize::s_width, ScreenSize::s_height));
}

void Inventory::getFont(sf::Font* t_font)
{
	m_font = t_font;

	for (int i = 0; i < ITEM_AMOUNT; i++)
	{
		m_itemCount[i].setFont(*m_font);
		m_itemCount[i].setString(std::to_string(i));
		m_itemCount[i].setOutlineColor(sf::Color(107, 107, 107));
		m_itemCount[i].setOutlineThickness(5.0f / viewObserver->cameraScaleUp);
		m_itemCount[i].setCharacterSize(45.0f / viewObserver->cameraScaleUp);
		m_itemCount[i].setPosition(sf::Vector2f(int(START_DIST.x + ìtemSize.x / 3.0f + (i * distanceBetweenItems)), int(START_DIST.y + ìtemSize.y / 4.0f)));
	}
}

void Inventory::initialise()
{
	START_DIST = sf::Vector2f(ScreenSize::s_width / (4.5f * viewObserver->cameraScaleUp), ScreenSize::s_height / (2.0f * viewObserver->cameraScaleUp));

	distanceBetweenItems = (ScreenSize::s_width - START_DIST.x) / (4.5f * viewObserver->cameraScaleUp);

	ìtemSize = sf::Vector2f(220.0f / viewObserver->cameraScaleUp, 220.0f / viewObserver->cameraScaleUp);

	selection.setFillColor(sf::Color(0, 0, 0, 0));
	selection.setOutlineColor(sf::Color::Green);
	selection.setOutlineThickness(20 / viewObserver->cameraScaleUp);
	selection.setSize(sf::Vector2f((220.0f / viewObserver->cameraScaleUp) + SELECTION_OFFSET,
		(220.0f / viewObserver->cameraScaleUp) + SELECTION_OFFSET));
	selection.setPosition(selectionStartDistance);
	selection.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));

	equippedItemHighlight.setFillColor(sf::Color(255, 255, 255, 100));
	equippedItemHighlight.setSize(sf::Vector2f((220.0f / viewObserver->cameraScaleUp) + SELECTION_OFFSET, (220.0f / viewObserver->cameraScaleUp) + SELECTION_OFFSET));
	equippedItemHighlight.setPosition(selectionStartDistance);
	equippedItemHighlight.setOrigin(sf::Vector2f(selection.getSize().x / 2.0f, selection.getSize().y / 2.0f));

	for (Item* t_item : storedItems)
	{
		t_item->changeSize(ìtemSize);
	}
}

void Inventory::handleKeyInventoryInput(sf::Event& event, sf::Vector2f playerPos, Direction t_angle)
{
	Scanner* scanner = dynamic_cast<Scanner*>(getItem(ItemType::SCANNER));

	if (event.type != sf::Event::MouseButtonPressed)
	{
		sf::Keyboard::Key key = event.key.code;

		if (sf::Event::KeyPressed == event.type)
		{
			if (scanner == nullptr || !scanner->isScannerOn())
			{
				if (key == sf::Keyboard::I)
				{
					inventoryOpen = !inventoryOpen;
					selectedStorageSpot = 0;
				}

				if (inventoryOpen)
				{
					if (key == sf::Keyboard::A || key == sf::Keyboard::Left)
					{
						moveSelectionLeft();
					}

					else if (key == sf::Keyboard::D || key == sf::Keyboard::Right)
					{
						moveSelectionRight();
					}

					else if (key == sf::Keyboard::E)
					{
						equipItem();
						selection.setOutlineColor(sf::Color::Yellow);
					}
				}

				else
				{
					if (key == sf::Keyboard::E)
					{
						useItem(playerPos,t_angle);
					}
				}
			}

			else
			{
				if (key == sf::Keyboard::E)
				{
					useItem(playerPos, t_angle);
				}
			}
		}

		else 
		{
			if (key == sf::Keyboard::E && inventoryOpen)
			{
				selection.setOutlineColor(sf::Color::Green);
			}

			else if (key == sf::Keyboard::O)
			{
				pickUpItem(playerPos);
			}
		}
	}
}

bool Inventory::inventoryIsOpen()
{
	return inventoryOpen;
}


void Inventory::equipItem()
{
	float selectionPos = std::roundf(selection.getPosition().x);

	for (Item* selectedItem : storedItems)
	{
		float i = std::roundf(selectedItem->getBody().getPosition().x);

		if (selectionPos == i
			&& selectedItem->inInventory())
		{
			equippedItem = selectedItem;
			break;
		}
	}
	
	if (equippedItem != nullptr)
	{
		std::cout << "equipped " << equippedItem->getItemType() << std::endl;
	}

	SoundManager::causeSound(NoiseTypes::Select);
}

void Inventory::useItem(sf::Vector2f playerPos, Direction t_angle)
{
	if (equippedItem != nullptr)
	{
		bool anotherItem = false;

		if (equippedItem->getItemType() != ItemType::SCANNER)
		{
			try
			{
				ThrowableItem* thrownItem = static_cast<ThrowableItem*>(equippedItem);

				thrownItem->useItem(playerPos, t_angle);
				//particle system initialised
			}

			catch (int e)
			{
				if (equippedItem->getItemType() == ItemType::BOOK)
				{
					equippedItem->useItem();
					deleteItem(equippedItem);
				}
			}


			for (Item* t_item : storedItems)
			{
				if (equippedItem->getItemType() == t_item->getItemType() && equippedItem != t_item
					&& t_item->inInventory())
				{
					anotherItem = true;
					equippedItem = t_item;
					break;
				}
			}

			if (!anotherItem)
			{
				equippedItem = nullptr;
			}

		}

		else
		{
			Scanner* scanner = dynamic_cast<Scanner*>(getItem(ItemType::SCANNER));
			scanner->useItem(playerPos);
		}

	}
}

void Inventory::render(sf::RenderWindow& t_window)
{
	Scanner* scanner = dynamic_cast<Scanner*>(getItem(ItemType::SCANNER));

	for (Item* t_item : storedItems)
	{
		if (!t_item->inInventory())
		{
			t_item->renderOnGrid(t_window);
		}
	}

	if (scanner != nullptr)
	{
		scanner->renderOnGrid(t_window);
	}

	if (inventoryOpen)
	{
		t_window.draw(background);

		updateInventoryLocation();

		renderItems(t_window);

		t_window.draw(selection);
	}


}

void Inventory::renderItems(sf::RenderWindow& t_window)
{
	if (!storedItems.empty())
	{
		std::array<int, 4> items = countItems();
		int itemPlacement = 0;
		Item* drawnItem;
		sf::Vector2f itemPoint;

		for (int col = 0; col < ITEM_AMOUNT; col++)
		{
			if (notEnoughItems(items, col))
			{
				continue;
			}

			itemPoint = sf::Vector2f(START_DIST.x + (itemPlacement * distanceBetweenItems), START_DIST.y);
			drawnItem = getItem(static_cast<ItemType>(col + 1));
			drawnItem->moveBody(itemPoint);

			m_itemCount[itemPlacement].setString(std::to_string(items[col]));

			drawnItem->renderInInventory(t_window);

			if (equippedItem != nullptr && drawnItem->getItemType() == equippedItem->getItemType())
			{
				equippedItemHighlight.setPosition(drawnItem->getBody().getPosition());
				t_window.draw(equippedItemHighlight);
			}

			t_window.draw(m_itemCount[itemPlacement]);

			itemPlacement++;
		}
	}
}

std::array<int, 4> Inventory::countItems()
{
	std::array<int, 4> itemAmounts { 0,0,0,0 };

	for (Item* indexItem : storedItems)
	{
		if (indexItem->inInventory())
		{
			itemAmounts[static_cast<int>(indexItem->getItemType()) - 1]++;
		}
	}

	return itemAmounts;
}

void Inventory::updateInventoryLocation()
{
	START_DIST = sf::Vector2f((ScreenSize::s_width / (5.0f * viewObserver->cameraScaleUp)) + viewObserver->getTopLeftOfView().x,
		(ScreenSize::s_height / (2.0f * viewObserver->cameraScaleUp)) + viewObserver->getTopLeftOfView().y);

	selectionStartDistance = START_DIST;

	sf::Vector2f selectionPos = sf::Vector2f(selectionStartDistance.x + distanceBetweenItems * selectedStorageSpot, selectionStartDistance.y);

	selection.setPosition(selectionPos);

	for (int i = 0; i < ITEM_AMOUNT; i++)
	{
		m_itemCount[i].setPosition(sf::Vector2f(START_DIST.x + ìtemSize.x / 3.0f + (i * distanceBetweenItems), START_DIST.y + ìtemSize.y / 4.0f));
	}
}
