#pragma once
#include <iostream>
#include <string>
#include <vector>

#include <filesystem>
namespace efs = std::filesystem;

std::string ltrim(const std::string &);
std::string rtrim(const std::string &);
std::vector<std::string> split(const std::string &);

std::vector<std::string> tokenize(std::string const &line, char delim);
std::vector<int> tokenize_to_int(std::string const &line, char delim);
void demo_exists(efs::path& p, efs::file_status s = efs::file_status{});
bool read_array_lines(efs::path& p, std::vector<std::string>& result);

std::vector<std::string> split_string(std::string);


inline bool Verbose() {
#if defined(MAS_TEST)
	return true;
#else
	return false;
#endif
}

template <class T>
void print_vector(std::vector<T>const & vec)
{
	if (Verbose()) {
		for (size_t i = 0; i < vec.size(); ++i)
		{
			std::cout << vec[i];
			if (i <= (vec.size() - 1)) {
				std::cout << " ";
			}
		}
		std::cout << std::endl;
	}
}

template <typename T>
void print_2d_vector(std::vector < std::vector<T> >const & a2d)
{
	typename std::vector<T>::iterator a2di_it;

	typename std::vector < std::vector<T> >::const_iterator a2d_it;

	for (a2d_it = a2d.begin(); a2d_it != a2d.end(); ++a2d_it)
	{
		print_vector(*a2d_it);
	}
}

inline
bool get_exists(efs::path& p, efs::file_status s = efs::file_status{}) {
	return efs::status_known(s) ? efs::exists(s) : efs::exists(p);
}