#pragma once

#include <SFML/Graphics.hpp>
#include <iostream>
#include "ScreenSize.h"
#include "Camera.h"
#include "SceneTypes.h"
#include "SFML/Audio.hpp"
#include "SoundManager.h"

class Screen
{
public:
	Screen(sf::RenderWindow& t_window);

	void initialise(int optionSize, sf::String t_texts[]);

	void render(sf::RenderWindow& t_window);

	void update(sf::Window& t_window);
	int processGameEvents(sf::Event& t_event, sf::Window& t_window);

	virtual void firstButtonPress(int& t_lvlChecker);
	virtual void secondButtonPress(int& t_lvlChecker);
	virtual void thirdButtonPress(int& t_lvlChecker);
	virtual void fourthButtonPress(int& t_lvlChecker);

protected:
	int OPTION_COUNT{ 3 };

	sf::Texture buttonTexture;
	std::vector<sf::Sprite> buttonSprites;
	sf::Font font;
	std::vector <sf::Text> buttonText;

	float TOPOFFSET;
	float LEFTOFFSET;

	float SPACE_BETWEEN_BUTTON;
	float BUTTON_WIDTH;
	float BUTTON_HEIGHT;

	float BUTTON_TEXT_OFFSET;

	float BUTTON_OFFSET_Y;

	float titleFontSize;
	float fontSize;
	sf::Sprite bgSprite;
	sf::Texture bgTexture;

	sf::Vector2f SPRITE_SCALEDOWN;
	sf::Vector2f buttonScale{ 0.0f,0.0f };
	sf::Vector2f textScale{ 0.0f,0.0f };

	sf::Text titleText;

	Camera view;

	sf::Music menuMusic;
};

