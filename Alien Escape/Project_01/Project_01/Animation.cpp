#include "Animation.h"

Animation::Animation(sf::Sprite& t_target) : target(t_target)
{
	progress = totalLength = 0.0f;
}

Animation::~Animation()
{
}

void Animation::addFrame(Frame&& t_frame)
{
	totalLength += t_frame.duration;
	frames.push_back(std::move(t_frame));
}

void Animation::update(double t_elapsed, bool t_loop)
{
	progress += t_elapsed;
	double p = progress;

	for (size_t i = 0; i < frames.size(); i++)
	{
		p -= frames[i].duration;

		if (t_loop && p > 0.0 && &(frames[i]) == &(frames.back()))
		{
			i = 0;
			continue;
		}
		if (p <= 0.0 || &(frames[i]) == &frames.back())
		{
			target.setTextureRect(frames[i].rect);
			break;
		}		
	}
}

