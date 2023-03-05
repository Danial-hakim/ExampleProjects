#include "Maths.h"

sf::Vector2f Maths::unitVector(sf::Vector2f vector)
{
	if (vector.x != 0 || vector.y != 0)
	{
		return vector / length(vector);
	}

	else
	{
		return vector;
	}
}

float Maths::dotProduct(sf::Vector2f vector)
{
	return vector.x * vector.x + vector.y * vector.y;
}

sf::Vector2f Maths::truncate(sf::Vector2f vector, float const max)
{
	float length = std::sqrtf(dotProduct(vector));

	if (length > max)
	{
		vector = unitVector(vector);
		vector *= max;
	}
	return vector;
}

float Maths::squaredLength(sf::Vector2f vector)
{
	return dotProduct(vector);
}

float Maths::length(sf::Vector2f vector)
{
	return std::sqrtf(squaredLength(vector));
}

float Maths::distance(sf::Vector2f vector_1, sf::Vector2f vector_2)
{
	return std::sqrtf((vector_1.x - vector_2.x) * (vector_1.x - vector_2.x) + (vector_1.y - vector_2.y) * (vector_1.y - vector_2.y));
}