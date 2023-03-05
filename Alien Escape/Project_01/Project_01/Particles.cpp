#include "Particles.h"

void Particles::Draw(sf::RenderWindow& win)
{
    if (timetoLive > 0)
    {
        win.draw(shape);
    }
}

void Particles::Update()
{
    if (timetoLive > 0)
    {
        shape.move(velocity);
        timetoLive--;
    }
}

Particles::Particles(sf::Vector2f pos, sf::Vector2f vel)
{
    shape.setSize(sf::Vector2f(4, 4));
    shape.setPosition(pos);
    shape.setFillColor(sf::Color(255, 140, 0));

    velocity = vel;
    timetoLive = rand() % 20;
}

void ParticleSystem::Initialise(sf::Vector2f pos)
{
    position = pos;
    for (int i = 0; i < maxParticles; i++)
    {
        particles[i] = Particles(position, sf::Vector2f(rand() / double(RAND_MAX) * 4 - 2, rand() / double(RAND_MAX) * 4 - 2));
    }
}

void ParticleSystem::Update()
{
    for (int i = 0; i < maxParticles; i++)
    {
        particles[i].Update();
    }
}

void ParticleSystem::Draw(sf::RenderWindow& win)
{
    for (int i = 0; i < maxParticles; i++)
    {
        particles[i].Draw(win);
    }
}