#pragma once
#include "Screen.h"

class LevelSelectMenu : public Screen
{
public:

	LevelSelectMenu(sf::RenderWindow& t_window);

	void init();

private:

	virtual void firstButtonPress(int& t_lvlChecker) override;
	virtual void secondButtonPress(int& t_lvlChecker) override;
	virtual void thirdButtonPress(int& t_lvlChecker) override;
};

