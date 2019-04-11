//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_TOOLS_H
#define FITCH_TOOLS_H

#include <string>
#include <vector>
#include "graphics/Texture2D.h"
#include "game/Block.h"

namespace fitchio {

    const char* loadFile(const char* file_path);
    Texture2D loadBMP(const char* image_path);
    std::vector<std::string> splitString(const std::string& str, char delim);
    std::vector<Block>* loadLevel(const char* file_path);

}

namespace fitchtools {

    float pixToWorld(float pix);
    float worldToPix(float world);

}

#endif //FITCH_TOOLS_H
