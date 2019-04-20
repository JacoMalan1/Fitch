#ifndef FITCH_RECTANGLE_H
#define FITCH_RECTANGLE_H

#include "Drawable.h"
#include "Texture2D.h"
#include "Shader.h"
#include "VBO.h"
#include "VAO.h"

class Rectangle : public Drawable {

private:
    glm::vec2 m_Pos;
    float m_Width;
    float m_Height;

    glm::vec4 m_Color;
    bool m_UseText = false;
    Texture2D m_Texture;
    glm::mat4 m_DrawMat;

    Shader* m_Shader;
    VBO m_VBO;
    VAO m_VAO;

public:

    Rectangle(float x, float y, float width, float height);

    void setColor(glm::vec4 color);
    void setTexture(Texture2D texture);
    void setShader(Shader* shader);
    void setPos(glm::vec2 pos);
    glm::vec2 getPos() const;

    void setMatrix(glm::mat4 mat) override;
    void init() override;
    void update() override;
    void draw() override;

};


#endif //FITCH_RECTANGLE_H
