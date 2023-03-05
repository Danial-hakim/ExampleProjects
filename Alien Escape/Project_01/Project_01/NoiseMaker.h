	#pragma once
#include "ThrowableItem.h"
#include "SoundManager.h"


class NoiseMaker : public ThrowableItem
{
public:
	NoiseMaker();
	NoiseMaker(sf::Vector2f t_position);
	void useItem();
	void update();
};

