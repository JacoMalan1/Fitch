#ifndef FITCH_MAIN_H
#define FITCH_MAIN_H

#include <glm/glm.hpp>
#include "logging/Logger.h"
#include "game/Block.h"

#define T_VELOCITY 1.5f

enum DebugMode {
    Full,
    Off
};

namespace fitch {

    bool getDebug();
    void renderFrame();
    Logger& getLogger();

}

#endif //FITCH_MAIN_H
