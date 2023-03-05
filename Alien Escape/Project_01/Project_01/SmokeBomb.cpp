#include "SmokeBomb.h"

SmokeBomb::SmokeBomb(Grid* t_gridRef)
	: ThrowableItem(ItemType::SMOKE_BOMB), gridRef(t_gridRef)
{
}

SmokeBomb::SmokeBomb(sf::Vector2f t_position, Grid* t_gridRef)
	: ThrowableItem(ItemType::SMOKE_BOMB, t_position), gridRef(t_gridRef)
{
	m_itemBody.setPosition(t_position);
}

void SmokeBomb::useItem(){}

void SmokeBomb::update()
{
	if (updateThrow())
	{
		std::cout << "Use Smokebomb" << std::endl;

		gridRef->smokeBombReaction(m_itemBody.getPosition());

		usedUp = true;
	}
}
