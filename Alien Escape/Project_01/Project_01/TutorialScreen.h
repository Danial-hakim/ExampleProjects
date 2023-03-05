#pragma once
#include "Screen.h"
class TutorialScreen : public Screen
{
public:
	TutorialScreen(sf::RenderWindow& t_window);

	void init();

private:

	virtual void firstButtonPress(int& t_lvlChecker) override;
};

