#pragma once
#include <string>
#include <list>
#include <vector>
#include <map>
#include <algorithm>
#include <sstream>


class MessageInfo_base
{
public:
	typedef std::map<char, double> CharFreqPercentMap;
	typedef std::map<std::string, double> WordFreqPercentMap;
	typedef std::map<char, int> CharCountMap;
	typedef std::map<std::string, int> WordCountMap;

	typedef std::map<std::string, std::string> MapWordPair;
	typedef MessageInfo_base::MapWordPair MapCTtoPT;
	typedef MessageInfo_base::MapWordPair CipherToPlain;	// hold guess for decryption from cipher-text to english-text

	typedef std::pair<std::string, double> PairWordFreq;
	typedef std::pair<std::string, std::string> PairWordWord;

	typedef std::list<PairWordFreq> ListWordFreq;
	typedef std::list<PairWordFreq> ListWordData;

	typedef std::list<std::string> ListWords;
	typedef std::vector<std::string> VWords;

	typedef std::list < ListWordFreq> ListListWordFreq;
	typedef std::vector< ListWordFreq > VectorListWordFreq;
	typedef std::vector< ListWords> VectorListWordByFreq;
	typedef std::map<std::string, ListWords> MapCTtoPTList;

	static map<char, int> CountChars(const std::string& word)
	{
		CharCountMap::iterator cci;
		CharCountMap mpChCt;
		for (size_t i = 0; i < word.size(); ++i)
		{
			cci = mpChCt.find(word[i]);
			if (cci != mpChCt.end())
			{
				cci->second++;
			}
			else
			{
				mpChCt.insert(std::pair<char, int>(word[i], 1));
			}
		}
		return mpChCt;
	}

	// calculate a "word score" for the string
	// word score is a count of the occurance of eachcharacter in the word
	// count position is based upon the initial position of the character
	// ex. 
	//	"the"   : "111"
	//	"that"  : "211" 
	//  "this"  : "1111"
	//  "data"  : "121"
	static std::string WordScore(const std::string& word)
	{
		std::string score;
		std::stringstream sss;
		std::stringstream seen; // attempt to filter out characters counted > 1 times
		CharCountMap mapChCt = MessageInfo_base::CountChars(word);
		size_t wordLen = word.length();

		for (size_t i = 0; i < word.size(); ++i) {
			CharCountMap::iterator cci = mapChCt.find(word[i]);
			size_t seenPos = 0;
			bool charSeen = false;
			// need to filter out double char items like aa ee ll tt
			// expecially in the middle of words this is resulting 
			// in score of 122 for been, good, cwhh, drry, ...
			if ((seen.str().length() > 0)) {
				char tc = word[i];
				std::string s = seen.str();
				seenPos = s.find(tc, 0);
				charSeen = (seenPos != std::string::npos);
			}
			if (cci != mapChCt.end() && !charSeen)
			{
				sss << cci->second;
				if (1 < cci->second)
				{
					seen << cci->first;
				}
			}
		}
		return sss.str();
	}

	static void PrintWordsFromVector(const std::string& msg, const VWords& wds)
	{
		std::cout << msg << std::endl;
		size_t cols = 0;
		for (size_t i = 0; i < wds.size(); ++i)
		{
			std::cout << wds[i] << " ";
			cols = wds[i].size() + 1;
			if (76 <= cols) {
				std::cout << std::endl;
				cols = 0;
			}
		}
		std::cout << std::endl;
	}

	static void PrintWordsFromList(const std::string& msg, const std::list<std::string>& wds)
	{
		std::cout << msg << std::endl;
		size_t cols = 0;

		for (std::list<std::string>::const_iterator i= wds.begin(); i != wds.end(); ++i)
		{
			std::cout << *i << " ";
			cols = (*i).size() + 1;
			if (76 <= cols) {
				std::cout << std::endl;
				cols = 0;
			}
		}
		std::cout << std::endl;
	}

};

