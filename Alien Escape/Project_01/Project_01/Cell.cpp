#include "Cell.h"

void Cell::setPassable(bool t_isPassable)
{
    m_isPassable = t_isPassable;
}

void Cell::setId(int t_id)
{
    m_id = t_id;
}

void Cell::setCentre(sf::Vector2f t_centre)
{
    m_centreX = t_centre.x;
    m_centreY = t_centre.y;
}

void Cell::setExit(bool t_isExit)
{
    isExit = t_isExit;
}

void Cell::setMarked(bool t_isMarked)
{
    m_marked = t_isMarked;
}

void Cell::addNeighbours(int t_cellId)
{
    m_neighbours.push_back(t_cellId);
}

std::vector<int>& Cell::neighbours() // return a list of neighbours
{
    return m_neighbours;
}

int Cell::id() const
{
    return m_id;
}

bool Cell::isMarked() const
{
    return m_marked;
}

int Cell::previous() const
{
    return m_previousCellId;
}

void Cell::setPrevious(int t_previous) // setting the parent cell 
{
    m_previousCellId = t_previous;
}

sf::Vector2f Cell::getPosition() // get the x and y value of the centre of the cell
{
    return sf::Vector2f(m_centreX, m_centreY);
}

bool Cell::getPassable() const
{
    return m_isPassable;
}

bool Cell::getExit() const
{
    return isExit;
}

void Cell::addNeighboursDistance(float t_distance)
{
    m_neighboursDistance.push_back(t_distance);
}

std::vector<float>& Cell::neighboursDistance()
{
    return m_neighboursDistance;
}

void Cell::addImpassableNeighbours(int t_additionalNeighbours)
{
    impassable_Neighbours_Counter = t_additionalNeighbours;
}

int Cell::totalImpassableNeighbours()
{
    return impassable_Neighbours_Counter;
}

