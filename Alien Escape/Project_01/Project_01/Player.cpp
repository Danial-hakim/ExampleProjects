#include "Player.h"

Player::Player(ScannerObserver* t_observer, sf::RenderWindow& t_window, Grid* t_grid):
playersInventory(t_observer, &playersView), playersView(t_window),
walkUpAnim(playerSprite),
walkDownAnim(playerSprite),
walkLeftAnim(playerSprite),
walkRightAnim(playerSprite),
idleAnim(playerSprite),
missionTracker(t_observer, &playersView, t_grid)
{

}

void Player::init(sf::Font* t_font, sf::Vector2f position)
{
	if (!playerTextureSheet.loadFromFile("./Assets/Sprites/FinalSprites/player.png"))
	{
		std::cout << "spritesheet error - player" << std::endl;
	}

	init(position);

	playersInventory.getFont(t_font);
	missionTracker.init(t_font);
}

void Player::init(sf::Vector2f position)
{
	position = sf::Vector2f(position.x * cellWidth, position.y * cellHeight);
	position.x += cellWidth / 2.0f;
	position.y += cellHeight / 2.0f;
	playerSprite.setTexture(playerTextureSheet);
	playerSprite.setScale(SCALE_DOWN);
	offset = sf::Vector2f{ playerTextureSheet.getSize().x / SPRITESHEET_COL, playerTextureSheet.getSize().y / SPRITESHEET_ROW };
	playerSprite.setOrigin(offset);
	playerSprite.setPosition(position);
	lastPos = playerSprite.getPosition();
	state = STATE::IDLE;
	setAnimationData();

	playerSprite.setTextureRect(sf::IntRect(0, 0, WIDTH, HEIGHT));

	playersView.scaleCameraUp();
	playersView.transformCamera(playerSprite.getPosition(), true);

	playersInventory.initialise();
	missionTracker.restart();
}

void Player::Update(bool nearWall ,double dt)
{
	switch (state)
	{
	case STATE::CROUCHING:
		break;
	case STATE::RUNNING:
		break;
	case STATE::WALKING_UP:
		walkUpAnim.update(dt, true);
		playersDirection = Direction::Up;
		break;
	case STATE::WALKING_DOWN:
		walkDownAnim.update(dt, true);
		playersDirection = Direction::Down;
		break;
	case STATE::WALKING_LEFT:
		walkLeftAnim.update(dt, true);
		playersDirection = Direction::Left;
		break;
	case STATE::WALKING_RIGHT:
		walkRightAnim.update(dt, true);
		playersDirection = Direction::Right;
		break;
	case STATE::IDLE:
		idleAnim.update(dt, true);
		break;
	default: 
		break;
	}

	if (!playersInventory.inventoryIsOpen())
	{
		checkInput(nearWall);
		updateMovement();
	}

	playersInventory.update();
	playersInventory.virtualUpdate();
	missionTracker.update(playerSprite.getPosition());
	playersView.updateCamera(playerSprite.getPosition(), direction);
}

void Player::checkInput(bool t_nearWall)
{
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::LShift))
	{
		speed = RUNNINGSPEED;
	}
	else if (sf::Keyboard::isKeyPressed(sf::Keyboard::LControl))
	{
		speed = CROUCHINGSPEED;
	}
	else
	{
		speed = WALKINGSPEED;
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up) || (sf::Keyboard::isKeyPressed(sf::Keyboard::W)))
	{
		direction.y = -1;
		state = STATE::WALKING_UP;
		rotationOffSet = -90.0f;
	}
	else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down) || (sf::Keyboard::isKeyPressed(sf::Keyboard::S)))
	{
		direction.y = 1;
		state = STATE::WALKING_DOWN;
		rotationOffSet = 90.0f;
	}
	else
	{
		direction.y = 0;
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Left) || (sf::Keyboard::isKeyPressed(sf::Keyboard::A)))
	{
		direction.x = -1;
		state = STATE::WALKING_LEFT;
		rotationOffSet = 0.0f;
	}
	else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Right) || (sf::Keyboard::isKeyPressed(sf::Keyboard::D)))
	{
		direction.x = 1;
		state = STATE::WALKING_RIGHT;
		rotationOffSet = 180.0f;
	}
	else
	{
		direction.x = 0;
	}
	if (playerSprite.getPosition() == lastPos)
	{
		state = STATE::IDLE;
	}
	if (t_nearWall)
	{
		playerSprite.setPosition(lastPos);
	}
	lastPos = playerSprite.getPosition();
}

void Player::checkDelayedInput(sf::Event& t_event)
{
	playersInventory.handleKeyInventoryInput(t_event, playerSprite.getPosition(), playersDirection);
}

sf::Vector2f Player::getPos()
{
	return playerSprite.getPosition();
}

float Player::getRadius()
{
	float radius;
	if (offset.x < offset.y)
	{
		radius = offset.y / 2.5f;
	}
	else
	{
		radius = offset.x / 2.5f;
	}
	return radius;
}

sf::Vector2f Player::getDirection()
{
	return direction;
}

sf::FloatRect Player::getGlobalBounds()
{
	return playerSprite.getGlobalBounds();
}

void Player::updateMovement()
{
	float radiant = atan2f(direction.y, direction.x);
	float degree = radiant * (radiantOffSet / M_PI) + radiantOffSet;

	if (direction.x != 0 || direction.y != 0)
	{
		playerSprite.setRotation(degree + rotationOffSet);
	}
	direction = Maths::unitVector(direction);
	playersView.changeSpeed(speed);
	playerSprite.move(direction * speed);
}

Inventory* Player::getInventory()
{
	return &playersInventory;
}

void Player::setAnimationData()
{
	for (int i = 0; i < 9; i++)
	{
		walkUpAnim.addFrame({ sf::IntRect(i * WIDTH , 0 , WIDTH, HEIGHT), ANIM_SPEED });
		walkDownAnim.addFrame({ sf::IntRect(i * WIDTH , HEIGHT * 2 , WIDTH, HEIGHT), ANIM_SPEED });
		walkLeftAnim.addFrame({ sf::IntRect(i * WIDTH , HEIGHT  , WIDTH, HEIGHT), ANIM_SPEED });
		walkRightAnim.addFrame({ sf::IntRect(i * WIDTH , HEIGHT * 3 , WIDTH, HEIGHT), ANIM_SPEED });
	}

	//temp idle animation 
	idleAnim.addFrame({ sf::IntRect(0 , 0 , WIDTH, HEIGHT), IDLE_SPEED });
	idleAnim.addFrame({ sf::IntRect( 0 , HEIGHT , WIDTH, HEIGHT),  IDLE_SPEED });
	idleAnim.addFrame({ sf::IntRect(0 , 2 * HEIGHT , WIDTH, HEIGHT),  IDLE_SPEED });
	idleAnim.addFrame({ sf::IntRect( 0 , 3 * HEIGHT , WIDTH, HEIGHT), IDLE_SPEED });
	idleAnim.addFrame({ sf::IntRect(0 , 2 * HEIGHT , WIDTH, HEIGHT),  IDLE_SPEED });
}

void Player::Draw(sf::RenderWindow& t_window)
{
	missionTracker.renderEscape(t_window);
	t_window.draw(playerSprite);
	playersInventory.render(t_window);

	if (playersInventory.inventoryIsOpen())
	{
		missionTracker.render(t_window);
	}
}

void Player::updateCamera()
{
	playersView.updateCamera();
}
