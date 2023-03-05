#include "Scanner.h"

Scanner::Scanner(Camera* t_camera, ScannerObserver* t_observer) : Item(ItemType::SCANNER), cameraRef(t_camera)
{
	initialise(t_observer);
}

Scanner::Scanner(sf::Vector2f t_position, ScannerObserver* t_observer) : Item(ItemType::SCANNER, t_position), cameraRef(nullptr)
{
	initialise(t_observer);
}

void Scanner::initialise(ScannerObserver* t_observer)
{
	observer = t_observer;
	background.setSize(sf::Vector2f(ScreenSize::s_width, ScreenSize::s_height));
	background.setFillColor(sf::Color(0, 128, 15, 200));
	screenOpen = false;
}

void Scanner::useItem(sf::Vector2f t_playerPos)
{
	std::cout << "Use Scanner" << std::endl;

	if (screenOpen == false)
	{
		SoundManager::causeSound(NoiseTypes::Scanner);
	}

	cameraRef->changeSizeInGame(t_playerPos);

	screenOpen = !screenOpen;
}

void Scanner::useItem()
{
	std::cout << "Error in Scanner !" << std::endl;
}

void Scanner::update(){}

void Scanner::storeCamera(Camera* t_camera)
{
	cameraRef = t_camera;
}


void Scanner::renderInInventory(sf::RenderWindow& t_window)
{
	if (!onMap)
	{
		t_window.draw(m_itemBody);
	}
}

void Scanner::renderOnGrid(sf::RenderWindow& t_window)
{
	if (screenOpen)
	{
		t_window.draw(background);
		observer->drawItemsForScanner(t_window);
	}

	if (onMap)
	{
		sf::Vector2f newScale;

		newScale = sf::Vector2f(cellWidth / m_itemBody.getTexture()->getSize().x,
			cellHeight / m_itemBody.getTexture()->getSize().y);


		m_itemBody.setPosition(mapPosition);
		m_itemBody.setScale(newScale);

		t_window.draw(m_itemBody);

		m_itemBody.setScale(1, 1);
	}
}

bool Scanner::isScannerOn()
{
	return screenOpen;
}

void Scanner::setCameraRef(Camera* t_camera)
{
	cameraRef = t_camera;
}
