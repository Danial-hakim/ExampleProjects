#pragma once
#include "Sound.h"
#include <vector>

enum class NoiseTypes
{
	NoiseMaker,
	AlienGrowl,
	AlienScreech,
	PlayerRun,
	Scanner,
	MoveSelection,
	Select,
	MenuSelect
};

class SoundManager
{
private:

	static std::vector<Sound*> currentSounds;
	static sf::SoundBuffer buffer;

	static void makeSound(NoiseTypes t_type);

public:

	static void causeSound(NoiseTypes t_type);
	static void causeSound(NoiseTypes t_type, sf::Vector2f t_positionOfSound);


	static void update();
};

