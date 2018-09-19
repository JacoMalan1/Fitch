//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_TOOLS_H
#define FITCH_TOOLS_H

#include "graphics/Texture2D.h"

namespace fitchio {

    const char* loadFile(const char* file_path);
    Texture2D loadBMP(const char* image_path);

}

#endif //FITCH_TOOLS_H
