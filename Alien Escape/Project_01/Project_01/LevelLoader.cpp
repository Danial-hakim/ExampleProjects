#include "LevelLoader.h"
///////////////////////////////
// Object Operators //////////
//////////////////////////////

void operator >> (const YAML::Node& t_objectNode, ObjectLayoutData& t_object)
{
	for (int i = 0; i < ScreenSize::COLUMNS; i++)
	{
		auto k = t_objectNode[std::to_string(i)];

		for (int j = 0; j < ScreenSize::ROWS; j++)
		{
			if (k[j].as<int>() == 1)
			{
				t_object.m_playerPos = sf::Vector2f(j,i);
			}

			else if (k[j].as<int>() == 2)
			{
				t_object.m_alienPos = sf::Vector2f(j, i);
			}

			else if (k[j].as<int>() >= 3 && k[j].as<int>() <= 6)
			{
				t_object.m_objectsPos.push_back(sf::Vector2f(j, i));
				t_object.m_objectTypes.push_back((ItemType)(k[j].as<int>() - 2));
			}
		}
	}
}

void operator >> (const ObjectLayoutData& t_object, YAML::Node& t_objectNode)
{
	for (int i = 0; i < ScreenSize::COLUMNS; i++)
	{
		for (int j = 0; j < ScreenSize::ROWS; j++)
		{
			t_objectNode[i][j] = 0;

			if (t_object.m_playerPos.x == j && t_object.m_playerPos.y == i)
			{
				t_objectNode[i][j] = 1;
			}

			else if (t_object.m_alienPos.x == j && t_object.m_alienPos.y == i)
			{
				t_objectNode[i][j] = 2;
			}

			else
			{
				for (int loop = 0; loop < t_object.m_objectsPos.size(); loop++)
				{
					if (t_object.m_objectsPos[loop].x == j && t_object.m_objectsPos[loop].y == i)
					{
						switch (t_object.m_objectTypes[loop])
						{
						case ItemType::SMOKE_BOMB:
							t_objectNode[i][j] = 3;
							break;

						case ItemType::NOISE_MAKER:
							t_objectNode[i][j] = 4;
							break;

						case ItemType::SCANNER:
							t_objectNode[i][j] = 5;
							break;

						case ItemType::BOOK:
							t_objectNode[i][j] = 6;
							break;

						default:
							break;
						}
						break;
					}
				}
			}
		}
	}
}

///////////////////////////////
// Floor layout operators ////
/////////////////////////////

void operator >> (FloorLayoutData& t_floor, YAML::Node& t_floorNode)
{
	for (int i = 0; i < ScreenSize::COLUMNS; i++)
	{
		for (int j = 0; j < ScreenSize::ROWS; j++)
		{
			t_floorNode[i][j] = 0;

			for (sf::Vector2i wall : t_floor.walls)
			{
				if (wall == sf::Vector2i(j, i))
				{
					t_floorNode[i][j] = 1;
					t_floor.walls.erase(std::remove(t_floor.walls.begin(), t_floor.walls.end(), wall), t_floor.walls.end());
					break;
				}
			}
		}
	}
}

void operator >> (const YAML::Node& t_floorNode, FloorLayoutData& t_floor)
{
	for (int i = 0; i < ScreenSize::COLUMNS; i++)
	{
		auto k = t_floorNode[std::to_string(i)];

		for (int j = 0; j < ScreenSize::ROWS; j++)
		{
			if (k[j].as<int>() == 1)
			{
				t_floor.walls.push_back(sf::Vector2i(j, i));
			}
		}
	}
}

////////////////////////////
// Texture operators //////
///////////////////////////
void operator >> (const TextureData& t_textures, YAML::Node& t_textureNode)
{
	for (unsigned i = 0; i < t_textures.m_textureStrings.size(); ++i)
	{
		t_textureNode[i]["name"] = t_textures.m_textureName[i];
		t_textureNode[i]["path"] = t_textures.m_textureStrings[i];
	}
}

void operator >> (const YAML::Node& t_textureNode, TextureData& t_textures)
{
	for (unsigned i = 0; i < t_textureNode.size(); ++i)
	{
		t_textures.m_textureStrings.push_back(t_textureNode[i]["path"].as<std::string>());
		t_textures.m_textureName.push_back(t_textureNode[i]["name"].as<std::string>());
	}
}

///////////////////////////////
// Level data operators //////
/////////////////////////////
void operator >> (const YAML::Node& t_levelNode, LevelData& t_level)
{
	for (int currentFloor = 0; currentFloor < 4; currentFloor++)
	{
		t_levelNode["floors"][std::to_string(currentFloor + 1)] >> t_level.m_floors[currentFloor];
		t_levelNode["objects"][std::to_string(currentFloor + 1)] >> t_level.m_objects[currentFloor];
	}

	t_levelNode["textures"] >> t_level.m_textures;
}

void operator >> (LevelData& t_level, YAML::Node& t_levelNode)
{
	for (int currentFloor = 0; currentFloor < 4; currentFloor++)
	{
		t_level.m_floors[currentFloor] >> t_levelNode["floors"][std::to_string(currentFloor + 1)];
		t_level.m_objects[currentFloor] >> t_levelNode["objects"][std::to_string(currentFloor + 1)];
	}

	t_level.m_textures >> t_levelNode["textures"];
}


////////////////////////////////////////////////////////////
void LevelLoader::load(LevelData& t_level, std::string t_fileName)
{
	resetLoad(t_level);
	try
	{
		YAML::Node baseNode = YAML::LoadFile(t_fileName);
		if (baseNode.IsNull())
		{
			std::string message("File: " + t_fileName + " not found");
			throw std::exception(message.c_str());
		}
		baseNode >> t_level;
	}
	catch (YAML::ParserException& e)
	{
		std::string message(e.what());
		message = "YAML Parser Error: " + message;
		throw std::exception(message.c_str());
	}
	catch (std::exception& e)
	{
		std::string message(e.what());
		message = "Unexpected Error: " + message;
		throw std::exception(message.c_str());
	}
}

void LevelLoader::resetLoad(LevelData& t_level)
{
	for (int i = 0; i < 4; i++)
	{
		t_level.m_floors[i].walls.clear();
		t_level.m_objects[i].m_objectTypes.clear();
		t_level.m_objects[i].m_objectsPos.clear();
	}
}

void LevelLoader::save(LevelData& t_level)
{
	std::string filename = "./levels/levelTest.yaml";

	try
	{
		YAML::Node baseNode;
		try
		{
			baseNode = YAML::LoadFile(filename);
		}

		catch (std::exception& e)
		{
			std::ofstream newFile{ "./levels/levelTest.yaml", std::ios::binary};
			std::ifstream src("./levels/level1.yaml", std::ios::binary);

			newFile << src.rdbuf();

			src.close();
			newFile.close();

			baseNode = YAML::LoadFile(filename);
		}

		t_level >> baseNode;

		std::ofstream fout(filename);
		fout << baseNode;

	}
	catch (YAML::ParserException& e)
	{
		std::string message(e.what());
		message = "YAML Parser Error: " + message;
		throw std::exception(message.c_str());
	}
	catch (std::exception& e)
	{
		std::string message(e.what());
		message = "Unexpected Error: " + message;
		throw std::exception(message.c_str());
	}
}


