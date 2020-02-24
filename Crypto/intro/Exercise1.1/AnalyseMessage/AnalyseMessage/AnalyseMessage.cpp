// AnalyseMessage.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <list>
#include <vector>

#include <standard-include.h>
#include "EnCharFrequency.h"
#include "CharPtFreqCtFreq.h"
#include "CharFreqNode.h"
#include "MessageInfo.h"

using namespace std;

typedef std::map<char, int> CharCount;
typedef std::map<char, CharPtFreqCtFreq> CharFreqMap;

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
void ProcessInput(std::string const& input)
{
	string ct = ReadInput(input);
	MessageInfo msgInfo(ct);

	msgInfo.printCharCount();
	msgInfo.printWordCount();
	msgInfo.CalcCharPercent();
	msgInfo.CalcWordPercent();
	std::cout << "Percent " << std::endl;
	msgInfo.printCharPercent();
	msgInfo.printWordPercent();
	msgInfo.PrintWordsBySizePercent();

}
int main()
{
    std::cout << "Hello World!\n";
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
