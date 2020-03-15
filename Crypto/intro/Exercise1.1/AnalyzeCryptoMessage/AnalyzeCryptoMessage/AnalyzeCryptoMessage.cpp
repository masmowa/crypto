// AnalyzeCryptoMessage.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <list>
#include <vector>
#include <regex>
#include <algorithm>
#include <ctime>
#include <iomanip>
#include <iostream>

#include <filesystem>

#include <standard-include.h>
#include "ProgramSettings.h"
#include "CipherText.h"
#include "EnglishText.h"
#include "CypherKey.h"

using namespace std;

std::string ReadInput(std::string const& input)
{
	efs::path inpath(input.c_str());
	//if (Verbose())
	//{
	//	std::cout << input << std::endl;
	//	//std::cout << "CWD: " << efs::current_path() << std::endl;
	//	demo_exists(inpath);
	//}
	if (!get_exists(inpath))
	{
		std::cout << "File: " << input.c_str() << " not found" << std::endl;
	}
	// get problem name
	std::ifstream fin(inpath, std::ifstream::in);
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

void WriteDTSOutputName(const std::string& prefix)
{
	std::time_t t = std::time(nullptr);
	std::tm tm = { 0 };
	errno_t err = localtime_s(&tm, &t);
	char mbstr[100] = { 0 };


	if (std::strftime(mbstr, sizeof(mbstr), "%Y%m%d-%H%M%S", &tm)) {
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

void DecryptMessage(CipherText& CT, EnglishText& eng)
{
	std::cout << "++" << __FUNCTION__ << "()" << std::endl;
	static int sizeOrder[] = { 1, 3, 2, 4 };
	// guess key
	CypherKey key("Guess0");
	// Pre-guess words with 1 char, there are 2

	// todo: make decipher into aset of trial attempts 
	// with a prompt to ask if the current guess is acceptable
	// if it is not, pop the last guess off the stack
	// 

	for (size_t cc = 0; cc < 4; ++cc)
	{
		int sel = sizeOrder[cc];
		MessageBase::VectorWords ctWdLN;
		MessageBase::VectorWords ptWdLN;
		if (sel == 1)
		{
			ctWdLN = CT.vectorWordSizeVectorWords[sel];
			ptWdLN = eng.monoWdByFreq;
		}
		else
		{
			ctWdLN = CT.GetFirstNofWordSize(2, sel);
			ptWdLN = eng.GetFirstNofWordSize(2, sel);
		}
		for (size_t z = 0; z < ctWdLN.size(); ++z)
		{
			std::cout << "CT[" << ctWdLN[z] << "] guess PT[" << ptWdLN[z] << "]\n";
		}
		key.SetKeyValues(ctWdLN, ptWdLN);
		// after we set some guesses, 
		// perform some attempt at decryption with the 
		// current partial key.  show the message and the 
		// partially decrypted message to the user
		// prompt for pass / fail judgement
		// if pass then procede to next set of guesses
		std::cout << std::endl;
	}
	key.SetKeyValuesFromCtCharCount(CT.vectorCharCount, eng.alphaByFreq);
	key.PrintKey();
	CT.PrintCharByFreq();
	eng.PrintCharByFreq();
	std::cout << std::endl;
	std::string decrypted = key.Decipher(CT.message);
	std::cout << "Decryption guess 0: " << std::endl;
	std::cout << decrypted << std::endl;

	std::cout << "--" << __FUNCTION__ << "()" << std::endl;
}
void ProcessInput(std::string const& input)
{
	WriteDTSOutputName("DecryptMessage");
	string ct = ReadInput(input);
	CipherText msgCt(ct);
	EnglishText english;

	std::cout << "Percent " << std::endl;

	msgCt.PrintWordsBySizeCount();

	DecryptMessage(msgCt, english);
}

int main()
{
	for (auto& p : efs::directory_iterator("input"))
	{
		string ps = p.path().string();
		ProcessInput(ps);
	}
}
