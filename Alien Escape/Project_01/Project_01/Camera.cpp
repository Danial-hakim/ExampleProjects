#include "Camera.h"

void Camera::updateCamera(sf::Vector2f t_playerPos, sf::Vector2f t_playerDirection)
{
	windowScreen.setView(camera);
	calculateNextCameraPos(t_playerPos, t_playerDirection);
	moveCamera();
}

void Camera::updateCamera(sf::Vector2f t_newPos)
{
	windowScreen.setView(camera);
	nextCameraPosition = t_newPos;
	moveCamera();
}

void Camera::updateCamera()
{
	windowScreen.setView(camera);
}

void Camera::transformCamera(sf::Vector2f t_newPos, bool t_checkWalls)
{
	if (t_checkWalls)
	{
		wallChecks(t_newPos);
	}

	camera.setCenter(t_newPos);
	nextCameraPosition = camera.getCenter();
}

void Camera::setCameraToCentreOfScreen()
{
	camera.setCenter(ScreenSize::s_width / 2.0f, ScreenSize::s_height / 2.0f);
	nextCameraPosition = camera.getCenter();
}



sf::Vector2f Camera::getCentre()
{
	return camera.getCenter();
}

sf::Vector2f Camera::getBottomRightOfView()
{
	return camera.getCenter() + (camera.getSize() / 2.0f);
}

sf::Vector2f Camera::getTopLeftOfView()
{
	return camera.getCenter() - (camera.getSize() / 2.0f);
}

bool Camera::moving()
{
	return nextCameraPosition != camera.getCenter();
}

sf::Vector2f Camera::getSize()
{
	return camera.getSize();
}

Camera::Camera(sf::RenderWindow& t_window) : windowScreen(t_window),
camera(sf::FloatRect(0, 0, ScreenSize::s_width / cameraScaleDown, ScreenSize::s_height / cameraScaleDown))
{
	nextCameraPosition = camera.getCenter();
	cameraSpeed = 5;
}

void Camera::changeSpeed(int t_newSpeed)
{
	if (t_newSpeed < 1)
	{
		t_newSpeed = 1;
	}
	cameraSpeed = t_newSpeed;
}

void Camera::scaleCameraUp()
{
	changeCameraScale(cameraScaleUp);
}

  void Camera::scaleCameraDown()
{
	  changeCameraScale(cameraScaleDown);
}

  void Camera::scaleCameraToEditor()
  {
	  changeCameraScale(cameraScaleEditor);
  }

void Camera::changeCameraScale(float t_newScale)
{
	camera.setSize(ScreenSize::s_width / t_newScale, ScreenSize::s_height / t_newScale);
	nextCameraPosition = camera.getCenter() / t_newScale;
}

void Camera::changeSizeInGame(sf::Vector2f t_playerPos)
{
	if (camera.getSize().x == ScreenSize::s_width / cameraScaleDown)
	{
		changeCameraScale(cameraScaleUp);
		transformCamera(t_playerPos, true);
	}

	else
	{
		changeCameraScale(cameraScaleDown);
		setCameraToCentreOfScreen();
	}
}

void Camera::wallChecks(sf::Vector2f& t_newCamCentre)
{
	if (t_newCamCentre.x - (camera.getSize().x / 2.0f) < 0)
	{
		t_newCamCentre.x = (camera.getSize().x / 2.0f);
	}

	else if (t_newCamCentre.x + (camera.getSize().x / 2.0f) > ScreenSize::s_width)
	{
		t_newCamCentre.x = ScreenSize::s_width - (camera.getSize().x / 2.0f);
	}

	if (t_newCamCentre.y - (camera.getSize().y / 2.0f) < 0)
	{
		t_newCamCentre.y = (camera.getSize().y / 2.0f);
	}

	else if (t_newCamCentre.y + (camera.getSize().y / 2.0f) > ScreenSize::s_height)
	{
		t_newCamCentre.y = ScreenSize::s_height - (camera.getSize().y / 2.0f);
	}
}

sf::RenderWindow& Camera::getWindowRef()
{
	return windowScreen;
}

void Camera::calculateNextCameraPos(sf::Vector2f t_playerPos, sf::Vector2f t_playerDirection)
{
	sf::Vector2f newCamCentre = t_playerPos;

	newCamCentre += sf::Vector2f(t_playerDirection.x * CAMERA_OFFSET.x, t_playerDirection.y * CAMERA_OFFSET.y);


	wallChecks(newCamCentre);

	if (t_playerDirection.x != 0 || t_playerDirection.y != 0)
	{
		nextCameraPosition = newCamCentre;
	}
	
}

void Camera::moveCamera()
{
	sf::Vector2f camDirection = Maths::unitVector(nextCameraPosition - camera.getCenter());
	camDirection = sf::Vector2f(camDirection.x * cameraSpeed, camDirection.y * cameraSpeed);
	

	if (Maths::distance(camera.getCenter(), nextCameraPosition) > 5)
	{
		camera.setCenter(camera.getCenter() + camDirection);
	}

	else
	{
		camera.setCenter(nextCameraPosition);
	}


	windowScreen.setView(camera);
}
