//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_TOOLS_H
#define FITCH_TOOLS_H

#include "graphics/Texture2D.h"
#include "game/Block.h"
#include <string>

namespace fitchio {

    const char* loadFile(const char* file_path);
    Texture2D loadBMP(const char* image_path);
    std::vector<std::string> splitString(const std::string& str, char delim);
    std::vector<Block>* loadLevel(const char* file_path);

}

#endif //FITCH_TOOLS_H
