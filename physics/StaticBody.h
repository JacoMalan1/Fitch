#ifndef STATICBODY_H
#define STATICBODY_H

#include "PhysicsBody.h"
#include <glm/glm.hpp>

class StaticBody : PhysicsBody
{

private:
    glm::vec2 position;
    float width, height;

public:
    StaticBody(glm::vec2 position, float width, float height);
    ~StaticBody();

    Rectangle2D getCBox() override;
    glm::vec2 getPosition() override;
    void setPosition(glm::vec2 position) override;
    void update() override;

};

#endif // STATICBODY_H
