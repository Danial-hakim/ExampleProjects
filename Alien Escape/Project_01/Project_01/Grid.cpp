#include "Grid.h"
#include "CollisionHandler.h"

Grid::Grid() : fireAnimation(fireSprite)
{
	m_cells.resize(ScreenSize::ROWS * ScreenSize::COLUMNS);

	for (int i = 0; i < ScreenSize::ROWS * ScreenSize::COLUMNS; i++)
	{
		m_cells.at(i).setId(i);
	}

	for (int col = 0; col < ScreenSize::COLUMNS; col++)
	{
		for (int row = 0; row < ScreenSize::ROWS; row++)
		{
			int index = ScreenSize::COLUMNS * row + col;

			float colOffSet = col * cellHeight;
			float rowOffSet = row * cellWidth;

			m_cells.at(index).setCentre(sf::Vector2f(rowOffSet + cellWidth / 2,
				colOffSet + cellHeight / 2));
			neighbours(col, row);

			//std::cout << index << std::endl;
		}
	}
}

void Grid::init(std::vector<sf::Vector2i> t_objectsPos)
{
	for (Cell& tempCell : m_cells)
	{
		tempCell.setPassable(true);
	}

	

	if (!fireTexture.loadFromFile("./Assets/Sprites/FinalSprites/fire.png"))
	{
		std::cout << "spritesheet error - player" << std::endl;
	}


	if (!IDFont.loadFromFile("./Assets/Fonts/arial.ttf"))
	{
		std::cout << "Error with ID font" << std::endl;
	}

	IDText.setFont(IDFont);

	fireSprite.setTexture(fireTexture);
	sf::Vector2f offset = sf::Vector2f{ fireTexture.getSize().x / SPRITESHEET_COL, fireTexture.getSize().y / SPRITESHEET_ROW };

	fireSprite.setOrigin(offset);

	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 5; j++)
		{
			fireAnimation.addFrame({ sf::IntRect(i * WIDTH , j * HEIGHT , WIDTH, HEIGHT), ANIM_SPEED });
		}
	}


	for (int i = 0; i < ScreenSize::ROWS; i++)
	{
		for (int j = 0; j < ScreenSize::COLUMNS; j++)
		{
			grid[i][j].setSize(sf::Vector2f(cellWidth, cellHeight));

			grid[i][j].setPosition(i * cellWidth, j * cellHeight);

			grid[i][j].setOutlineThickness(1);

			grid[i][j].setOutlineColor(sf::Color::Blue);

			grid[i][j].setFillColor(sf::Color::Transparent);
		}
	}

	int cellID = 0;

	for (sf::Vector2i v : t_objectsPos)
	{
		cellID = getCellID(ScreenSize::getActualPosition(v.x, v.y));
		m_cells.at(cellID).setPassable(false);
		grid[v.x][v.y].setFillColor(sf::Color::Green);

		m_originalWalls.push_back(cellID);
	}

	for (int row = 0; row < ScreenSize::COLUMNS; row++)
	{
		for (int col = 0; col < ScreenSize::ROWS; col++)
		{
			int index = ScreenSize::COLUMNS * col + row;

			float rowOffset = row * cellHeight;
			float colOffset = col * cellWidth;

			calculateNeighboursDistance(row, col);
			addImpassableNeighbours(row, col);
		}
	}
}

void Grid::draw(sf::RenderWindow& t_window, bool t_debugMode)
{
	if (!t_debugMode)
	{
		for (std::pair<Cell, int> centreCell : smokeCells)
		{
			std::vector<int> neighbours = centreCell.first.neighbours();

			for (int nb : neighbours)
			{
				fireSprite.setPosition(ScreenSize::getActualPosition(nb));
				t_window.draw(fireSprite);
			}
		}
	}
	else
	{
		debugMode(t_window);
	}
}

void Grid::neighbours(int t_row, int t_col)
{
	for (int direction = 0; direction < SURROUNDING_NEIGHBOURS; direction++)
	{
		int n_row = t_col + ((direction / 3) - 1); // neighbour row
		int n_col = t_row + ((direction % 3) - 1);

		if (n_row >= 0 && n_row < ScreenSize::ROWS && n_col >= 0 && n_col < ScreenSize::COLUMNS)
		{
			int neighbourIndex = n_row * ScreenSize::COLUMNS + n_col;
			int index = t_col * ScreenSize::COLUMNS + t_row;

			m_cells.at(index).addNeighbours(neighbourIndex);
		}
	}
}

void Grid::refreshImpassableCells()
{
	int cellID = 0;

	for (int i = 0; i < ScreenSize::ROWS; i++)
	{
		for (int j = 0; j < ScreenSize::COLUMNS; j++)
		{
			cellID = (i * ScreenSize::COLUMNS) + j;

			if (!m_cells[cellID].getPassable())
			{
				grid[i][j].setFillColor(sf::Color::Green);
			}

			else
			{
				grid[i][j].setFillColor(sf::Color::Transparent);
			}
		}
	}

}

std::vector<Cell> Grid::getCell()
{
	return m_cells;
}

sf::Vector2f Grid::getCellOrigin()
{
	return sf::Vector2f(cellWidth / 2, cellHeight / 2);
}

void Grid::update(float dt)
{
	if (smokeBombActive)
	{
		fireAnimation.update(dt, true);

		for (std::pair<Cell, int>& i : smokeCells)
		{
			i.second--;

			if (i.second <= 0)
			{
				std::vector<int>* neighbours = &i.first.neighbours();
				neighbours->push_back(i.first.id());

				for (int i : *neighbours)
				{
					m_cells[i].setPassable(true);

					for (int wall : m_originalWalls)
					{
						if (i == wall)
						{
							m_cells[i].setPassable(false);
							break;
						}
					}
				}

				smokeCells.erase(std::remove_if(smokeCells.begin(), smokeCells.end(), 

					[=] (const std::pair<Cell, int> t_cell)
					{
						return t_cell.first.id() == i.first.id();
					}

				), smokeCells.end());

				refreshImpassableCells();

				if (smokeCells.empty())
				{
					smokeBombActive = false;
				}
			}
		}
	}
}

void Grid::smokeBombReaction(sf::Vector2f t_smokeBombPosition)
{
	smokeBombActive = true;

	int id = getCellID(t_smokeBombPosition);

	std::vector<int>* neighbours = &m_cells[id].neighbours();

	m_cells[id].setPassable(false);

	for (int i : *neighbours)
	{
		m_cells[i].setPassable(false);
	}

	smokeCells.push_back(std::pair<Cell, int>(m_cells[id], TIME_LEFT_FOR_SMOKE_TO_PASS));

	smokeChange = true;

	refreshImpassableCells();
}

bool Grid::cellsNearEachother(int A, int B)
{
	bool near = false;

	for (int c : m_cells[A].neighbours())
	{
		if (c == B)
		{
			near = true;
			break;
		}
	}

	return near;
}

int Grid::getCellID(sf::Vector2f t_position)
{
	return static_cast<int>(floor(t_position.x / cellWidth)) * ScreenSize::COLUMNS + static_cast<int>(floor(t_position.y / cellHeight));
}

sf::Vector2f Grid::getCellRowAndCol(sf::Vector2f t_position)
{
	sf::Vector2f rowCol;

	rowCol = sf::Vector2f(t_position.x / cellWidth, t_position.y / cellHeight);
	rowCol.x = floorf(rowCol.x);
	rowCol.y = floorf(rowCol.y);

	return rowCol;
}

void Grid::debugMode(sf::RenderWindow& t_window)
{
	for (int i = 0; i < ScreenSize::ROWS; i++)
	{
		for (int j = 0; j < ScreenSize::COLUMNS; j++)
		{
			t_window.draw(grid[i][j]);
		}
	}

	int col = 0;
	for (int i = 0; i < ScreenSize::ROWS; i++)
	{
		for (int j = 0; j < ScreenSize::COLUMNS; j++)
		{	
			IDText.setString(std::to_string(col));
			IDText.setPosition(i * cellWidth, j * cellHeight);
			IDText.setFillColor(sf::Color::Red);
			IDText.setCharacterSize(15.0f);
			t_window.draw(IDText);
			col++;
		}
	}
}

std::vector<std::pair<Cell, int>>* Grid::getSmokeCells()
{
	return &smokeCells;
}
void Grid::addImpassableNeighbours(int t_row, int t_col)
{
	int impassableNeighboursCounter = 0;
	int currentIndex = 0;
	for (int direction = 0; direction < SURROUNDING_NEIGHBOURS; direction++)
	{
		int n_row = t_col + ((direction / 3) - 1); // neighbour row
		int n_col = t_row + ((direction % 3) - 1);

		if (n_row >= 0 && n_row < ScreenSize::ROWS && n_col >= 0 && n_col < ScreenSize::COLUMNS)
		{
			int neighbourIndex = n_row * ScreenSize::COLUMNS + n_col;
			int index = t_col * ScreenSize::COLUMNS + t_row;

			float differences = index - neighbourIndex;

			if (differences == 1 || differences == -1 || differences == 25 || differences == -25)
			{
				if (m_cells.at(index).id() == m_cells.at(neighbourIndex).id())
				{
					continue;
				}
				else if (!m_cells.at(neighbourIndex).getPassable())
				{
					impassableNeighboursCounter++;
					m_cells.at(index).addImpassableNeighbours(impassableNeighboursCounter);
					//std::cout << "index = " << index << std::endl;
					//std::cout << "neighbours = " << impassableNeighboursCounter << std::endl;
				}
			}
			//currentIndex = index;
		}
	}
	
	
}

sf::Vector2i Grid::mouseCellCollisionLoop(sf::Vector2f t_mousePos, sf::RenderWindow& t_window)
{
	sf::Vector2i mouseOverCell = sf::Vector2i(-1,-1);

	for (int i = 0; i < ScreenSize::ROWS; i++)
	{
		for (int j = 0; j < ScreenSize::COLUMNS; j++)
		{
			if (CollisionHandler::mouse_cell(sf::Vector2f(t_window.mapCoordsToPixel(grid[i][j].getPosition())),
				grid[i][j].getSize(), sf::Vector2f(t_mousePos)))
			{
				mouseOverCell = sf::Vector2i(i, j);
			}

		}

	}

	return mouseOverCell;
}

bool Grid::gridMouseCheck(sf::Vector2f t_mousePos, sf::RenderWindow& t_window)
{
	sf::Vector2i result = mouseCellCollisionLoop(t_mousePos, t_window);

	if (result != sf::Vector2i(-1,-1))
	{
		int x = getCellID(ScreenSize::getActualPosition(result.x, result.y));
		m_cells.at(x).setPassable(!m_cells.at(x).getPassable());

		if (!m_cells[x].getPassable())
		{
			grid[result.x][result.y].setFillColor(sf::Color::Green);
		}

		else
		{
			grid[result.x][result.y].setFillColor(sf::Color::Transparent);
		}
	}

	return result != sf::Vector2i(-1, -1);
}

sf::Vector2i Grid::gridMouseCheck(sf::Vector2f t_mousePos, sf::RenderWindow& t_window, bool itemSelected)
{
	sf::Vector2i result = mouseCellCollisionLoop(t_mousePos, t_window);

	if (result != sf::Vector2i(-1, -1) && !itemSelected)
	{
		int x = getCellID(ScreenSize::getActualPosition(result.x, result.y));
		m_cells.at(x).setPassable(!m_cells.at(x).getPassable());

		if (!m_cells[x].getPassable())
		{
			grid[result.x][result.y].setFillColor(sf::Color::Green);
		}

		else
		{
			grid[result.x][result.y].setFillColor(sf::Color::Transparent);
		}
	}

	return result;
}

void Grid::calculateNeighboursDistance(int t_row, int t_col)
{
	sf::Vector2f fromCellPos ;
	sf::Vector2f toCellPos ;

	for (int direction = 0; direction < SURROUNDING_NEIGHBOURS; direction++)
	{
		int n_row = t_col + ((direction / 3) - 1); // neighbour row
		int n_col = t_row + ((direction % 3) - 1);

		if (n_row >= 0 && n_row < ScreenSize::ROWS && n_col >= 0 && n_col < ScreenSize::COLUMNS)
		{
			int neighbourIndex = n_row * ScreenSize::COLUMNS + n_col;
			int index = t_col * ScreenSize::COLUMNS + t_row;

			if (m_cells.at(index).getPassable())
			{
				if (m_cells.at(neighbourIndex).getPassable())
				{
					fromCellPos = m_cells.at(index).getPosition();
					toCellPos = m_cells.at(neighbourIndex).getPosition();

					m_cells.at(index).addNeighboursDistance(Maths::distance(fromCellPos, toCellPos));

					//std::cout << index << std::endl;
				}
			}
		}
	}
}

