//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_PHYSICSBODY_H
#define FITCH_PHYSICSBODY_H
#include "Rectangle2D.h"
#include <glm/glm.hpp>

class PhysicsBody {

public:
    PhysicsBody();

    virtual Rectangle2D getCBox() = 0;
    virtual glm::vec2 getPosition() = 0;
    virtual void setPosition(glm::vec2 position) = 0;
    virtual void update() = 0;

};


#endif //FITCH_PHYSICSBODY_H
