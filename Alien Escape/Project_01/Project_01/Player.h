#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include "Maths.h"
#include "Inventory.h"
#include "Camera.h"
#include "Animation.h"
#include "playerMission.h"

enum class STATE
{
	CROUCHING,
	RUNNING,
	WALKING_UP,
	WALKING_DOWN,
	WALKING_LEFT,
	WALKING_RIGHT,
	IDLE
};

class Player
{
public:

	Player(ScannerObserver* t_observer, sf::RenderWindow& t_window, Grid* t_grid);
	void init(sf::Font* t_font, sf::Vector2f position);
	void init(sf::Vector2f position);
	void Draw(sf::RenderWindow& t_window);

	void updateCamera();

	void Update(bool t_nearWall , double dt);
	Camera playersView;

	void checkInput(bool t_nearWall);

	void checkDelayedInput(sf::Event& t_event);

	sf::Vector2f getPos();


	float getRadius();
	sf::Vector2f getDirection();
	sf::FloatRect getGlobalBounds();

	void updateMovement();

	Inventory* getInventory();

	void setAnimationData();
	
private:
	playerMission missionTracker;

	Direction playersDirection;

	Inventory playersInventory;

	sf::Sprite playerSprite;
	sf::Texture playerTextureSheet;
	
	const sf::Vector2f SCALE_DOWN{ 0.5f,0.5f };
	sf::Vector2f direction;

	const float RUNNINGSPEED{ 3 };
	const float CROUCHINGSPEED{ 0.2 };
	const float WALKINGSPEED{ 1 };
	float speed{ 0.0f };
	const float radiantOffSet{ 180.0f };
	sf::Vector2f lastPos;
	sf::Vector2f offset;
	STATE state{ STATE::WALKING_UP };
	
	float rotationOffSet{ 0.0f };

	Animation walkUpAnim;
	Animation walkDownAnim;
	Animation walkLeftAnim;
	Animation walkRightAnim;
	Animation idleAnim;

	const float ANIM_SPEED{ 200.0f };
	const float IDLE_SPEED{ 2000.0f };
	const float SPRITESHEET_ROW{ 8.0f };
	const float SPRITESHEET_COL{ 18.0f }; 
	const int WIDTH{ 82 };
	const int HEIGHT{ 80 };
};
