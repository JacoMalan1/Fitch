#ifndef FITCH_PHYSICSOBJECT_H
#define FITCH_PHYSICSOBJECT_H

#include <Box2D/Dynamics/b2World.h>

class PhysicsObject {

public:
    virtual void initPhysics(b2World* world) = 0;

};

#endif //FITCH_PHYSICSOBJECT_H
