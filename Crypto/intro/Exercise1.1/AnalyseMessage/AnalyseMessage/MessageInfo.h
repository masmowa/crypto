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
	typedef std::pair<std::string, double> WordData;
	typedef std::list<WordData> WordFreqList;
	typedef std::list<WordData> ListWordData;
	typedef std::list < std::list<WordData> > ListListWordFreq;
	typedef std::vector< WordFreqList > VectorListWordFreq;


	std::string msg;
	std::vector<string> words;

	std::list<CharFreqNode> cfList;
	CharCountMap charCount;
	CharFreqPercentMap charPercentMap;
	WordCountMap wordCount;
	WordFreqPercentMap wordPercentMap;
	VectorListWordFreq wordsByCharCount;
	ListListWordFreq wordsListsByFreq;
	
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
		size_t wdSizeCount = 0;			// count 
		// there should be 0 words with 0 characters so, initialize element 0 with 0 element
		WordFreqPercentMap::iterator itwfc;

		for (WordCountMap::iterator itwcm = wordCount.begin(); itwcm != wordCount.end(); ++itwcm)
		{
			double freq = static_cast<double>(itwcm->second) / static_cast<double>(wdcount);
			WordData data(itwcm->first, freq);
			if (itwcm->first == "")
			{
				WordFreqList v;
				v.push_back(data);
				if (wordsByCharCount.empty()) {
					wordsByCharCount.push_back(v);
				}
				else {
					wordsByCharCount[0] = v;
				}
			}
			else {
				wordPercentMap.insert(data);
				size_t pos = data.first.size();
				if (pos >= wordsByCharCount.size()) {
					wordsByCharCount.resize(pos + 1);
				}
				wordsByCharCount[pos].push_back(data);
			}
		}
		VectorListWordFreq::iterator itVecListWF;
		WordFreqList::iterator itListWF;
		for (VectorListWordFreq::iterator itvlwf = wordsByCharCount.begin(); itvlwf != wordsByCharCount.end(); ++itvlwf) {
			(*itvlwf).sort([](const WordData& a, const WordData& b) { return (a.second < b.second); });
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
	void SeperateWordsByCharacterCount()
	{
		for (WordFreqPercentMap::iterator itwfc = wordPercentMap.begin(); itwfc != wordPercentMap.end(); ++itwfc)
		{
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
	void PrintWordsBySizePercent()
	{
		for (VectorListWordFreq::iterator itvlwf = wordsByCharCount.begin(); itvlwf != wordsByCharCount.end(); ++itvlwf)
		{
			for (ListWordData::iterator itlwd = (*itvlwf).begin(); itlwd != (*itvlwf).end(); ++itlwd) {
				if (itlwd->first == "") {
					std::cout << "[empty]" << "[" << itlwd->second << "]" << std::endl;
				}
				else {
					std::cout << "[" << itlwd->first << "]" << "[" << itlwd->second << "]" << std::endl;
				}
			}
		}
	}
};

