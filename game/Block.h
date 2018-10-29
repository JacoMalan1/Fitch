#define BLOCK_SIZE 50
#ifndef BLOCK_H
#define BLOCK_H
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "../physics/RigidBody.h"
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../graphics/Shader.h"
#include "../graphics/Texture2D.h"

enum BlockType {
    Solid,
    Air,
    Start,
    Powerup
};

std::ostream& operator<<(std::ostream& stream, BlockType type);

class Block : Renderable {

private:
    glm::vec2 position;
    BlockType type;
    Texture2D texture;

    VAO vertexArray;
    VBO buffer;
    Shader* shader;
    bool collected = false; // TODO: Implement powerups.

public:
    Block(glm::vec2 position, BlockType type);
    Block(glm::vec2 position, BlockType type, const char* texture_path);

    RigidBody* asRBody();
    static int getSize();
    void initBuffer() override;
    void resendBuffer() override;
    void render() override;
    void render(const glm::mat4& projMat);
    void initShaders() override;
    bool isRenderable();
    glm::vec2 getPos();

    void setTexture(Texture2D texture);
    void setShader(Shader* shader);
    BlockType getType();

};

#endif // BLOCK_H
