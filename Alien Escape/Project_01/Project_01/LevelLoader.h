#pragma once

#include <SFML/System/Vector2.hpp>
#include "ScreenSize.h"
#include <vector>
#include <fstream>
#include <iostream>
#include "yaml-cpp\yaml.h"
#include <string>
#include "Item.h"

/// <summary>
/// @brief A struct to represent Obstacle data in the level.
/// 
/// </summary>

struct ObjectLayoutData
{
	sf::Vector2f m_playerPos = sf::Vector2f(-1,-1);
	sf::Vector2f m_alienPos = sf::Vector2f(-1, -1);
	std::vector<sf::Vector2f> m_objectsPos;
	std::vector<ItemType> m_objectTypes;
};

struct FloorLayoutData
{
	std::vector<sf::Vector2i> walls;
};

struct TextureData
{
	std::vector<std::string> m_textureStrings;
	std::vector<std::string> m_textureName;
};

/// <summary>
/// @brief A struct representing all the Level Data.
/// 
/// Obstacles may be repeated and are therefore stored in a container.
/// </summary>
struct LevelData
{
	ObjectLayoutData m_objects[4];
	FloorLayoutData m_floors[4];
	TextureData m_textures;
};


class LevelLoader
{
public:

	/// <summary>
	/// @brief No-op default constructor.
	/// 
	/// </summary>
	LevelLoader() = default;

	static void load(LevelData& t_level, std::string t_fileName);
	static void resetLoad(LevelData& t_level);
	static void save(LevelData& t_level);
};