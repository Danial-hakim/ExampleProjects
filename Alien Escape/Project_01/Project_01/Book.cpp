#include "Book.h"

Book::Book(std::string t_code) : Item(ItemType::BOOK), doorUnlockCode(t_code)
{
}

Book::Book(std::string t_code, sf::Vector2f t_position) : Item(ItemType::BOOK, t_position), doorUnlockCode(t_code)
{
}

void Book::useItem()
{
	std::cout << "Use Book" << std::endl;
	RememberedCodes::learnCode(doorUnlockCode);
}

void Book::update(){}
