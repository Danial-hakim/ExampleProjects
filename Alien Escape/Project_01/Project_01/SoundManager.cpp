#include "SoundManager.h"
#include "AlienHearing.h"

std::vector<Sound*> SoundManager::currentSounds;


void SoundManager::makeSound(NoiseTypes t_type)
{
	switch (t_type)
	{
	case NoiseTypes::NoiseMaker:
		currentSounds.push_back(new Sound(SoundLevel::ADJACENT_SHIP, "Assets/Sound/NoiseMaker.wav"));
		break;
	case NoiseTypes::AlienGrowl:
		currentSounds.push_back(new Sound(SoundLevel::ADJACENT_SHIP, "Assets/Sound/_alienGrowl_.wav"));
		break;
	case NoiseTypes::AlienScreech:
		break;
	case NoiseTypes::PlayerRun:
		currentSounds.push_back(new Sound(SoundLevel::ADJACENT_SHIP, ""));
		break;
	case NoiseTypes::Scanner:
		currentSounds.push_back(new Sound(SoundLevel::ALL_SHIPS, "Assets/Sound/Scanner.wav"));
		break;
	case NoiseTypes::MoveSelection:
		currentSounds.push_back(new Sound(SoundLevel::UNNOTICABLE, "Assets/Sound/moveSelectCursor.wav"));
		break;
	case NoiseTypes::Select:
		currentSounds.push_back(new Sound(SoundLevel::UNNOTICABLE, "Assets/Sound/Select.wav"));
		break;
	case NoiseTypes::MenuSelect:
		currentSounds.push_back(new Sound(SoundLevel::UNNOTICABLE, "Assets/Sound/menuSound.wav"));
		break;
	default:
		break;
	}
}

void SoundManager::causeSound(NoiseTypes t_type)
{
	makeSound(t_type);

	if (t_type == NoiseTypes::Scanner)
	{
		AlienHearing::didAlienHearThat(currentSounds.back()->getSoundLevel());
	}
}

void SoundManager::causeSound(NoiseTypes t_type, sf::Vector2f t_positionOfSound)
{
	makeSound(t_type);
	AlienHearing::didAlienHearThat(currentSounds.back()->getSoundLevel(), t_positionOfSound);
}

void SoundManager::update()
{
	for (Sound* s : currentSounds)
	{
		if (s->finishedPlaying())
		{
			remove(currentSounds.begin(), currentSounds.end(), s);
		}

		else if (s->hasntStartedPlaying())
		{		
			s->play();
		}
	}
}
