#pragma once
#include "SFML/Graphics.hpp"
#include "ScreenSize.h"
#include "Maths.h"

class Camera
{
public:

	void updateCamera(sf::Vector2f t_playerPos, sf::Vector2f t_playerDirection);
	void updateCamera();
	void updateCamera(sf::Vector2f t_newPos);

	void transformCamera(sf::Vector2f t_newPos, bool t_checkWalls);
	void setCameraToCentreOfScreen();

	sf::Vector2f getCentre();
	sf::RenderWindow& getWindowRef();

	sf::Vector2f getBottomRightOfView();
	sf::Vector2f getTopLeftOfView();

	bool moving();

	sf::Vector2f getSize();

	Camera(sf::RenderWindow& t_window);
	void changeSpeed(int t_newSpeed);

	void scaleCameraUp();
	void scaleCameraDown();
	void scaleCameraToEditor();

	float cameraScaleDown = 1.0f;
	float cameraScaleUp = 3.0f;

	void changeSizeInGame(sf::Vector2f t_playerPos);

private:

	sf::Vector2f* playerPos;


	double cameraScaleEditor = 0.7;
	void changeCameraScale(float t_newScale);
	

	void wallChecks(sf::Vector2f& t_newCamCentre);

	void calculateNextCameraPos(sf::Vector2f t_playerPos, sf::Vector2f t_playerDirection);
	void moveCamera();

	int cameraSpeed;

	sf::Vector2f nextCameraPosition;
	sf::RenderWindow& windowScreen;
	sf::View camera;
	const sf::Vector2f CAMERA_OFFSET = sf::Vector2f(200,100);

};

