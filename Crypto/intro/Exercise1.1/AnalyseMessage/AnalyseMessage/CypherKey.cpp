#include "CypherKey.h"
bool CypherKey::KeyCollision(const std::string& CT, const std::string& PT)
{
	bool hasCollision = false;
	for (size_t i = 0; i < CT.size(); ++i)
	{
		hasCollision = (key[CT[i]] == PT[i]);

		if (hasCollision)
		{
			break;
		}
	}
	return hasCollision;
}

bool CypherKey::SetKeyValues(const std::string& CT, const std::string& PT)
{
	bool hasCollision = KeyCollision(CT, PT);

	if (!hasCollision)
	{
		for (size_t i = 0; i < CT.size(); ++i)
		{
			key[CT[i]] = PT[i];
		}
	}
	return (!hasCollision);
}

// send the key contents to std::cout
void CypherKey::PrintKey()
{
	// 
	std::cout << name << std::endl;
	for (MapKey::iterator it = key.begin(); it != key.end(); ++it)
	{
		std::cout << "[" << it->first << ", " << it->second << "]" << std::endl;
	}
}



// Create a new string with cipher-text replaced by plain-text
std::string CypherKey::Decipher(const std::string& CT)
{
	std::stringstream ssPt;
	for (std::string::const_iterator ctit = CT.begin(); ctit != CT.end(); ++ctit)
	{
		if (' ' == *ctit)
		{
			ssPt << *ctit;
			continue;
		}
		else if ('?' == key[*ctit])
		{
			// no Plain-Text character
			ssPt << *ctit;
			continue;
		}
		ssPt << static_cast<char>(::toupper(key[*ctit]));

	}
	return ssPt.str();
}
