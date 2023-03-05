#include "AlienHearing.h"

Alien* AlienHearing::alienRef;

void AlienHearing::alienHeardThat(sf::Vector2f t_positionOfSound)
{
	std::cout << "The alien heard that noise!" << std::endl;
	alienRef->heardNoise(t_positionOfSound);
}

void AlienHearing::alienHeardThat()
{
	std::cout << "The alien heard that noise!" << std::endl;
	alienRef->heardNoise();
}

void AlienHearing::didAlienHearThat(SoundLevel level, sf::Vector2f t_positionOfSound)
{
	switch (level)
	{
	case SoundLevel::UNNOTICABLE:
		break;
	case SoundLevel::SAME_SHIP:
		break;
	case SoundLevel::ADJACENT_SHIP:
		alienHeardThat(t_positionOfSound);
		break;
	case SoundLevel::ALL_SHIPS:
		alienHeardThat(t_positionOfSound);
		break;
	default:
		break;
	}
}

void AlienHearing::didAlienHearThat(SoundLevel level)
{
	switch (level)
	{
	case SoundLevel::UNNOTICABLE:
		break;
	case SoundLevel::SAME_SHIP:
		break;
	case SoundLevel::ADJACENT_SHIP:
		alienHeardThat();
		break;
	case SoundLevel::ALL_SHIPS:
		alienHeardThat();
		break;
	default:
		break;
	}
}

void AlienHearing::storeAlienRef(Alien* t_alien)
{
	alienRef = t_alien;
}
