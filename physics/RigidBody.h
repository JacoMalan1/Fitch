#ifndef FITCH_RIGIDBODY_H
#define FITCH_RIGIDBODY_H

#include <vector>
#include <glm/glm.hpp>
#include "Rectangle2D.h"
#include "PhysicsBody.h"
#include "../graphics/Renderable.h"
#include "../graphics/Shader.h"
#include "../graphics/VBO.h"
#include "../graphics/VAO.h"

enum CollisionType {
    All,
    Specific
};

class RigidBody : PhysicsBody, Renderable {

private:
    glm::vec2 position;
    glm::vec2 velocity;
    glm::vec2 acceleration;
    float width, height;
    float mass;
    bool p = false;
    Rectangle2D collisionBox;
    std::vector<RigidBody>* collisionList;
    CollisionType collisionType;

    Shader shader;
    VBO buffer;
    VAO vertexArray;

public:
    RigidBody(float x, float y, float width, float height, float mass, CollisionType collisionType);
    RigidBody(const RigidBody& other);
    ~RigidBody();

    glm::vec2 getVelocity() override;
    glm::vec2 getPosition() override;
    void setPosition(glm::vec2 position) override;
    void setVelocity(glm::vec2 velocity) override;
    float getMass();
    Rectangle2D getCBox() override;
    void addCollider(RigidBody& collider);
    void update() override;
    std::vector<RigidBody>* getCollisionList();

    void initBuffer() override;
    void resendBuffer() override;
    void initShaders() override;
    void render() override;
    void render(const glm::mat4& projMat);

    void registerRenderer();

};

#endif //FITCH_RIGIDBODY_H
