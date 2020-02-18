// CountLetterFrequency.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <standard-include.h>

class CharFrequency {
public:
	map<char, double> english_freq;
	CharFrequency()
	{
		english_freq = {
			std::pair<unsigned char, double>('a', 0.0817),
			std::pair<unsigned char, double>('b', 0.0150),
			std::pair<unsigned char, double>('c', 0.0278),
			std::pair<unsigned char, double>('d', 0.0425),
			std::pair<unsigned char, double>('e', 0.1270),
			std::pair<unsigned char, double>('f', 0.0223),
			std::pair<unsigned char, double>('g', 0.0202),
			std::pair<unsigned char, double>('h', 0.0609),
			std::pair<unsigned char, double>('i', 0.0697),
			std::pair<unsigned char, double>('j', 0.0015),
			std::pair<unsigned char, double>('k', 0.0077),
			std::pair<unsigned char, double>('l', 0.0403),
			std::pair<unsigned char, double>('m', 0.0241),
			std::pair<unsigned char, double>('n', 0.0675),
			std::pair<unsigned char, double>('o', 0.0751),
			std::pair<unsigned char, double>('p', 0.0193),
			std::pair<unsigned char, double>('q', 0.0010),
			std::pair<unsigned char, double>('r', 0.0599),
			std::pair<unsigned char, double>('s', 0.0633),
			std::pair<unsigned char, double>('t', 0.0906),
			std::pair<unsigned char, double>('u', 0.0276),
			std::pair<unsigned char, double>('v', 0.0098),
			std::pair<unsigned char, double>('w', 0.0236),
			std::pair<unsigned char, double>('x', 0.0015),
			std::pair<unsigned char, double>('y', 0.0197),
			std::pair<unsigned char, double>('z', 0.0007)
		};
	}
};

void ProcessInput(std::string const& input)
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
	typedef std::map<char, int> CharCount;
	CharCount ccmap;
	CharCount::iterator cci;
	string ct = intoks[1];
	for (size_t i = 0; i < ct.size(); ++i)
	{
		cci = ccmap.find(ct[i]);
		if (cci != ccmap.end())
		{
			cci->second++;
		}
		else
		{
			ccmap.insert(std::pair<char, int>(ct[i], 1));
		}
	}
	cout << "Letter Count " << std::endl;
	for (CharCount::iterator t = ccmap.begin(); t != ccmap.end(); ++t)
	{
		cout << t->first << " " << t->second << std::endl;
	}
	// ****************
	// find the frequency of each char in the cipher text
	// ****************
	typedef map<char, double> CipherTextFreqCountMap;
	CipherTextFreqCountMap ctfMap;

	size_t ctcount = ct.size();
	for (auto itccm = ccmap.begin(); itccm != ccmap.end(); ++itccm)
	{
		double freq = static_cast<double>(itccm->second) / static_cast<double>(ctcount);
		ctfMap.insert(pair<char, double>(itccm->first, freq));
	}
	for (CipherTextFreqCountMap::iterator t = ctfMap.begin(); t != ctfMap.end(); ++t)
	{
		cout << t->first << " " << t->second << std::endl;
	}
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
