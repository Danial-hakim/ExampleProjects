#include "GridRenderer.h"

void GridRenderer::init(Grid* t_grid)
{
	grid = t_grid;
	cell = grid->getCell();

	groundSprite = new sf::Sprite[ScreenSize::ROWS * ScreenSize::COLUMNS];

	if (!standAloneWall.loadFromFile("./Assets/Sprites/FinalSprites/walls/0_wall.png"))
	{
		std::cout << "spritesheet error - 0" << std::endl;
	}
	if (!endWall.loadFromFile("./Assets/Sprites/FinalSprites/walls/1_wall.png"))
	{
		std::cout << "spritesheet error - 1" << std::endl;
	}
	if (!straightWall.loadFromFile("./Assets/Sprites/FinalSprites/walls/2_wall.png"))
	{
		std::cout << "spritesheet error - 2" << std::endl;
	}
	if (!threeWayWall.loadFromFile("./Assets/Sprites/FinalSprites/walls/3_wall.png"))
	{
		std::cout << "spritesheet error - 3" << std::endl;
	}
	if (!junctionWall.loadFromFile("./Assets/Sprites/FinalSprites/walls/4_wall.png"))
	{
		std::cout << "spritesheet error - 4" << std::endl;
	}
	if (!cornerWallTex.loadFromFile("./Assets/Sprites/FinalSprites/walls/corner_wall.png"))
	{
		std::cout << "spritesheet error - corner" << std::endl;
	}
	if (!groundTexture.loadFromFile("./Assets/Sprites/FinalSprites/walls/ground.jpg"))
	{
		std::cout << "spritesheet error - ground" << std::endl;
	}

	originOffset = { standAloneWall.getSize().x / 2.0f, standAloneWall.getSize().y / 2.0f };
	groundOffset = { groundTexture.getSize().x / 2.0f, groundTexture.getSize().y / 2.0f };

	setupGrounds();
	setUpSprite();

}

void GridRenderer::draw(sf::RenderWindow& t_window)
{
	if (test == true)
	{
		t_window.clear(sf::Color(0, 0, 0, 0));
		test = false;
	}
	for (int i = 0; i < 750; i++)
	{
		t_window.draw(groundSprite[i]);
		if (!cell[i].getPassable())
		{
			t_window.draw(gridSprite[i]);
		}
	}
}

void GridRenderer::setUpSprite()
{
	// grid width = 48  
	// grid height = 48
	float width = cellWidth / 64; // 48/64
	float height = cellHeight / 64; // 48/64
	int currentRotation = 0;
	
	for (Cell tempCell : cell)
	{
		int id = tempCell.id();

		sf::Vector2f spritePos = { tempCell.getPosition().x , tempCell.getPosition().y };

		if (!tempCell.getPassable())
		{
			int totalNeighbours = tempCell.totalImpassableNeighbours();

			if (tempCell.totalImpassableNeighbours() == 0)
			{
				gridSprite[id].setTexture(standAloneWall);
				currentRotation = 90;
			}
			else if (tempCell.totalImpassableNeighbours() == 1)
			{
				for (int neigbour : tempCell.neighbours())
				{				
					if (!cell.at(neigbour).getPassable() && cell.at(neigbour).id() != tempCell.id())
					{
						currentRotation = getRotation(tempCell.id(), cell.at(neigbour).id());
					}
				}
				gridSprite[id].setTexture(endWall);
			}
			else if (tempCell.totalImpassableNeighbours() == 2)
			{
				if (isCornerWall(id))
				{
					gridSprite[id].setTexture(cornerWallTex);
				}
				else
				{
					gridSprite[id].setTexture(straightWall);
				}
				currentRotation = rotation;
			}
			else if (tempCell.totalImpassableNeighbours() == 3)
			{
				gridSprite[id].setTexture(threeWayWall);
				currentRotation = threeWayRotation(id);				
			}
			else if (tempCell.totalImpassableNeighbours() == 4)
			{
				gridSprite[id].setTexture(junctionWall);
				currentRotation = 90;
			}
		}

		gridSprite[id].setScale(width , height);
		gridSprite[id].setOrigin(originOffset);
		gridSprite[id].setRotation(currentRotation);
		gridSprite[id].setPosition(spritePos);
	}
}

void GridRenderer::updateGrid(sf::Vector2f t_clickedPos)
{
	int id = Grid::getCellID(t_clickedPos);
	std::vector<Cell> cell = grid->getCell();
	int currentRotation = 0;
	float width = cellWidth / 64; // 48/64
	float height = cellHeight / 64; // 48/64


	for (int i : cell[id].neighbours())
	{
		currentRotation = checkWallType(cell[i], cell);

		gridSprite[i].setScale(width, height);
		gridSprite[i].setOrigin(originOffset);
		gridSprite[i].setRotation(currentRotation);
		gridSprite[i].setPosition(cell[i].getPosition());
	}
}

int GridRenderer::checkWallType(Cell& t_currentCell, std::vector<Cell> t_cell)
{
	int totalNeighbours = t_currentCell.totalImpassableNeighbours();
	int id = t_currentCell.id();
	int currentRotation = 0;


	if (t_currentCell.totalImpassableNeighbours() == 0)
	{
		gridSprite[id].setTexture(standAloneWall);
		currentRotation = 90;
	}
	else if (t_currentCell.totalImpassableNeighbours() == 1)
	{
		for (int neigbour : t_currentCell.neighbours())
		{
			if (!t_cell.at(neigbour).getPassable() && t_cell.at(neigbour).id() != t_currentCell.id())
			{
				currentRotation = getRotation(t_currentCell.id(), t_cell.at(neigbour).id());
			}
		}
		gridSprite[id].setTexture(endWall);
	}
	else if (t_currentCell.totalImpassableNeighbours() == 2)
	{
		if (isCornerWall(id))
		{
			gridSprite[id].setTexture(cornerWallTex);
		}
		else
		{
			std::cout << id << " " << "not corner" << std::endl;
			gridSprite[id].setTexture(straightWall);
		}
		currentRotation = rotation;
	}
	else if (t_currentCell.totalImpassableNeighbours() == 3)
	{
		gridSprite[id].setTexture(threeWayWall);
		currentRotation = threeWayRotation(id);
	}
	else if (t_currentCell.totalImpassableNeighbours() == 4)
	{
		gridSprite[id].setTexture(junctionWall);
		currentRotation = 90;
	}

	return currentRotation;
}

int GridRenderer::getRotation(int t_cellID, int t_neighbourID)
{
	int differences = t_cellID - t_neighbourID;

	if (differences == 1) // neighbour below
	{
		rotation = 90;
	}
	else if (differences == -1) // neighbour above
	{
		rotation = -90;
	}
	else if (differences == -25) // neighbour to the right
	{
		rotation = 180;
	}
	else // if the differences is 25 , neighbour is on the left 
	{
		rotation = 0;
	}

	// incase the last cell checked is diagonal to the current cell
	if (differences != 1 && differences != -1 && differences != -25 && differences != 25)
	{
		rotation = lastRotation;
	}

	lastRotation = rotation;

	return rotation;
}

bool GridRenderer::isCornerWall(int t_cellID)
{
	int firstNeighbour = 0;
	int secondNeighbour = 0;

	for (int neighbourID : cell.at(t_cellID).neighbours())
	{	
		if (!cell.at(neighbourID).getPassable() && neighbourID != t_cellID)
		{
			int differences = t_cellID - neighbourID; // to see check if the neighbour is diagonal to current cell 
			if(differences == 1 || differences == -1 || differences == 25 || differences == -25) // should be true if not diagonal 
			{
				if (firstNeighbour == 0)
				{
					firstNeighbour = neighbourID;
				}
				else
				{
					secondNeighbour = neighbourID;
				}
			}
		}
	}

	if (t_cellID == 530)
	{
		int i = 0;
	}
	int dif = firstNeighbour - secondNeighbour;

	int cornerWallDif = t_cellID - firstNeighbour;

	if (dif == 50 || dif == -50 || dif == -2 || dif == 2) // the neighbours of the current cell is opposite of each other
	{
		if (dif == 50 || dif == -50)
		{
			rotation = 0;
		}
		else
		{
			rotation = 90;
		}
		return false;
	}
	else
	{
		if (t_cellID < firstNeighbour)
		{
			rotation = 0;
		}
		else if (t_cellID > secondNeighbour)
		{
			rotation = 180;
		}
		else if (t_cellID > firstNeighbour && t_cellID < secondNeighbour)
		{
			if (cornerWallDif == 1)
			{
				rotation = -90;
			}
			else
			{
				rotation = 90;
			}
		}
		return true;
	}
}

int GridRenderer::threeWayRotation(int t_cellID)
{
	int dif = 0;
	
	int firstNeighbour = -1;
	int secondNeighbour = -1;
	int thirdNeighbour = -1;

	for (int neighbourID : cell.at(t_cellID).neighbours())
	{
		if (!cell.at(neighbourID).getPassable())
		{
			dif = t_cellID - neighbourID;

			if (dif == -1 || dif == 1 || dif == -25 || dif == 25)
			{
				if (firstNeighbour == -1)
				{
					firstNeighbour = neighbourID;
				}
				else if (firstNeighbour != -1 && secondNeighbour == -1)
				{
					secondNeighbour = neighbourID;
				}
				else
				{
					thirdNeighbour = neighbourID;
				}
			}
		}
	}

	int firstDif = t_cellID - firstNeighbour;
	int secondDif = t_cellID - secondNeighbour;
	int thirdDif = t_cellID - thirdNeighbour;
	int rotation = 0;

	if (firstDif == 1)
	{
		rotation = 0;
	}
	else if (secondDif == -1)
	{
		rotation = 90;
	}
	else if (thirdDif == -1)
	{
		rotation = 180;
	}
	else
	{
		rotation = -90;
	}


	return rotation;
}

void GridRenderer::setupGrounds()
{
	float width = cellWidth / 32; // 48/64
	float height = cellHeight / 32; // 48/64

	for (Cell tempCell : cell)
	{
		int id = tempCell.id();

		sf::Vector2f spritePos = { tempCell.getPosition().x , tempCell.getPosition().y };

		groundSprite[id].setTexture(groundTexture);
		groundSprite[id].setScale(width, height);
		groundSprite[id].setOrigin(groundOffset);
		groundSprite[id].setPosition(spritePos);
	}
}

