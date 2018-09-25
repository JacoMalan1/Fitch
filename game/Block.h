#define BLOCK_SIZE 50
#ifndef BLOCK_H
#define BLOCK_H
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "../physics/RigidBody.h"
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../graphics/Shader.h"

enum BlockType {
    Solid,
    Air
};

class Block : Renderable {

private:
    glm::vec2 position;
    BlockType type;

    VAO vertexArray;
    VBO buffer;
    Shader shader;

public:
    Block(glm::vec2 position, BlockType type);

    RigidBody* asRBody();
    static int getSize();
    void initBuffer() override;
    void resendBuffer() override;
    void render() override;
    void render(const glm::mat4& projMat);
    void initShaders() override;

};

#endif // BLOCK_H
