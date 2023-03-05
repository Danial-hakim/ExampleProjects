#include "playerMission.h"

playerMission::playerMission(ScannerObserver* t_mapItemsRef, Camera* t_camera, Grid* t_grid)
	: mapItemRef(t_mapItemsRef), viewObserver(t_camera), gridRef(t_grid)
{
	std::srand(std::time(0));
}

void playerMission::init(sf::Font* t_font)
{
	if (!escapeTexture.loadFromFile("./Assets/Sprites/FinalSprites/walls/escape.jpg"))
	{
		std::cout << "spritesheet error - escape" << std::endl;
	}

	escapeSprite.setTexture(escapeTexture);

	goalFont = t_font;
	goalText.setFont(*goalFont);
	goalText.setOutlineColor(sf::Color(50, 50, 50));
	goalText.setOutlineThickness(5.0f / viewObserver->cameraScaleUp);
	goalText.setOrigin(goalText.getGlobalBounds().width / 2.0f, goalText.getGlobalBounds().height);
	//goalText.setCharacterSize(45.0f / viewObserver->cameraScaleUp);
	goalText.setScale(sf::Vector2f(viewObserver->cameraScaleUp / 5.0f, viewObserver->cameraScaleUp / 5.0f));

}

void playerMission::update(sf::Vector2f t_playerPos)
{
	goalText.setPosition(sf::Vector2f(viewObserver->getCentre().x,viewObserver->getTopLeftOfView().y + GOAL_TEXT_Y_OFFSET));

	int numberOfBooksLeft = 0;
	std::vector<Item*> items = mapItemRef->getItems();

	for (Item* oneItem : items)
	{
		if (oneItem->getItemType() == ItemType::BOOK)
		{
			numberOfBooksLeft++;
		}
	}

	if (numberOfBooksLeft == 0)
	{
		if (!gotAllBooks)
		{
			goalText.setString("Mission: Find the escape!");
			goalText.setScale(sf::Vector2f(1, 1));
			goalText.setOrigin(goalText.getGlobalBounds().width / 2.0f, goalText.getGlobalBounds().height);
			goalText.setScale(sf::Vector2f(viewObserver->cameraScaleUp / 5.0f, viewObserver->cameraScaleUp / 5.0f));

			std::vector<Cell> cells = gridRef->getCell();


			int cellID = 0;
			do
			{
				cellID = std::rand() % cells.size();
			}

			while (!cells[cellID].getPassable());

			escapeSprite.setScale(cellWidth / 64, cellHeight / 64);
			escapeSprite.setOrigin(escapeTexture.getSize().x / 2.0f, escapeTexture.getSize().y / 2.0f);
			escapeSprite.setPosition(ScreenSize::getActualPosition(cellID));

			gotAllBooks = true;
		}

		if (escapeSprite.getGlobalBounds().contains(t_playerPos))
		{
			SceneTypes::updateScene(Scenes::WIN_SCREEN);
		}

	}

	else
	{
		goalText.setString("Mission: find all books - " + std::to_string(numberOfBooksLeft) + " remaining.");
		goalText.setScale(sf::Vector2f(1, 1));
		goalText.setOrigin(goalText.getGlobalBounds().width / 2.0f, goalText.getGlobalBounds().height);
		goalText.setScale(sf::Vector2f(viewObserver->cameraScaleUp / 5.0f, viewObserver->cameraScaleUp / 5.0f));
	}
}

void playerMission::render(sf::RenderWindow& t_window)
{
	t_window.draw(goalText);
}

void playerMission::renderEscape(sf::RenderWindow& t_window)
{
	if (gotAllBooks)
	{
		t_window.draw(escapeSprite);
	}
}

void playerMission::restart()
{
	gotAllBooks = false;
}
