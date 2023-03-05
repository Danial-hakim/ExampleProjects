#include "NoiseMaker.h"

NoiseMaker::NoiseMaker() : ThrowableItem(ItemType::NOISE_MAKER)
{
}

NoiseMaker::NoiseMaker(sf::Vector2f t_position)
	: ThrowableItem(ItemType::NOISE_MAKER, t_position)
{
}	

void NoiseMaker::useItem(){}

void NoiseMaker::update()
{
	if (updateThrow())
	{
		std::cout << "Use NoiseMaker" << std::endl;
		SoundManager::causeSound(NoiseTypes::NoiseMaker, m_itemBody.getPosition());
		usedUp = true;
	}
}
