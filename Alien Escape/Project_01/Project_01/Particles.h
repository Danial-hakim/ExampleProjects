#include <SFML/Graphics.hpp>

class Particles
{
private:

    int timetoLive{ 0 }; // the amount of time the particle can stay on screen 
    sf::Vector2f velocity{ 0.5f, 0.5f };
    sf::RectangleShape shape;

public:
    void Draw(sf::RenderWindow& win);
    void Update();
    Particles() {}
    Particles(sf::Vector2f pos, sf::Vector2f vel);
};


#define maxParticles 50

class ParticleSystem
{
public:
    Particles particles[maxParticles];
    sf::Vector2f position;
    void Initialise(sf::Vector2f pos);
    void Update();
    void Draw(sf::RenderWindow& win);
    ParticleSystem() {};
};