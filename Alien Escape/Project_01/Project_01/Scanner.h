#pragma once
#include "Item.h"
#include "SoundManager.h"
#include "Grid.h"
#include "ScannerObserver.h"
#include "Camera.h"

class Scanner : public Item
{
private:
	sf::RectangleShape background;
	Camera* cameraRef;
	bool screenOpen = false;

	ScannerObserver* observer;

	void initialise(ScannerObserver* t_observer);

public:
	Scanner(Camera* t_camera, ScannerObserver* t_observer);
	Scanner(sf::Vector2f t_position, ScannerObserver* t_observer);

	void useItem(sf::Vector2f t_playerPos);
	void useItem();
	void update();

	void storeCamera(Camera* t_camera);

	void renderInInventory(sf::RenderWindow& t_window) override;
	void renderOnGrid(sf::RenderWindow& t_window) override;

	bool isScannerOn();

	void setCameraRef(Camera* t_camera);

};

