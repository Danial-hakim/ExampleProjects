#pragma once
#include "SFML/Audio.hpp"
#include <iostream>
#include "SoundLevels.h"

class Sound
{
private:
	SoundLevel m_level;
	sf::Sound noiseMade;
	sf::SoundBuffer m_buffer;
	bool hasPlayed;
public:
	Sound(SoundLevel t_level, std::string t_soundPath);
	void play();
	bool finishedPlaying();
	bool hasntStartedPlaying();
	SoundLevel getSoundLevel();

	bool operator== (Sound const& t_other);
};

