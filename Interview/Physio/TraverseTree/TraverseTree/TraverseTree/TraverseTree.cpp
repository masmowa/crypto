// TraverseTree.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <vector>
#include <stack>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Tree.h"

bool traverseTree(node *root);

int _tmain(int argc, _TCHAR* argv[])
{
	node *root = (node *)NULL;
	createTree(&root);

	traverseTree(root);

	removeTree(&root);

	getchar();

	return 0;
}

/*
	Given the root of a tree, print out the nodes, one level at a time, traversing from right to left, to left to right.

	Example: 

               1 <--
              / \
             /   \
		-->	2     3
           / \   / \
          7   6 5   4 <--

	Output: 1, 2, 3, 4, 5, 6, 7

	/*** FOR REFERENCE ***
struct node
{
	int data;
	node *left;
	node *right;
};
 *** FOR REFERENCE ***/

void printdata(node * n)
{
	if (n != NULL) 
		std::cout << n->data;
}

bool traverseTree(node *root)
{
	std::stack<node*> left;
	std::stack<node*> right;
	bool done = false;
	bool leftToRight = true;
	bool rightToLeft = false;
	if (root != NULL) {
		left.push(root);
		while (!done) {
			if (!left.empty() && leftToRight)
			{
				node * n = left.top();
				std::cout << n->data;
				if (n->right != NULL) right.push(n->right);
				if (n->left != NULL) right.push(n->left);
				left.pop();
				leftToRight = !left.empty();
				rightToLeft = !leftToRight;
			}
			else if (!right.empty() && rightToLeft) {
				node * n = right.top();
				std::cout << n->data;
				if (n->left != NULL) left.push(n->left);
				if (n->right != NULL) left.push(n->right);
				right.pop();
				rightToLeft = !right.empty();
				leftToRight = !rightToLeft;
				
			}
			std::cout << " ";
			done = (right.empty() && left.empty());
		}
		/*
		if (right % 2 == 0) {
			traverseTree(current->right);
			traverseTree(current->left);
		}
		else {
			traverseTree(current->left);
			traverseTree(current->right);
		}
		*/
	}
	return true;
}

/* masmowa@hotmail.com */