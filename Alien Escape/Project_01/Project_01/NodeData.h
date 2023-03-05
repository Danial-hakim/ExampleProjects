#pragma once
#include <string>

struct NodeData
{
	std::string m_name;
	int index;
	int m_pathCost;
	int heuristic;
	int xPos;
	int yPos;
};