#include "Sound.h"

Sound::Sound(SoundLevel t_level, std::string t_soundPath) : m_level(t_level)
{
	if (!m_buffer.loadFromFile(t_soundPath))
	{
		std::cout << "error loading sound" << std::endl;
	}

	noiseMade.setBuffer(m_buffer);

	if (t_soundPath == "Assets/Sound/_alienGrowl_.wav")
	{
		noiseMade.setVolume(100);
	}
	else
	{
		noiseMade.setVolume(15);
	}
	hasPlayed = false;
}

void Sound::play()
{
	noiseMade.play();
	hasPlayed = true;
}

bool Sound::finishedPlaying()
{
	return ((noiseMade.getStatus() == sf::Sound::Stopped) && hasPlayed);
}

bool Sound::hasntStartedPlaying()
{
	return !hasPlayed;
}

SoundLevel Sound::getSoundLevel()
{
	return m_level;
}

bool Sound::operator==(Sound const& t_other)
{
	bool theyreEqual = true;

	if (m_level != t_other.m_level)
	{
		theyreEqual = false;
	}

	else if (hasPlayed != t_other.hasPlayed)
	{
		theyreEqual = false;
	}

	return theyreEqual;
}
