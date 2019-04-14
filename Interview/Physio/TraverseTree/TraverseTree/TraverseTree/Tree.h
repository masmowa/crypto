#pragma once

#include <iostream>
#include <vector>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

struct node
{
	int data;
	node *left;
	node *right;
};

bool createTree(node **root);
bool printNode(node *node);
bool removeTree(node **root);


class Tree
{
public:
	Tree();
	~Tree();
};



