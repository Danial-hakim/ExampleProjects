#include "Item.h"

Item::Item(ItemType t_type) : type(t_type), onMap(false), m_bodyTexture(sf::Texture())
{
	initialise(t_type);
}

Item::Item(ItemType t_type, sf::Vector2f t_position)
	: type(t_type), onMap(true), m_bodyTexture(sf::Texture())
{
	initialise(t_type);
	mapPosition = t_position;
	//m_itemBody.setPosition(mapPosition);
}

void Item::setTextureRect(sf::IntRect t_newRect)
{
	m_itemBody.setTextureRect(t_newRect);
}

std::ostream& operator << (std::ostream& strm, const ItemType tt)
{
	const std::string nameTT[] = { "DEFAULT", "SMOKE_BOMB", "NOISE_MAKER", "SCANNER", "BOOK", "Player", "Alien"};
	return strm << nameTT[static_cast<int>(tt)];
}

void Item::update()
{
}

bool Item::shouldBeDeleted()
{
	return usedUp;
}

void Item::changeSize(sf::Vector2f t_size)
{
	sf::Vector2f newScale = sf::Vector2f(t_size.x / m_bodyTexture.getSize().x, t_size.y / m_bodyTexture.getSize().y);
	m_itemBody.setScale(newScale);
}

void Item::initialise(ItemType t_type)
{
	pickedUp = !onMap;

	type = t_type;
	std::ostringstream texture(std::ostringstream::ate);
	texture << "Assets/Sprites/FinalSprites/" << type << ".png";

	if (!m_bodyTexture.loadFromFile(texture.str()))
	{
		std::cout << "Could not load texture" << std::endl;
	}

	m_itemBody.setTexture(m_bodyTexture);
	m_itemBody.setOrigin( sf::Vector2f(m_bodyTexture.getSize().x / 2.0f, m_bodyTexture.getSize().y / 2.0f));
}

ItemType Item::getItemType()
{
	return type;
}

void Item::pickUpItem()
{
	onMap = false;
	pickedUp = true;

}

sf::Sprite Item::getBody()
{
	return m_itemBody;
}

void Item::setMapPos(sf::Vector2f t_newPos)
{
	mapPosition = t_newPos;
}

sf::Vector2f Item::getMapPos()
{
	return mapPosition;
}

void Item::moveBody(sf::Vector2f newPos)
{
	m_itemBody.setPosition(newPos);
}

void Item::renderOnGrid(sf::RenderWindow& t_window)
{
	sf::Vector2f newScale;
	sf::Vector2f passedValue;

	newScale = sf::Vector2f(cellWidth / m_itemBody.getTexture()->getSize().x,
		cellHeight / m_itemBody.getTexture()->getSize().y);

	passedValue = sf::Vector2f(cellWidth, cellHeight);

	if (newScale.x > newScale.y)
	{
		passedValue.x = newScale.y * m_itemBody.getTexture()->getSize().x;
	}

	else
	{
		passedValue.y = newScale.x * m_itemBody.getTexture()->getSize().y;
	}

	m_itemBody.setPosition(mapPosition);

	changeSize(passedValue);

	t_window.draw(m_itemBody);

	changeSize(sf::Vector2f(1,1));
}



void Item::renderInInventory(sf::RenderWindow& t_window)
{
	t_window.draw(m_itemBody);
}

bool Item::inInventory()
{
	return !onMap;
}