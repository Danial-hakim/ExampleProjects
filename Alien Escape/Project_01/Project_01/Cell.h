#pragma once
#include <vector>
#include <SFML/Graphics.hpp>

class Cell
{
public:

	void setPassable(bool t_isPassable);
	void setId(int t_id);
	void setCentre(sf::Vector2f t_centre);
	void setExit(bool t_isExit);

	void addNeighbours(int t_cellId);
	std::vector<int>& neighbours();

	int id() const;
	sf::Vector2f getPosition();
	bool getPassable() const;
	bool getExit()const;

	void addNeighboursDistance(float t_distance);
	std::vector<float>& neighboursDistance();

	void addImpassableNeighbours(int t_additionalNeighbours);
	int totalImpassableNeighbours();

	//Was used in BFS so we might need it later
	void setMarked(bool t_isMarked);
	bool isMarked() const;
	int previous() const;
	void setPrevious(int t_previous);
private:

	int m_id; // the id of the cell
	bool m_isPassable{ true }; // if passable is false , there is a sprite/rectangle on the cell 
	bool isExit{ false };

	int m_centreX; // the centre of the cell on x axis
	int m_centreY; // the centre of the cell on y axis
	std::vector<int> m_neighbours; //the cells around the current cell
	std::vector<float> m_neighboursDistance;
	int impassable_Neighbours_Counter = 0;


	//Was used in BFS so we might use it later 
	int m_previousCellId{ -1 }; // the parent id
	bool m_marked; // if mark is false , the algorithm have not reach this cell 
};
