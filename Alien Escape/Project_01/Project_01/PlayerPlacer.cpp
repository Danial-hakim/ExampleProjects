#include "PlayerPlacer.h"

PlayerPlacer::PlayerPlacer() : Item(ItemType::PLAYER_PLACER)
{}

PlayerPlacer::PlayerPlacer(sf::Vector2f t_newPos) : Item(ItemType::PLAYER_PLACER, t_newPos)
{
	m_itemBody.setPosition(t_newPos);
}

void PlayerPlacer::useItem()
{
}

void PlayerPlacer::update()
{
}

void PlayerPlacer::changeSize(sf::Vector2f t_size)
{
	setTextureRect(sf::IntRect(0, 0, WIDTH, HEIGHT));
	m_itemBody.setOrigin(m_bodyTexture.getSize().x / SPRITESHEET_COL, m_bodyTexture.getSize().y / SPRITESHEET_ROW);
	sf::Vector2f newScale = sf::Vector2f(t_size.x / WIDTH, t_size.x / HEIGHT);
	m_itemBody.setScale(newScale);
}
