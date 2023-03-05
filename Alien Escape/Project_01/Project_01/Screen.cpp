#include "Screen.h"

Screen::Screen(sf::RenderWindow& t_window) : view(t_window)
{
}

void Screen::initialise(int optionSize, sf::String t_texts[])
{
	SPACE_BETWEEN_BUTTON = ScreenSize::s_height / 6 ;
	TOPOFFSET = ScreenSize::s_height / 3 ;
	LEFTOFFSET = ScreenSize::s_width / 3 ;

	BUTTON_WIDTH = ScreenSize::s_width / 3 ;
	BUTTON_HEIGHT = ScreenSize::s_height / 8 ;

	BUTTON_OFFSET_Y = ScreenSize::s_height / 3;

	BUTTON_TEXT_OFFSET = -(ScreenSize::s_width / 96);

	fontSize = ScreenSize::s_width / 22.5f;
	titleFontSize = ScreenSize::s_width / 10;

	OPTION_COUNT = optionSize;

	buttonSprites.resize(OPTION_COUNT);
	buttonText.resize(OPTION_COUNT);

	if (!font.loadFromFile("Assets/Fonts/alien.ttf"))
	{
		std::cout << "Unable to load font." << std::endl;
	}

	if (!buttonTexture.loadFromFile("./Assets/Sprites/button.png"))
	{
		std::cout << "Error with button png" << std::endl;
	}

	titleText.setFont(font);
	titleText.setString(t_texts[0]);
	titleText.setFillColor(sf::Color::Black);
	titleText.setCharacterSize(titleFontSize);
	titleText.setStyle(sf::Text::Bold);
	titleText.setStyle(sf::Text::Underlined);
	titleText.setOutlineColor(sf::Color::White);
	titleText.setOutlineThickness(4.0f);
	titleText.setPosition(LEFTOFFSET - titleText.getCharacterSize(), ScreenSize::s_height / 30);

	for (int i = 0; i < OPTION_COUNT; i++)
	{
		buttonSprites[i].setTexture(buttonTexture);
		buttonSprites[i].setColor(sf::Color(255, 200, 20, 255));
		buttonSprites[i].setOrigin(buttonTexture.getSize().x / 2.0f, buttonTexture.getSize().y / 2.0f);

		sf::Vector2u textureSize = buttonTexture.getSize();
		buttonSprites[i].setScale(BUTTON_WIDTH / textureSize.x, BUTTON_HEIGHT / textureSize.y);
		buttonSprites[i].setPosition(ScreenSize::s_width / 2.0f,BUTTON_OFFSET_Y + (SPACE_BETWEEN_BUTTON * i));

		buttonText[i].setFont(font);
		buttonText[i].setString(t_texts[i + 1]);
		buttonText[i].setFillColor(sf::Color::Black);
		buttonText[i].setCharacterSize(fontSize);
		buttonText[i].setStyle(sf::Text::Bold);
		buttonText[i].setOutlineColor(sf::Color::White);
		buttonText[i].setOutlineThickness(2.0f);
		buttonText[i].setOrigin(buttonText[i].getGlobalBounds().width / 2.0f, buttonText[i].getGlobalBounds().height);
		sf::FloatRect textSize = buttonText[i].getGlobalBounds();
		buttonText[i].setPosition(buttonSprites[i].getPosition() + sf::Vector2f(0, BUTTON_TEXT_OFFSET));
	}

	if (!bgTexture.loadFromFile("./Assets/Sprites/background.jpeg"))
	{
		std::cout << "Error with background image" << std::endl;
	}

	SPRITE_SCALEDOWN = { ScreenSize::s_width / bgTexture.getSize().x , ScreenSize::s_height / bgTexture.getSize().y };

	bgSprite.setTexture(bgTexture);
	bgSprite.setScale(SPRITE_SCALEDOWN);

	buttonScale = buttonSprites[0].getScale();
	textScale = buttonText[0].getScale();

	view.transformCamera(sf::Vector2f(ScreenSize::s_width / 2.0f, ScreenSize::s_height / 2.0f), false);
	view.scaleCameraDown();
}

void Screen::render(sf::RenderWindow& t_window)
{
	view.updateCamera();
	t_window.draw(bgSprite);
	t_window.draw(titleText);
	for (int i = 0; i < OPTION_COUNT; i++)
	{
		t_window.draw(buttonSprites[i]);
		t_window.draw(buttonText[i]);
	}
}

void Screen::update(sf::Window& t_window)
{
	sf::Vector2i mouseLocation;
	mouseLocation = sf::Mouse::getPosition(t_window);
	for (int i = 0; i < OPTION_COUNT; i++)
	{
		if (buttonSprites[i].getGlobalBounds().contains(mouseLocation.x, mouseLocation.y))
		{
			buttonText[i].setFillColor(sf::Color::Red);
		}
		else
		{
			buttonText[i].setFillColor(sf::Color::Black);
		}
	}
}

int Screen::processGameEvents(sf::Event& t_event, sf::Window& t_window)
{
	int levelChecker = -1;
	static bool lock_click;
	sf::Vector2i mouseLocation;

	mouseLocation = sf::Mouse::getPosition(t_window);
	for (int i = 0; i < OPTION_COUNT; i++)
	{
		if (t_event.type == sf::Event::MouseButtonPressed && buttonSprites[i].getGlobalBounds().contains(mouseLocation.x, mouseLocation.y))
		{
			buttonSprites[i].setScale(buttonSprites[i].getScale() / 1.25f);
			buttonText[i].setScale(buttonText[i].getScale() / 1.25f);

			if (t_event.mouseButton.button == sf::Mouse::Left && lock_click != true)
			{
				//std::cout << "left mouse pressed" << std::endl;
				lock_click = true;
			}
		}
		if (t_event.type == sf::Event::MouseButtonReleased)
		{
			if (buttonSprites[i].getScale().x < buttonScale.x && buttonSprites[i].getScale().y < buttonScale.y)
			{
				if (t_event.mouseButton.button == sf::Mouse::Left)
				{
					//std::cout << "release" << std::endl;
					lock_click = false; // when mouse left button is released 
					SoundManager::causeSound(NoiseTypes::MenuSelect);
					if (buttonSprites[i].getGlobalBounds().contains(mouseLocation.x, mouseLocation.y))
					{
						if (i == 0)
						{
							firstButtonPress(levelChecker);
						}

						else if (i == 1)
						{
							secondButtonPress(levelChecker);
						}

						else if (i == 2)
						{
							thirdButtonPress(levelChecker);
						}

						else if (i == 3)
						{
							fourthButtonPress(levelChecker);
						}
					}
				}
			}
			if (buttonSprites[i].getScale().x < buttonScale.x && buttonSprites[i].getScale().y < buttonScale.y)
			{
				buttonSprites[i].setScale(buttonScale);
				buttonText[i].setScale(textScale);
			}
		}
	}

	return levelChecker;
}

void Screen::firstButtonPress(int& t_lvlChecker)
{
}

void Screen::secondButtonPress(int& t_lvlChecker)
{
}

void Screen::thirdButtonPress(int& t_lvlChecker)
{
}

void Screen::fourthButtonPress(int& t_lvlChecker)
{
}
