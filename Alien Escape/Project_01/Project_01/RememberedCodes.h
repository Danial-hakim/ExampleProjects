#pragma once
#include <vector>
#include <iostream>

class RememberedCodes
{
private:
	static std::vector<std::string> m_knownCodes;

public:
	static void learnCode(std::string t_newCode);
	static bool openDoorAttempt(std::string t_wantedCode);
};

