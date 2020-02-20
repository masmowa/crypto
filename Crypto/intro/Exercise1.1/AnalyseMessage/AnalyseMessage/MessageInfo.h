#pragma once
#include <string>
#include <list>
#include <vector>
#include <map>

#include "CharFreqNode.h"
#include "parseUtil.h"

class MessageInfo
{
public:
	typedef std::map<char, double> CharFreqPercentMap;
	typedef std::map<std::string, double> WordFreqPercentMap;
	typedef std::map<char, int> CharCountMap;
	typedef std::map<std::string, int> WordCountMap;

	std::string msg;
	std::vector<string> words;

	std::list<CharFreqNode> cfList;
	CharCountMap charCount;
	CharFreqPercentMap charPercentMap;
	WordCountMap wordCount;
	WordFreqPercentMap wordPercentMap;
	
	MessageInfo(std::string m)
		: msg(m)
	{
		CountChars();
		CountWords();
	}

	void CountChars()
	{
		CharCountMap::iterator cci;
		for (size_t i = 0; i < msg.size(); ++i)
		{
			cci = charCount.find(msg[i]);
			if (cci != charCount.end())
			{
				cci->second++;
			}
			else
			{
				charCount.insert(std::pair<char, int>(msg[i], 1));
			}
		}
	}
	void CountWords()
	{
		words = tokenize(msg, ' ');
		WordCountMap::iterator itwd;
		for (size_t i = 0; i < words.size(); ++i)
		{
			itwd = wordCount.find(words[i]);
			if (itwd != wordCount.end())
			{
				itwd->second++;
			}
			else
			{
				wordCount.insert(std::pair<std::string, int>(words[i], 1));
			}
		}

	}
	void printCharCount()
	{
		std::cout << "Letter Count " << std::endl;
		for (CharCountMap::iterator itccm = charCount.begin(); itccm != charCount.end(); ++itccm)
		{
			std::cout << itccm->first << " " << itccm->second << std::endl;
		}

	}
	void printWordCount()
	{
		std::cout << "Word Count " << std::endl;
		for (WordCountMap::iterator it = wordCount.begin(); it != wordCount.end(); ++it)
		{
			std::cout << it->first << " " << it->second << std::endl;
		}

	}
	void CalcCharPercent()
	{
		size_t ctcount = charCount.size();
		CharFreqPercentMap::iterator itfc;
		for (CharCountMap::iterator itccm = charCount.begin(); itccm != charCount.end(); ++itccm)
		{
			double freq = static_cast<double>(itccm->second) / static_cast<double>(ctcount);
			charPercentMap.insert(std::pair<char,double>(itccm->first, freq));
		}
	}
	void CalcWordPercent()
	{
		size_t wdcount = wordCount.size();
		WordFreqPercentMap::iterator itwfc;
		for (WordCountMap::iterator itwcm = wordCount.begin(); itwcm != wordCount.end(); ++itwcm)
		{
			double freq = static_cast<double>(itwcm->second) / static_cast<double>(wdcount);
			wordPercentMap.insert(std::pair<std::string, double>(itwcm->first, freq));
		}
	}
	void printCharPercent()
	{
		std::cout << "Letter Percent " << std::endl;
		;
		for (CharFreqPercentMap::iterator itcfp = charPercentMap.begin(); itcfp != charPercentMap.end(); ++itcfp)
		{
			std::cout << itcfp->first << " " << itcfp->second << std::endl;
		}

	}
	void printWordPercent()
	{
		std::cout << "Word Percent " << std::endl;
		for (WordFreqPercentMap::iterator itwfc = wordPercentMap.begin(); itwfc != wordPercentMap.end(); ++itwfc)
		{
			std::cout << itwfc->first << " " << itwfc->second << std::endl;
		}
	}
};

