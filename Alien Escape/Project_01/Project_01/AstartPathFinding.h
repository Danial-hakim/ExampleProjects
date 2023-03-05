#pragma once

#include<iostream>
#include<string>
#include<fstream>
#include<queue>
#include<map>
#include "Grid.h"
#include "NodeData.h"
#include "Graph.h"

typedef GraphArc<NodeData, int>Arc;
typedef GraphNode<NodeData, int>Node;

class Pathfinder {
public:
	void writeToFile(int level);
	Grid* getGridRef();

	std::vector<sf::Vector2f> setPath(int alienCell , int targetCell);

	Pathfinder(Grid* t_grid);

	void readingFromFile(int level);
private:

	Grid* m_gridRef;
	std::vector<std::pair<Cell, int>>* smokeCellsRef;
	int cellsNum = ScreenSize::ROWS * ScreenSize::COLUMNS;
	Graph<NodeData, int> graph;
};
