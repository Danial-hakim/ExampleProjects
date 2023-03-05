#include "Alien.h"

Alien::Alien(Player* t_player, Pathfinder* t_pathfinder): m_pathfinder(t_pathfinder),
alienAnim(alienSprite)
{
	player = t_player;
	alienSprite.setScale(alienSprite.getScale().x / 1.75f, alienSprite.getScale().y / 1.75f);
}

void Alien::init(sf::Vector2f position)
{
	wayPoints.clear();
	line.clear();
	if (!alienTextureSheet.loadFromFile("./Assets/Sprites/FinalSprites/Alien.png"))
	{
		std::cout << "Error with alienSprite" << std::endl;
	}

	position = sf::Vector2f(position.x * cellWidth, position.y * cellHeight);
	position.x += cellWidth / 2.0f;
	position.y += cellHeight / 2.0f;

	alienSprite.setTexture(alienTextureSheet);
	alienSprite.setOrigin(alienTextureSheet.getSize().x / SPRITESHEET_COL, alienTextureSheet.getSize().y / SPRITESHEET_ROW);
	alienSprite.setPosition(position);

	currentState = alienState::PATROLLING;

	setAnimationData();

	currentCell = getCell(alienSprite.getPosition());

	updatePath();
	
	for (int i = 0; i < nextTarget.size(); i++)
	{
		wayPoints.push_back(nextTarget.at(i));
	}
	nextWayPoints = 0;

	updateLine();

	srand(time(NULL));
	alienAnim.update(1, true);
	pahtfinderDelay = sf::seconds(1.5f);
	growlDelay = sf::seconds(4.0f);
}

void Alien::Draw(sf::RenderWindow& t_window, bool t_debugMode)
{
	t_window.draw(alienSprite);

	if (t_debugMode)
	{
		t_window.draw(visionCone);

		if (line.size() > 1)
		{
			t_window.draw(&line[0], line.size(), sf::LinesStrip);
		}
	}
}

void Alien::update(double dt)
{
	alienAnim.update(dt, true);

	switch (currentState)
	{
	case alienState::PATROLLING:
		if (nextWayPoints == nextTarget.size() - 1)
		{
			currentCell = getCell(alienSprite.getPosition());

			wayPoints.clear();

			line.clear();

			updatePath();

			updateaWayPoints();
			updateLine();
		}
		break;
	case alienState::CHASING:
		if (pathfinderClock.getElapsedTime() > pahtfinderDelay)
		{
			currentCell = getCell(alienSprite.getPosition());

			playerCell = getCell(player->getPos());

			wayPoints.clear();

			line.clear();

			nextTarget = m_pathfinder->setPath(currentCell, playerCell);

			updateaWayPoints();
			updateLine();

			pathfinderClock.restart();
		}
		break;
	case alienState::CONFUSE:

		currentState = alienState::PATROLLING;
		break;

	case alienState::HEARD_NOISE:

		if(lastState != alienState::HEARD_NOISE || nextWayPoints == nextTarget.size() - 1)
		{
			currentCell = getCell(alienSprite.getPosition());

			soundCell = getCell(soundSpot);

			wayPoints.clear();

			line.clear();

			nextTarget = m_pathfinder->setPath(currentCell, soundCell);

			updateaWayPoints();
			updateLine();
		}
		goingTowardsSound();
	default:
		break;

	}
	
	updateVisionCone();
	if (isPlayerSpotted()) 
	{
		currentState = alienState::CHASING;
	}
	else if(!isPlayerSpotted() && currentState == alienState::CHASING)
	{
		currentState = alienState::PATROLLING;
	}

	if (isPlayerNear())
	{
		if (growlClock.getElapsedTime() > growlDelay)
		{
			SoundManager::causeSound(NoiseTypes::AlienGrowl);
			growlClock.restart();
		}
	}
	checkIfFireIsPlaced();
	updateMovement((wayPoints[nextWayPoints]));
	Patrolling();
	lastState = currentState;

	if (alienSprite.getGlobalBounds().intersects(player->getGlobalBounds()))
	{
		SceneTypes::updateScene(Scenes::GAME_OVER);
	}
}

void Alien::updateVisionCone()
{
	visionCone = sf::VertexArray(sf::Triangles, 3);

	conePointA = alienSprite.getPosition();
	conePointB = alienSprite.getPosition() + patrolConeLength * rotatedVector(coneDirection, -coneAngle + alienSprite.getRotation());
	conePointC = alienSprite.getPosition() + patrolConeLength * rotatedVector(coneDirection, coneAngle + alienSprite.getRotation());

	visionCone[0].position = conePointA;
	visionCone[1].position = conePointB;
	visionCone[2].position = conePointC;

	visionCone[0].color = sf::Color::Red;
	visionCone[1].color = sf::Color::Transparent;
	visionCone[2].color = sf::Color::Transparent;
}

sf::Vector2f Alien::rotatedVector(const sf::Vector2f& t_vector, float t_angle)
{
	sf::Vector2f copy = t_vector;
	rotate(copy, t_angle);
	return copy;
}

void Alien::rotate(sf::Vector2f& t_vector, float t_angle)
{
	float cos = std::cos(t_angle* M_PI/180);
	float sin = std::sin(t_angle* M_PI /180);

	t_vector = sf::Vector2f(
		cos * t_vector.x - sin * t_vector.y,
		sin * t_vector.x + cos * t_vector.y);
}
sf::Vector2f Alien::getPos()
{
	return alienSprite.getPosition();
}

void Alien::heardNoise(sf::Vector2f t_noisePos)
{
	soundSpot = t_noisePos;
	currentState = alienState::HEARD_NOISE;
}

void Alien::heardNoise()
{
	soundSpot = player->getPos();
	currentState = alienState::HEARD_NOISE;
}

void Alien::changeSpeed(float t_newSpeed)
{
	speed = t_newSpeed;
}

void Alien::updatePath()
{
	nextTarget = m_pathfinder->setPath(currentCell, nextCheckPoint());
}

void Alien::clearLine()
{
	line.clear();
}

bool Alien::isPlayerSpotted()
{
	playerCircle.r = 15.0f;

	playerCircle.p.x = player->getPos().x;
	playerCircle.p.y = player->getPos().y;

	visionConePoly.count = 3;

	visionConePoly.verts[0] = c2v{ conePointA.x,conePointA.y };
	visionConePoly.verts[1] = c2v{ conePointB.x,conePointB.y };
	visionConePoly.verts[2] = c2v{ conePointC.x,conePointC.y };

	int result = c2CircletoPoly(playerCircle, &visionConePoly, NULL);
	return result != 0;
}

void Alien::Patrolling()
{
	float result = Maths::distance(alienSprite.getPosition(), wayPoints[nextWayPoints]);

	if (result < 2)
	{
		if (nextWayPoints >= wayPoints.size() - 1)
		{
			nextWayPoints = 0;
		}
		else
		{
			nextWayPoints++;
		}
	}
}

void Alien::goingTowardsSound()
{
	int result = Maths::distance(alienSprite.getPosition(), soundSpot);

	if (result < 80)
	{
		currentState = alienState::PATROLLING;
	}
}

void Alien::setAnimationData()
{
	for (int i = 0; i < 6; i++)
	{
		alienAnim.addFrame({ sf::IntRect(i * WIDTH, 0 , WIDTH, HEIGHT), ANIM_SPEED });
	}
}

void Alien::updateMovement(sf::Vector2f nextTarget)
{
	if (alienSprite.getPosition().x < nextTarget.x - 1.0f)
	{
		direction.x = 1;
	}
	else if (alienSprite.getPosition().x > nextTarget.x  + 1.0f)
	{
		direction.x = -1;
	}
	else
	{
		direction.x = 0;
	}

	if (alienSprite.getPosition().y < nextTarget.y - 1.0f)
	{
		direction.y = 1;
	}
	else if (alienSprite.getPosition().y > nextTarget.y + 1.0f)
	{
		direction.y = -1;
	}
	else
	{
		direction.y = 0;
	}

	updateRotation();
}

void Alien::updateRotation()
{
	float radiant = atan2f(direction.y, direction.x);
	float degree = radiant * (180.0f / M_PI) + 90.0f;

	if (direction.x != 0 || direction.y != 0) // if both x and y is 0 , there is no need to do this 
	{
		alienSprite.setRotation(degree);
	}

	direction = Maths::unitVector(direction);
	alienSprite.move(direction * speed);
}

void Alien::checkIfFireIsPlaced()
{
	if (m_pathfinder->getGridRef()->smokeChange)
	{
		m_pathfinder->getGridRef()->smokeChange = false;

		currentCell = getCell(alienSprite.getPosition());

		nextTarget = m_pathfinder->setPath(currentCell, Grid::getCellID(nextTarget[nextTarget.size() - 1]));

		wayPoints.clear();

		line.clear();

		for (int i = 0; i < nextTarget.size(); i++)
		{
			wayPoints.push_back(nextTarget.at(i));
		}
		nextWayPoints = 0;

		updateLine();
	}
}


int Alien::nextCheckPoint()
{
	int randomChoices = rand() % 4;
	
	if (randomChoices == 0)
	{
		nextCell_ID = mapCorners[0];
	}
	else if (randomChoices == 1)
	{
		nextCell_ID = mapCorners[1];
	}
	else if (randomChoices == 2)
	{
		nextCell_ID = mapCorners[2];
	}
	else 
	{
		if (lastCell == mapCorners[0])
		{
			nextCell_ID = mapCorners[2];
		}
		else
		{
			nextCell_ID = mapCorners[3];
		}
	}

	lastCell = nextCell_ID;
	return nextCell_ID;
}

void Alien::updateLine()
{
	for (int i = 0; i < nextTarget.size() - 1; i++)
	{
		line.push_back(sf::Vertex(nextTarget.at(i)));
		if (i % 5 == 0)
		{
			line.at(i).color = sf::Color(MAX_RBG_VALUE, MAX_RBG_VALUE - (i * COLOR_MULTIPLIER), MAX_RBG_VALUE - (i * COLOR_MULTIPLIER));
			lastLineColor = line.at(i).color;
		}
		else
		{
			line.at(i).color = lastLineColor;
		}
	}
}

void Alien::updateaWayPoints()
{
	for (int i = 0; i < nextTarget.size(); i++)
	{
		wayPoints.push_back(nextTarget.at(i));
	}
	nextWayPoints = 0;
}

int Alien::getCell(sf::Vector2f targetPos)
{
	int targetRow = round((targetPos.x + Grid::getCellOrigin().x) / cellWidth);
	int targetCol = round((targetPos.y + Grid::getCellOrigin().y) / cellHeight);
	int targetCell = ((targetRow - 1) * ScreenSize::COLUMNS) + (targetCol - 1);

	return targetCell;
}

bool Alien::isPlayerNear()
{
	int distance = Maths::distance(alienSprite.getPosition(), player->getPos());

	if (distance < 250)
	{
		return true;
	}
	else
	{
		return false;
	}
}

