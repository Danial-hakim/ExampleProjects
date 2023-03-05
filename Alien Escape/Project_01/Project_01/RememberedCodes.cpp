#include "RememberedCodes.h"

std::vector<std::string> RememberedCodes::m_knownCodes;

void RememberedCodes::learnCode(std::string t_newCode)
{
	m_knownCodes.push_back(t_newCode);
	std::cout << "learned code : " << t_newCode << std::endl;
}

bool RememberedCodes::openDoorAttempt(std::string t_wantedCode)
{
	bool canOpenDoor = false;

	for (std::string code : m_knownCodes)
	{
		if (code == t_wantedCode)
		{
			canOpenDoor = true;
			break;
		}
	}

	return canOpenDoor;
}
