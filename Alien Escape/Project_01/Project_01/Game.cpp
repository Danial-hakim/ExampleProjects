
#define TINYC2_IMPL

#include "Game.h"
#include "Alien.h"
#include <iostream>

// Updates per milliseconds
static double const MS_PER_UPDATE = 10.0;

////////////////////////////////////////////////////////////
Game::Game()
	: m_window(sf::VideoMode(ScreenSize::s_width, ScreenSize::s_height, 32), "SFML Playground", sf::Style::Default),
	gameplay(m_window), editor(m_window, m_level), menu(m_window), inGameMenu(m_window), levelMenu(m_window),winScreen(m_window),gameOverScreen(m_window),tutorial(m_window)
{
	if (!gameFont.loadFromFile("Assets/Fonts/arial.ttf"))
	{
		std::cout << "Unable to load font." << std::endl;
	}

	if (!gameMusic.openFromFile("Assets/Sound/music.wav"))
	{
		std::cout << "error with game music" << std::endl;
	}
	if (!menuMusic.openFromFile("Assets/Sound/backgroundMusic.wav"))
	{
		std::cout << "error with menu music" << std::endl;
	}
	if (!loseMusic.openFromFile("Assets/Sound/alienRoar.wav"))
	{
		std::cout << "error with lose music" << std::endl;
	}
	menuMusic.play();
	menuMusic.setLoop(true);
	gameMusic.setVolume(15);
	gameMusic.setLoop(true);
	menu.init();
	inGameMenu.init();
	levelMenu.init();
	tutorial.init();
	winScreen.init();
	gameOverScreen.init();
}

////////////////////////////////////////////////////////////
void Game::run()
{
	sf::Clock clock;
	sf::Time timeSinceLastUpdate = sf::Time::Zero;
	sf::Time timePerFrame = sf::seconds(1.f / 60.f);
	sf::Int32 lag = 0;

	while (m_window.isOpen())
	{
		processEvents();

		timeSinceLastUpdate += clock.restart();

		while (timeSinceLastUpdate > timePerFrame)
		{
			timeSinceLastUpdate -= timePerFrame;
			processEvents();
			update(timePerFrame.asMicroseconds() / 500);
		}

		render();
	}
}

////////////////////////////////////////////////////////////
void Game::processEvents()
{
	sf::Event event;
	while (m_window.pollEvent(event))
	{
		if (event.type == sf::Event::Closed)
		{
			m_window.close();
		}

		processGameEvents(event);
	}
}

////////////////////////////////////////////////////////////
void Game::processGameEvents(sf::Event& event)
{
	int levelLoaderChecker = -1;

	// check if the event is a a mouse button release
	if (sf::Event::KeyPressed == event.type || sf::Event::KeyReleased == event.type
		|| event.type == sf::Event::MouseButtonPressed || event.type == sf::Event::MouseMoved
		|| event.type == sf::Event::MouseButtonReleased)
	{
		switch (event.key.code)
		{
		case sf::Keyboard::Escape:

			switch (SceneTypes::currentScene)
			{
			case Scenes::MAIN_GAME:
			case Scenes::LEVEL_EDITOR:
				case Scenes::TUTORIAL:

				SceneTypes::updateScene(Scenes::IN_GAME_MENU);
				break;

			case Scenes::MENU:

				m_window.close();
				break;
			}

			break;
		default:
			break;
		}

		switch (SceneTypes::currentScene)
		{
		case Scenes::MAIN_GAME:
			gameplay.processGameEvents(event);
			break;
		case Scenes::MENU:
			levelLoaderChecker = menu.processGameEvents(event, m_window);
			break;
		case Scenes::GAME_OVER:
			levelLoaderChecker = gameOverScreen.processGameEvents(event, m_window);
			break;
		case Scenes::LEVEL_EDITOR:
			editor.processGameEvents(event);
			break;
		case Scenes::IN_GAME_MENU:
			inGameMenu.processGameEvents(event, m_window);
			break;
		case Scenes::LEVEL_SELECT:
			levelLoaderChecker = levelMenu.processGameEvents(event, m_window);
			break;
		case Scenes::TUTORIAL:
			tutorial.processGameEvents(event, m_window);
			break;
		case Scenes::WIN_SCREEN:
			levelLoaderChecker = winScreen.processGameEvents(event, m_window);
			break;
		default:
			break;
		}

		if (levelLoaderChecker != -1)
		{
			loadLevel(levelLoaderChecker);
		}
	}
}

void Game::loadLevel(int t_level)
{
	try// will generate exception if level loading fails
	{
		if (t_level == 1)
		{
			LevelLoader::load(m_level, "./levels/level1.yaml");
			level = 0;
		}
		else
		{
			LevelLoader::load(m_level, "./levels/levelTest.yaml");
			level = 1;
		}

		gameplay.init(gameFont, &m_level, level);
		editor.init(&m_level);

	}
	catch (std::exception& e)
	{
		std::cout << "Leveling loading failure" << std::endl;
		std::cout << e.what() << std::endl;
		throw e;
	}
}

////////////////////////////////////////////////////////////
void Game::update(double dt)
{
	SoundManager::update();
	if (ScreenSize::s_width != m_window.getSize().x)
	{
		ScreenSize::updateWidthAndHeight(m_window.getSize());
		//std::cout << m_window.getSize().x << std::endl;
		//std::cout << ScreenSize::s_width << std::endl;
		//std::cout << "==================" << std::endl;
		menu.init();
		inGameMenu.init();
		levelMenu.init();
		winScreen.init();
		gameOverScreen.init();
		tutorial.init();
	}
	
	switch (SceneTypes::currentScene)
	{
	case Scenes::MAIN_GAME:
		if (loseMusic.getStatus() == loseMusic.Playing)
		{
			loseMusic.stop();
		}
		if (menuMusic.getStatus() == menuMusic.Playing)
		{
			menuMusic.pause();
		}
		if (gameMusic.getStatus() != gameMusic.Playing)
		{
			gameMusic.play();
		}
		gameplay.update(dt);
		break;
	case Scenes::MENU:
		if (loseMusic.getStatus() == loseMusic.Playing)
		{
			loseMusic.stop();
		}
		if (menuMusic.getStatus() != menuMusic.Playing)
		{
			menuMusic.play();
		}
		if (gameMusic.getStatus() == gameMusic.Playing)
		{
			gameMusic.pause();
		}
		menu.update(m_window);
		break;
	case Scenes::GAME_OVER:
		gameOverScreen.update(m_window);
		if (gameMusic.getStatus() == gameMusic.Playing)
		{
			gameMusic.pause();
		}
		if (loseMusic.getStatus() != loseMusic.Playing)
		{
			loseMusic.play();
		}
		break;
	case Scenes::LEVEL_EDITOR:
		if (loseMusic.getStatus() == loseMusic.Playing)
		{
			loseMusic.stop();
		}
		if (menuMusic.getStatus() != menuMusic.Playing)
		{
			menuMusic.play();
		}
		if (gameMusic.getStatus() == gameMusic.Playing)
		{
			gameMusic.pause();
		}
		editor.update(dt);
		break;
	case Scenes::IN_GAME_MENU:
		inGameMenu.update(m_window);
		break;
	case Scenes::LEVEL_SELECT:
		if (loseMusic.getStatus() == loseMusic.Playing)
		{
			loseMusic.stop();
		}
		if (menuMusic.getStatus() != menuMusic.Playing)
		{
			menuMusic.play();
		}
		if (gameMusic.getStatus() == gameMusic.Playing)
		{
			gameMusic.pause();
		}
		levelMenu.update(m_window);
		break;
	case Scenes::WIN_SCREEN:
		winScreen.update(m_window);
		break;
	case Scenes::TUTORIAL:
		tutorial.update(m_window);
		break;
	default:
		break;
	}
}

////////////////////////////////////////////////////////////
void Game::render()
{
	m_window.clear(sf::Color(0, 0, 0, 0));

	switch (SceneTypes::currentScene)
	{
	case Scenes::MAIN_GAME:
		gameplay.render(m_window);
		break;
	case Scenes::MENU:
		menu.render(m_window);
		break;
	case Scenes::GAME_OVER:
		gameOverScreen.render(m_window);
		break;
	case Scenes::LEVEL_EDITOR:
		editor.render(m_window);
		break;
	case Scenes::IN_GAME_MENU:
		inGameMenu.render(m_window);
		break;
	case Scenes::LEVEL_SELECT:
		levelMenu.render(m_window);
		break;
	case Scenes::TUTORIAL:
		tutorial.render(m_window);
		break;
	case Scenes::WIN_SCREEN:
		winScreen.render(m_window);
		break;
	default:
		break;
	}

	m_window.display();
}





