#define BLOCK_SIZE 50
#ifndef BLOCK_H
#define BLOCK_H

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <memory>
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../graphics/Shader.h"
#include "../graphics/Texture2D.h"
#include "../graphics/Drawable.h"

enum BlockType {
    Solid,
    Air,
    Start
};

std::ostream& operator<<(std::ostream& stream, BlockType type);

class Block : Drawable {

private:
    glm::vec2 position;
    BlockType type;
    Texture2D texture;

    VAO vao;
    VBO vbo;
    Shader* shaderProgram;

    glm::mat4 drawMat;

public:
    Block(glm::vec2 position, BlockType type);
    Block(glm::vec2 position, BlockType type, const char* texture_path);
    static int getSize();
    bool isRenderable() const;
    glm::vec2 getPos() const;

    glm::vec2 screenPos();

    void setTexture(Texture2D texture);
    void setShader(Shader* shader);
    BlockType getType() const;

    void setMatrix(glm::mat4 mat) override;

    void init() override;
    void update() override;
    void draw() override;

};

#endif // BLOCK_H
