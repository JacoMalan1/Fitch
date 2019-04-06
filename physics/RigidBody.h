#ifndef FITCH_RIGIDBODY_H
#define FITCH_RIGIDBODY_H

#include <vector>
#include <glm/glm.hpp>
#include "Rectangle2D.h"
#include "PhysicsBody.h"
#include "../graphics/Shader.h"
#include "../graphics/VBO.h"
#include "../graphics/VAO.h"
#include <memory>

enum CollisionType {
    All,
    Specific
};

class RigidBody : PhysicsBody {

private:
    glm::vec2 position;
    glm::vec2 velocity;
    glm::vec2 acceleration;
    float width, height;
    float mass;
    bool p = false;
    Rectangle2D collisionBox;
    std::shared_ptr<std::vector<RigidBody>> collisionList;
    CollisionType collisionType;

    std::shared_ptr<Shader> shader;
    VBO buffer;
    VAO vertexArray;

public:
    RigidBody(float x, float y, float width, float height, float mass, CollisionType collisionType);
    RigidBody(const RigidBody& other);
    ~RigidBody();

    glm::vec2 getPosition() override;
    void setPosition(glm::vec2 position) override;
    float getMass();
    Rectangle2D getCBox() override;
    void addCollider(RigidBody& collider);
    void update() override;
    std::shared_ptr<std::vector<RigidBody>> getCollisionList();
    void setVelocity(glm::vec2 velocity);
    glm::vec2 getVelocity();

};

#endif //FITCH_RIGIDBODY_H
