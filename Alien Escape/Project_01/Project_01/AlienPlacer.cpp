#include "AlienPlacer.h"

AlienPlacer::AlienPlacer() : Item(ItemType::ALIEN_PLACER)
{}

AlienPlacer::AlienPlacer(sf::Vector2f t_newPos) : Item(ItemType::ALIEN_PLACER, t_newPos)
{
	m_itemBody.setPosition(t_newPos);
}

void AlienPlacer::useItem()
{
}

void AlienPlacer::update()
{
}

void AlienPlacer::changeSize(sf::Vector2f t_size)
{
	setTextureRect(sf::IntRect(0, 0, WIDTH, HEIGHT));
	m_itemBody.setOrigin(m_bodyTexture.getSize().x / SPRITESHEET_COL / 1.3, m_bodyTexture.getSize().y / SPRITESHEET_ROW);
	sf::Vector2f newScale = sf::Vector2f(t_size.x / WIDTH, t_size.x / HEIGHT);
	m_itemBody.setScale(newScale);
}
