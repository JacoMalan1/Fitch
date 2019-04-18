#ifndef BLOCK_H
#define BLOCK_H

#define BLOCK_SIZE 50

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <memory>
#include <Box2D/Dynamics/b2Body.h>
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../graphics/Shader.h"
#include "../graphics/Texture2D.h"
#include "../graphics/Drawable.h"
#include "PhysicsObject.h"

enum BlockType {
    Solid,
    Air,
    Start
};

std::ostream& operator<<(std::ostream& stream, BlockType type);

class Block : Drawable, PhysicsObject {

private:
    glm::vec2 position;
    BlockType type;
    Texture2D texture;

    VAO vao;
    VBO vbo;
    Shader* shaderProgram;

    glm::mat4 drawMat;

    b2Body* physicsBody;
    b2World* physicsWorld;

public:
    Block(glm::vec2 position, BlockType type);
    Block(glm::vec2 position, BlockType type, const char* texture_path);
    static int getSize();
    bool isRenderable() const;
    glm::vec2 getPos() const;
    std::shared_ptr<float[]> getVertices() const;

    glm::vec2 screenPos();

    void setTexture(Texture2D texture);
    void setShader(Shader* shader);
    BlockType getType() const;

    b2Body* getBody() { return physicsBody; }
    void setBody(b2Body* body) { physicsBody = body; }

    void setMatrix(glm::mat4 mat) override;

    void initPhysics(b2World* world) override;
    void init() override;
    void update() override;
    void draw() override;

};

#endif // BLOCK_H
