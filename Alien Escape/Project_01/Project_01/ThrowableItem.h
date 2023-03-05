#pragma once
#include "Item.h"
#include "Particles.h"

enum class Direction
{
    Up,
    Down,
    Left,
    Right
};

class ThrowableItem : public Item
{
private:
    void throwItem(float t_angle);
    float angleCalculation(sf::Vector2f t_playerDirection);
    sf::Vector2f playerPos;


protected:

    ParticleSystem particleSystem[5];
    int particleInit = 0;
    int timeBetweenInits = 0;
    const int TIME = 1 * 5;

    const int THROW_DISTANCE = 200; 

public:

    ThrowableItem(ItemType t_type);
    ThrowableItem(ItemType t_type, sf::Vector2f t_position);
    void useItem(sf::Vector2f t_playerPos, Direction t_angle);

    void renderOnGrid(sf::RenderWindow& t_window) override;

    bool updateThrow();
};

