#pragma once
#include <SFML/Graphics.hpp>

class Maths
{
public:
	static sf::Vector2f unitVector(sf::Vector2f vector);
	static float dotProduct(sf::Vector2f vector);
	static sf::Vector2f truncate(sf::Vector2f vector, float const max);
	static float squaredLength(sf::Vector2f vector);
	static float length(sf::Vector2f vector);
	static float distance(sf::Vector2f vector_1, sf::Vector2f vector_2);
};

