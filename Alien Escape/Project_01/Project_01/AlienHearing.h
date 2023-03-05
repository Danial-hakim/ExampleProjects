#pragma once
#include <iostream>
#include "SoundLevels.h"
#include "SFML/Graphics.hpp"
#include "Alien.h"

class AlienHearing
{
private:

	static Alien* alienRef;
	static void alienHeardThat(sf::Vector2f t_positionOfSound);
	static void alienHeardThat();

public:
	static void didAlienHearThat(SoundLevel level, sf::Vector2f t_positionOfSound);
	static void didAlienHearThat(SoundLevel level);
	static void storeAlienRef(Alien* t_alien);
};