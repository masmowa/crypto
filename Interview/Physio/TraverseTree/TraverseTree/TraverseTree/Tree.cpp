#include "Tree.h"


Tree::Tree()
{
}


Tree::~Tree()
{
}


bool createTree(node **root)
{
	int value = 0;
	// Level 1 (right to left)
	(*root) = (node *)malloc(sizeof(node));
	memset((*root), 0, sizeof(node));
	(*root)->data = value++;

	// Level 2 (left to right)
	(*root)->left = (node *)malloc(sizeof(node));
	(*root)->left->data = value++;
	(*root)->right = (node *)malloc(sizeof(node));
	(*root)->right->data = value++;

	// Level 3 (right to left)
	(*root)->right->right = (node *)malloc(sizeof(node));
	(*root)->right->right->data = value++;
	(*root)->right->left = (node *)malloc(sizeof(node));
	(*root)->right->left->data = value++;

	(*root)->left->right = (node *)malloc(sizeof(node));
	(*root)->left->right->data = value++;
	(*root)->left->left = (node *)malloc(sizeof(node));
	(*root)->left->left->data = value++;

	// Level 4 (left to right)
	(*root)->left->left->left = (node *)malloc(sizeof(node));
	memset((*root)->left->left->left, 0, sizeof(node));
	(*root)->left->left->left->data = value++;
	(*root)->left->left->right = (node *)malloc(sizeof(node));
	memset((*root)->left->left->right, 0, sizeof(node));
	(*root)->left->left->right->data = value++;

	(*root)->left->right->left = (node *)malloc(sizeof(node));
	memset((*root)->left->right->left, 0, sizeof(node));
	(*root)->left->right->left->data = value++;
	(*root)->left->right->right = (node *)malloc(sizeof(node));
	memset((*root)->left->right->right, 0, sizeof(node));
	(*root)->left->right->right->data = value++;

	(*root)->right->left->left = (node *)malloc(sizeof(node));
	memset((*root)->right->left->left, 0, sizeof(node));
	(*root)->right->left->left->data = value++;
	(*root)->right->left->right = (node *)malloc(sizeof(node));
	memset((*root)->right->left->right, 0, sizeof(node));
	(*root)->right->left->right->data = value++;

	(*root)->right->right->left = (node *)malloc(sizeof(node));
	memset((*root)->right->right->left, 0, sizeof(node));
	(*root)->right->right->left->data = value++;
	(*root)->right->right->right = (node *)malloc(sizeof(node));
	memset((*root)->right->right->right, 0, sizeof(node));
	(*root)->right->right->right->data = value++;

	return true;
}

bool printNode(node *node)
{
	if (node == NULL)
	{
		return false;
	}

	printf("node = %i\n", node->data);
	printNode(node->left);
	printNode(node->right);
	return true;
}

bool removeTree(node **root)
{
	free((*root)->left->left->left);
	free((*root)->left->left->right);
	free((*root)->left->right->left);
	free((*root)->left->right->right);
	free((*root)->right->left->left);
	free((*root)->right->left->right);
	free((*root)->right->right->left);
	free((*root)->right->right->right);
	free((*root)->right->right);
	free((*root)->right->left);
	free((*root)->left->right);
	free((*root)->left->left);
	free((*root)->left);
	free((*root)->right);
	free(*root);

	*root = NULL;

	return true;
}
