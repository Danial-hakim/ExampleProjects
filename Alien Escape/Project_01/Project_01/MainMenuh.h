#pragma once
#include "Screen.h"

class MainMenu : public Screen
{
public:

	MainMenu(sf::RenderWindow& t_window);

	void init();

private:

	virtual void firstButtonPress(int& t_lvlChecker) override;
	virtual void secondButtonPress(int& t_lvlChecker) override;
	virtual void thirdButtonPress(int& t_lvlChecker) override;
	virtual void fourthButtonPress(int& t_lvlChecker) override;
};