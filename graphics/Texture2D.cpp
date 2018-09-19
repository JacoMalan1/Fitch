//
// Created by jacom on 2018/08/22.
//

#include "Texture2D.h"

Texture2D::Texture2D() : ID(0), width(0), height(0) {}
Texture2D::Texture2D(GLuint ID, int width, int height) : ID(ID), width(width), height(height) {}