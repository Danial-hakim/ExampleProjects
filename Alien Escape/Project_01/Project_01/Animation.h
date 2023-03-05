#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>

struct Frame
{
	sf::IntRect rect;
	double duration;
};

class Animation
{
public:

	Animation(sf::Sprite& t_target);
	virtual ~Animation();
	void addFrame(Frame&& t_frame);
	void update(double t_elapsed , bool t_loop);
	const double getLength() const { return totalLength; };

private:
	std::vector<Frame> frames;
	double totalLength;
	double progress;
	sf::Sprite& target;
};