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

	for (size_t i = 0; i < CT.size(); ++i)
	{
		if (key[CT[i]] != '?')
		{
			std::cout << "guess: CT:[" << CT[i] << "] Eng: [" << key[CT[i]] << "] eng guess: [" << PT[i] << "]" << std::endl;
			continue;
		}
		bool found = KeyHasPtVal(PT[i]);

		// prevent duplicate guess 
		if (!found) {
			key[CT[i]] = PT[i];
		}
	}
	return (!hasCollision);
}

bool CypherKey::SetKeyValues(const std::vector<std::string>& CT, const std::vector<std::string>& PT)
{
	bool hasCollision = false;
	size_t len = CT.size();
	size_t wdLen = CT[0].length();
	// for each word in the set
	// get the word
	for (size_t i = 0; i < len; ++i) {
		std::string wdct = CT[i];
		std::string wdpt = PT[i];
		hasCollision = SetKeyValues(wdct, wdpt);
	}
	return (!hasCollision);

}

// here we swap the order of CT to PT map to see if this is a better fit (probably not)
bool CypherKey::SetKeyValuesRev(const std::vector<std::string>& CT, const std::vector<std::string>& PT)
{
	bool hasCollision = false;
	size_t len = CT.size();
	size_t wdLen = CT[0].length();
	// for each word in the set
	// get the word
	size_t i = len - 1;
	size_t j = 0;
	while (i >= 0 && j < len)
	{
		std::string wdct = CT[i];
		std::string wdpt = PT[j];
		hasCollision = SetKeyValues(wdct, wdpt);
		--i;
		++j;
	}
	//for (size_t i = len-1, j=0; i >= 0 && j < len; --i, ++j) {
	//	std::string wdct = CT[i];
	//	std::string wdpt = PT[j];
	//	hasCollision = SetKeyValues(wdct, wdpt);
	//}
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


// match Cyper-Text to Plain-Text based on "frequency" from frequency sorted vectors
bool CypherKey::SetKeyValuesFromCtCharCount(const MessageBase::VectorCharCount vCT, const std::vector<char>& PT)
{
	bool hasCollision = false;
	// TODO: Add your implementation code here.
	for (size_t i=0, j=0; i < vCT.size() && j < PT.size(); ++i, ++j)
	{
		if (vCT[i].second < 10)
		{
			break;
		}
		// check if this PT value is already in the key
		bool ptValFound = KeyHasPtVal(PT[j]);
		if (key[vCT[i].first] != '?')
		{
			std::cout << "key: " << vCT[i].first << " val: " << key[vCT[i].first] << " eng guess: " << PT[j] << std::endl;
			// if key.english-value does not matche the PT[j] value
			// and if j > 0 
			// use this PT[j] value in the next guess.
			//if (key[vCT[i].first] != PT[j] && (j > 0))
			//{
			//	j--;
			//}
			continue;
		}
		if (!ptValFound) {
			key[vCT[i].first] = PT[j];
		}
	}
	return hasCollision;
}


bool CypherKey::KeyHasPtVal(const char guess)
{
	bool found = false;
	for (MapKey::iterator itk = key.begin(); itk != key.end() & !found; ++itk)
	{
		found = (itk->second == guess);
	}
	return found;
}
