#include "AstartPathFinding.h"

Pathfinder::Pathfinder(Grid* t_grid) : m_gridRef(t_grid), graph(cellsNum, t_grid->getSmokeCells()), smokeCellsRef(t_grid->getSmokeCells())
{
}

void Pathfinder::writeToFile(int level)
{
	graph.clearArcsAndNodes();

	std::string cellNum = "";
	std::string cell_posX = "";
	std::string cell_posY = "";

	smokeCellsRef = m_gridRef->getSmokeCells();

	std::ofstream outfile;

	outfile.open("Nodes" + std::to_string(level) + ".txt");

	std::vector<Cell> cell = m_gridRef->getCell();
 
	for (Cell tempCell : cell)
	{
		cellNum = std::to_string(tempCell.id());
		cell_posX = std::to_string(static_cast<int>(tempCell.getPosition().x));
		cell_posY = std::to_string(static_cast<int>(tempCell.getPosition().y));

		outfile << cellNum << " " << cell_posX << " " << cell_posY << std::endl;
	}

	outfile.close();

	outfile.open("Arcs" + std::to_string(level) + ".txt");

	std::string fromCell = "";
	std::string toCell = "";
	std::string distance = "";
	int index = 0;

	for (Cell tempCell : cell)
	{
		if (tempCell.getPassable())
		{
			fromCell = std::to_string(tempCell.id());

			for (int neighbours : tempCell.neighbours())
			{
				if(cell.at(neighbours).getPassable())
				{
					if (tempCell.neighboursDistance().at(index) != 0)
					{
						toCell = std::to_string(neighbours);
						distance = std::to_string(static_cast<int>(tempCell.neighboursDistance().at(index)));
						outfile << fromCell << " " << toCell << " " << distance << std::endl;
					}					
					index++;
				}
			}
		}
		index = 0;
	}

	outfile.close();

	readingFromFile(level);
}

Grid* Pathfinder::getGridRef()
{
	return m_gridRef;
}

std::vector<sf::Vector2f> Pathfinder::setPath(int alienCell, int targetCell)
{
	std::vector<Node*> path;

	graph.clearMarks();

	graph.aStar(graph.nodeIndex(alienCell), graph.nodeIndex(targetCell), path);

	std::vector<sf::Vector2f> cells_Pos;
	int cell_x;
	int cell_y;

	for (int i = path.size() - 1; i >= 0 ; i--)
	{
		cell_x = path.at(i)->m_data.xPos;
		cell_y = path.at(i)->m_data.yPos;
		cells_Pos.push_back(sf::Vector2f(cell_x, cell_y));
		//std::cout << path.at(i)->m_data.m_name << std::endl;
	}
	//std::cout << "=============" << std::endl;
	return cells_Pos;
}

void Pathfinder::readingFromFile(int level)
{
	//Create a graph with capacity for 750 nodes. we have 750 cells
	//Templates parameters are NodeType (std::string), ArcType (int)
	std::map<std::string, int> cells;

	NodeData nodeData;
	int nodeIndex = 0;
	std::ifstream myFile;

	myFile.close();

	myFile.open("Nodes" + std::to_string(level) + ".txt");

	while (myFile >> nodeData.m_name >> nodeData.xPos >> nodeData.yPos)
	{
		cells.insert(std::make_pair(nodeData.m_name, nodeIndex));
		graph.addNode(nodeData, nodeIndex++);
	}
	myFile.close();

	//Create the arcs between the nodes
	myFile.open("Arcs" + std::to_string(level) + ".txt");

	std::string fromCell;
	std::string toCell;

	int distance;

	while (myFile >> fromCell >> toCell >> distance)
	{
		graph.addArc(cells[fromCell], cells[toCell], distance);
	}
	myFile.close();
}

