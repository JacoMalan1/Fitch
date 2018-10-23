//
// Created by jacom on 2018/09/18.
//

#ifndef FITCH_MAIN_H
#define FITCH_MAIN_H

#define LEVEL_SIZE_X 250
#define LEVEL_SIZE_Y 50

#include "game/Block.h"

enum DebugMode {
    Full,
    Off
};

namespace fitch {

    DebugMode getDebug();
    void renderFrame();

}

#endif //FITCH_MAIN_H
