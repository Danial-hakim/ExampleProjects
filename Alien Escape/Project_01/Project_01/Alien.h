#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include <time.h>
#include "tinyc2.h"
#include "Player.h"
#include "Maths.h"
#include "LevelLoader.h"
#include "Animation.h"
#include "AstartPathFinding.h"
#include <time.h>
#include <SoundManager.h>
#include "SceneTypes.h"


enum class alienState
{
	PATROLLING,
	CHASING,
	CONFUSE,
	HEARD_NOISE
};

class Alien
{
public:

	Alien(Player* t_player, Pathfinder* t_pathfinder);

	void init(sf::Vector2f position);
	void Draw(sf::RenderWindow& window , bool t_debugMode);
	void update(double dt);

	sf::Vector2f getPos();

	void heardNoise(sf::Vector2f t_noisePos);
	void heardNoise();
	void changeSpeed(float t_newSpeed);

	void updatePath();

	void clearLine();

private:

	void Patrolling();
	void goingTowardsSound();

	void updateVisionCone();
	sf::Vector2f rotatedVector(const sf::Vector2f& t_vector, float t_angle);
	void rotate(sf::Vector2f& t_vector, float t_angle);

	bool isPlayerSpotted();

	void setAnimationData();

	void updateMovement(sf::Vector2f nextTarget);
	void updateRotation();
	void checkIfFireIsPlaced();

	int nextCheckPoint();

	void updateLine();

	void updateaWayPoints();

	int getCell(sf::Vector2f targetPos);

	bool isPlayerNear();

	sf::Sprite alienSprite;
	sf::Texture alienTextureSheet;

	const sf::Vector2f SCALE_DOWN{ 0.1f, 0.1f };
	float speed = 1;
	bool hasReached;
	sf::Vector2f soundSpot;
	sf::Vector2f direction;

	sf::Vector2f conePointA;
	sf::Vector2f conePointB;
	sf::Vector2f conePointC;

	sf::Vector2f coneDirection{ 0.0f , -1.0f };
	float patrolConeLength{ 400.0f };
	float coneAngle{ 10.0f };
	float MAX_VISION_RANGE{ 20.0f};

	sf::VertexArray visionCone;

	const float MAX_SPEED{ 50.0f };
	const float MAX_FORCE{ 250.0f };
	double rotation{ 0.0f };
	const float ROTATION_OFFSET{ 90.0f };
	float tempRotation;

	std::vector<sf::Vector2f> wayPoints;
	int nextWayPoints = 0;
	sf::Vector2f velocity;

	c2Circle playerCircle;
	c2Poly visionConePoly;
	Player* player;

	alienState currentState;
	alienState lastState;

	Animation alienAnim;

	Pathfinder* m_pathfinder;

	const float SPRITESHEET_ROW{ 2.0f };
	const float SPRITESHEET_COL{ 12.0f };

	const int WIDTH{ 64 };
	const int HEIGHT{ 64 };

	const float ANIM_SPEED{ 200.0f };

	std::vector<sf::Vector2f> nextTarget;

	int mapCorners[4] = { 53 , 70 , 577 , 572 };

	std::vector<sf::Vertex> line;
	sf::Color lastLineColor;
	const int MAX_RBG_VALUE{ 255 };
	const int COLOR_MULTIPLIER{ 10 };

	int lastCell;
	int nextCell_ID;

	sf::Clock pathfinderClock;
	sf::Time pahtfinderDelay;

	sf::Clock growlClock;
	sf::Time growlDelay;

	int soundCell;
	int currentCell;
	int playerCell;
};
