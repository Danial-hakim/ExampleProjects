#include "LevelEditor.h"

LevelEditor::LevelEditor(sf::RenderWindow& t_window, LevelData& t_layout) : editorsView(t_window), items(&editorsView), layout(t_layout){}

void LevelEditor::init(LevelData* t_level)
{
	if (!buttonTexture.loadFromFile("./Assets/Sprites/button.png"))
	{
		std::cout << "Error with button png" << std::endl;
	}

	if (!menuIconTextures[0].loadFromFile("./Assets/Sprites/FinalSprites/menu.png"))
	{
		std::cout << "Error with menu png" << std::endl;
	}

	if (!menuIconTextures[1].loadFromFile("./Assets/Sprites/FinalSprites/cancel.png"))
	{
		std::cout << "Error with save png" << std::endl;
	}

	if (!menuIconTextures[2].loadFromFile("./Assets/Sprites/FinalSprites/save.png"))
	{
		std::cout << "Error with cancel png" << std::endl;
	}

	for (int i = 0; i < icons; i++)
	{
		menuIconSprites[i].setTexture(menuIconTextures[i]);
	}

	m_grid.init(t_level->m_floors[0].walls);
	editorsView.transformCamera(sf::Vector2f(200, ScreenSize::s_height / 2.0f), false);
	editorsView.scaleCameraToEditor();

	sideMenu.setFillColor(sf::Color(209, 209, 209));
	sideMenu.setSize(sf::Vector2f(editorsView.getSize().x / 3.45f, editorsView.getSize().y));
	sideMenu.setPosition(editorsView.getTopLeftOfView().x - 100, editorsView.getTopLeftOfView().y);

	centreXPos = editorsView.getCentre().x;


	menuButton.setColor(sf::Color(209, 209, 209));
	menuButton.setTexture(buttonTexture);
	menuButton.setScale(sf::Vector2f(0.6f,2.0f));
	menuButton.setPosition(editorsView.getTopLeftOfView().x + editorsView.getSize().x / 3.8f,
		editorsView.getTopLeftOfView().y + editorsView.getSize().y / 35.0f);

	menuIconSprites[0].setPosition(menuButton.getPosition() + sf::Vector2f(20, 50));
	menuIconSprites[0].setScale(sf::Vector2f(3.1f, 3.1f));

	backButton.setColor(sf::Color(255, 80, 80));
	backButton.setTexture(buttonTexture);
	backButton.setScale(sf::Vector2f(0.6f, 2.0f));
	backButton.setPosition(editorsView.getTopLeftOfView().x + editorsView.getSize().x / 3.8f,
		editorsView.getBottomRightOfView().y - editorsView.getSize().y / 8.0f);

	menuIconSprites[1].setPosition(backButton.getPosition() + sf::Vector2f(0, 10));
	menuIconSprites[1].setScale(sf::Vector2f(0.095f, 0.095f));

	saveButton.setColor(sf::Color(235, 216, 52));
	saveButton.setTexture(buttonTexture);
	saveButton.setScale(sf::Vector2f(0.6f, 2.0f));
	saveButton.setPosition(editorsView.getTopLeftOfView().x + editorsView.getSize().x / 2.9f,
		editorsView.getTopLeftOfView().y + editorsView.getSize().y / 35.0f);

	menuIconSprites[2].setPosition(saveButton.getPosition() + sf::Vector2f(20, 30));
	menuIconSprites[2].setScale(sf::Vector2f(0.3f, 0.3f));


	items.init(t_level->m_objects[0]);
	//m_gridRenderer.init(&m_grid);
}

void LevelEditor::update(double dt)
{
	editorsView.updateCamera(sf::Vector2f(centreXPos, editorsView.getCentre().y));
}

void LevelEditor::render(sf::RenderWindow& t_window)
{
	editorsView.updateCamera();
	t_window.draw(backButton);
	t_window.draw(menuButton);
	t_window.draw(saveButton);
	t_window.draw(sideMenu);



	for (int i = 0; i < icons; i++)
	{
		t_window.draw(menuIconSprites[i]);
	}

	//m_gridRenderer.draw(t_window);
	m_grid.draw(t_window, true);
	items.render(t_window);

}

void LevelEditor::processGameEvents(sf::Event& event)
{
	sf::Vector2i mousePos = sf::Mouse::getPosition(editorsView.getWindowRef());

	sf::Vector2f actualMousePos = sf::Vector2f(mousePos);

	actualMousePos += sf::Vector2f(7, 10);

	if (event.type == sf::Event::MouseButtonPressed)
	{
		sf::Vector2i position = m_grid.gridMouseCheck(actualMousePos, editorsView.getWindowRef(), items.haveItemEquipped());
		//m_gridRenderer.updateGrid(sf::Vector2f(actualMousePos) - sf::Vector2f(350, 0));

		if (position != sf::Vector2i(-1,-1) && items.haveItemEquipped())
		{
			items.placeDownItem(position);
		}

		

		else if (CollisionHandler::mouse_cell(sf::Vector2f(editorsView.getWindowRef().mapCoordsToPixel(menuButton.getPosition())),
			menuButton.getGlobalBounds(), sf::Vector2f(actualMousePos)))
		{
			if (centreXPos < 700)
			{
				centreXPos = 700;
			}

			else
			{
				centreXPos = 200;
			}
		}

		else if (CollisionHandler::mouse_cell(sf::Vector2f(editorsView.getWindowRef().mapCoordsToPixel(backButton.getPosition())),
			backButton.getGlobalBounds(), sf::Vector2f(actualMousePos)))
		{
			SceneTypes::updateScene(Scenes::MENU);
			saveChanges();
		}

		else if (CollisionHandler::mouse_cell(sf::Vector2f(editorsView.getWindowRef().mapCoordsToPixel(saveButton.getPosition())),
			saveButton.getGlobalBounds(), sf::Vector2f(actualMousePos)))
		{
			layout.m_floors[0].walls.clear();

			std::vector<Cell> cells = m_grid.getCell();

			for (Cell oneCell : cells)
			{
				if (!oneCell.getPassable())
				{
					layout.m_floors[0].walls.push_back(sf::Vector2i(oneCell.getPosition()));
				}
			}


			

			LevelLoader::save(layout);
		}
	}
	    items.handleKeyInventoryInput(actualMousePos, event);
}


void LevelEditor::saveChanges()
{
	std::vector<Item*> putDownItems = items.getAllItems();

	std::vector <std::pair<ItemType, sf::Vector2f>> itemsToSave;

	std::vector<sf::Vector2i> walls;
	sf::Vector2f playerPos;
	sf::Vector2f alienPos;

	std::vector<Cell> cells = m_grid.getCell();


	// Walls 
	cells.erase(std::remove_if(cells.begin(), cells.end(),

		[](Cell t_i)
		{
			return t_i.getPassable();
		}
	),
		cells.end());

	for (Cell c : cells)
	{
		walls.push_back(sf::Vector2i(m_grid.getCellRowAndCol(c.getPosition())));
	}


	// Items, Player and Alien
	putDownItems.erase(std::remove_if(putDownItems.begin(), putDownItems.end(),

		[](Item* t_i)
		{
			return t_i->inInventory();
		}
	), 
		putDownItems.end());

	
	for (Item* oneItem : putDownItems)
	{
		if (oneItem->getItemType() == ItemType::PLAYER_PLACER)
		{
			playerPos = m_grid.getCellRowAndCol(oneItem->getMapPos());
		}
			
		else if (oneItem->getItemType() == ItemType::ALIEN_PLACER)
		{
			alienPos = m_grid.getCellRowAndCol(oneItem->getMapPos());
		}

		else
		{
			sf::Vector2f pos = m_grid.getCellRowAndCol(oneItem->getMapPos());
			itemsToSave.push_back(std::make_pair(oneItem->getItemType(), pos));
		}
	}


	// Adding them to Level object
	level.m_objects[0].m_alienPos = alienPos;
	level.m_objects[0].m_playerPos = playerPos;

	for (std::pair<ItemType, sf::Vector2f> items : itemsToSave)
	{
		level.m_objects[0].m_objectsPos.push_back(items.second);
		level.m_objects[0].m_objectTypes.push_back(items.first);
	}
	
	for (sf::Vector2i wall : walls)
	{
		level.m_floors[0].walls.push_back(wall);
	}

	LevelLoader::save(level);
}




