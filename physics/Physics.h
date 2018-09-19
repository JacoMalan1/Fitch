//
// Created by jacom on 2018/08/20.
//

#ifndef FITCH_PHYSICS_H
#define FITCH_PHYSICS_H

#include "RigidBody.h"

namespace fitch {

    void tickPhysics(RigidBody& body);

    void makeCollide(RigidBody& dynBody, RigidBody& statBody);
    void limitVector(glm::vec2& vector, float mag);
    void limitVector_lin(glm::vec2& vector, float l_mag);

}

#endif //FITCH_PHYSICS_H
