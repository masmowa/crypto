// AnalyseMessage.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <list>
#include <vector>
#include <regex>
#include <algorithm>
#include <ctime>
#include <iomanip>

#include <standard-include.h>
#include "CypherKey.h"

#include "EnCharFrequency.h"
#include "CharPtFreqCtFreq.h"
#include "CharFreqNode.h"
#include "MessageInfo.h"

using namespace std;

typedef std::map<char, int> CharCount;
typedef std::map<char, CharPtFreqCtFreq> CharFreqMap;
typedef std::map<char, int> CharCountMap;
typedef std::pair<char, int> PairCharCount;
typedef std::vector<std::string> VWords;
typedef std::map<std::string, int> WordCountMap;
typedef std::pair<std::string, CharCountMap> PairWordToCharCountMap;
typedef std::map<std::string, CharCountMap> MapWordToCharCountMap;




std::string ReadInput(std::string const& input)
{
	efs::path inpath(input.c_str());
	if (Verbose())
	{
		std::cout << input << std::endl;
		//std::cout << "CWD: " << efs::current_path() << std::endl;
		demo_exists(inpath);
	}
	if (!get_exists(inpath))
	{
		std::cout << "File: " << input.c_str() << " not found" << std::endl;
	}
	// get problem name
	std::ifstream fin(inpath);
	std::string line;
	std::getline(fin, line);
	string problemName = line;
	// get input ciphertext
	std::getline(fin, line);
	vector<string> intoks = tokenize(line, ':');
	for (size_t i = 0; i < intoks.size(); ++i)
	{
		cout << intoks[i] << std::endl;
	}


	return intoks[1];
}

void GetCharCountMapFromWord(const std::string& val, CharCountMap& ccMap)
{
	size_t wdsz = val.length();
	for (size_t i = 0; i < wdsz; ++i)
	{
		CharCountMap::iterator itc = ccMap.find(val[i]);
		if (itc != ccMap.end())
		{
			itc->second++;
		}
		else
		{
			ccMap.insert(PairCharCount(val[i], 1));
		}
	}
}

void PrintLNCharCount(size_t len, const std::string & word, CharCountMap& ccm, std::string score)
{
	std::cout << "word [" << word << "] length[" << word.length()  << "] score:"<< score<< ": ";
	for (CharCountMap::iterator itccm = ccm.begin(); itccm != ccm.end(); ++itccm)
	{
		std::cout << "[" << itccm->first << " " << itccm->second << "] ";
	}
	std::cout << std::endl;
}

MessageInfo_base::MapWordPair MakeWordScoreMap(const MessageInfo_base::ListWords& listWord, size_t wdLen, const std::string& title)
{
	MessageInfo_base::MapWordPair res;
	MessageInfo_base::PrintWordsFromList(title, listWord);
	for (MessageInfo_base::ListWords::const_iterator itcwd = listWord.begin(); itcwd != listWord.end();  ++itcwd)
	{
		CharCountMap tmpMap;
		std::string score;
		GetCharCountMapFromWord(*itcwd, tmpMap);
		score = MessageInfo_base::WordScore(*itcwd);
		res.insert(std::pair<std::string, std::string>(*itcwd, score));
		PrintLNCharCount(wdLen, *itcwd, tmpMap, score);
		//top3CtMapWdToCCL4.insert(PairWordToCharCountMap(*itcwd, tmpMap));
	}


	return res;
}

MessageInfo_base::ListWords GuessPTFromCTScore(const std::string& word, const std::string& score, const MessageInfo_base::MapWordPair& mapWordScorePt, const std::string& CT)
{
	MessageInfo_base::ListWords listWords;
	//MessageInfo_base::MapWordPair::const_iterator itpt = 
	for (MessageInfo_base::MapWordPair::const_iterator it = mapWordScorePt.begin(); it != mapWordScorePt.end(); ++it)
	{
		if (it->second == score)
		{
			listWords.push_back(it->first);
		}
	}
	return listWords;
}

// for words in WordScoreCt, to words i mapWordScorePt whos scores match
MessageInfo_base::MapCTtoPTList MakeSubstListFromWordScores(const MessageInfo_base::MapWordPair& mapWordScoreCt, 
	const MessageInfo_base::MapWordPair& mapWordScorePt, 
	const MessageInfo_base::ListWords& wordList, 
	MessageInfo& ct, 
	EnglishData& eng)
{
	MessageInfo_base::MapCTtoPTList mapCttoPtList;
	// holds guess of word
	MessageInfo_base::ListWords listWords;
	std::vector<CypherKey> rgGuessKeys;

	// find PT words with scores that match CT
	for (MessageInfo_base::ListWords::const_iterator itct = wordList.begin(); itct != wordList.end(); ++itct)
	{

		MessageInfo_base::MapWordPair::const_iterator itCtScore = mapWordScoreCt.find(*itct);	// .second;
		MessageInfo_base::ListWords guesses = 
			GuessPTFromCTScore(*itct, itCtScore->second, mapWordScorePt, ct.msg);
		size_t count = 0;
		for (MessageInfo_base::ListWords::iterator ptIt = guesses.begin(); ptIt != guesses.end(); ++ptIt, ++count) {
			// todo: create a copy of the cipher message,
			//std::string msgcpy = ct.msg;

			//std::string PT = *it;
			//std::transform(PT.begin(), PT.end(), PT.begin(), ::toupper);

			std::string PTGuess;
			if (rgGuessKeys.size() != 0)
			{
				bool keyUpdated = false;
				// cycle through keys add new guess
				for (std::vector<CypherKey>::iterator ckit = rgGuessKeys.begin(); ckit != rgGuessKeys.end(); ++ckit)
				{

					if (ckit->SetKeyValues(itCtScore->first, *ptIt))
					{
						// no collision
						keyUpdated = true;
						break;
					}
					else
					{
						// check next entry
						continue;
					}

				}
				if (!keyUpdated)
				{
					std::stringstream ssn;
					ssn << "KeyGuess(" << count << ")";
					// no guesses yet add the first cypher key guess
					CypherKey gk(ssn.str());
					// add new key-guess with some substitutions
					gk.SetKeyValues(*itct, *ptIt);
					gk.PrintKey();

					// replace CT with PT
					PTGuess = gk.Decipher(ct.msg);

					rgGuessKeys.push_back(gk);

				}
			}
			else
			{
				std::stringstream ssn;
				ssn << "KeyGuess(" << count << ")";
				// no guesses yet add the first cypher key guess
				CypherKey gk(ssn.str());
				// add new key-guess with some substitutions
				gk.SetKeyValues(*itct, *ptIt);
				gk.PrintKey();

				// replace CT with PT
				PTGuess = gk.Decipher(ct.msg);

				rgGuessKeys.push_back(gk);
			}

			// print message
			std::cout << PTGuess << std::endl;
		}
	}
	return mapCttoPtList;
}
void DirectWordGuessing(MessageInfo& ct, EnglishData& eng)
{
	size_t wantWordSize = 4;

	MapWordToCharCountMap ctMapWdToCCL4;
	MessageInfo_base::MapWordPair mapWordScoreCtL4;
	MessageInfo_base::MapWordPair mapWordScoreCtL3;
	MessageInfo_base::MapWordPair mapWordScorePtL4;
	MessageInfo_base::MapWordPair mapWordScorePtL3;
	MessageInfo_base::ListWords listCtWordsL4 = ct.GetWordListAt(4);
	mapWordScoreCtL4 = MakeWordScoreMap(listCtWordsL4, 4, "[CIPHER TEXT L4]");

	MessageInfo_base::ListWords listPtWordsL4 = eng.GetWordListAt(4);
	mapWordScorePtL4 = MakeWordScoreMap(listPtWordsL4, 4, "[PLAIN TEXT L4]");

	MakeSubstListFromWordScores(mapWordScoreCtL4, mapWordScorePtL4, listCtWordsL4, ct, eng);

	MapWordToCharCountMap top3CtMapWdToCCL3;
	MessageInfo_base::ListWords listCtWordsL3 = ct.GetWordListAt(3);
	mapWordScoreCtL3 = MakeWordScoreMap(listCtWordsL3, 3, "[CIPHER TEXT L3]");

	//VWords top3PtwordsL3 = eng.GetTop3PTWords(3);
	CharCountMap top3PtL3ccm;
	//MapWordToCharCountMap top3PtMapWdToCCL3;
	MessageInfo_base::ListWords listPtWordsL3 = eng.GetWordListAt(3);
	mapWordScorePtL3 = MakeWordScoreMap(listPtWordsL3, 3, "[PLAIN TEXT L3]");

	// now find a match of top score CT to PT
}

void WriteDTSOutputName(const std::string& prefix)
{
	std::time_t t = std::time(nullptr);
	std::tm tm = { 0 };
	errno_t err = localtime_s(&tm, &t);
	char mbstr[100] = { 0 };


	if (std::strftime(mbstr, sizeof(mbstr), "%Ym%d-%H%M%S", &tm)) {
		std::cout << mbstr << '\n';
	}
	stringstream ssname;
	if (prefix.empty()) {
		ssname << "output-" << mbstr << ".txt";// << std::endl;
	}
	else {
		ssname << prefix << "-output-" << mbstr << ".txt";
	}
	std::cout << ssname.str() << std::endl;
}
void ProcessInput(std::string const& input)
{
	string ct = ReadInput(input);
	MessageInfo msgInfo(ct);
	EnglishData english;

	WriteDTSOutputName("");
	//msgInfo.printCharCount();
	//msgInfo.printWordCount();
	msgInfo.CalcCharPercent();
	msgInfo.CalcWordPercent();
	std::cout << "Percent " << std::endl;
	//msgInfo.printCharPercent();
	//msgInfo.printWordPercent();
	msgInfo.PrintWordsBySizePercent();

	//msgInfo.GuessWordWithNChar(4, english.vlWordsByFreq);
	//msgInfo.GuessWordsFromNCharWidth(4, english.vlWordsByFreq);
	DirectWordGuessing(msgInfo, english);
}
int main()
{
    //std::cout << "Hello World!\n";
	for (auto& p : efs::directory_iterator("input"))
	{
		string ps = p.path().string();
		ProcessInput(ps);
	}
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
