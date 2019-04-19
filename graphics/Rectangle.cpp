#include "Rectangle.h"

Rectangle::Rectangle(float x, float y, float width, float height) : m_Pos(glm::vec2(x, y)), m_Width(width), m_Height(height),
    m_Color(glm::vec4(1, 1, 1, 1)), m_DrawMat(glm::mat4(1)), m_Texture(Texture2D(0, 0, 0)) {

    m_VAO = VAO::create();
    m_VBO = VBO::create(GL_ARRAY_BUFFER);
    m_Shader = new Shader(0);

}

void Rectangle::setTexture(Texture2D texture) {
    m_Texture = texture;
    m_UseText = true;
}

void Rectangle::setMatrix(glm::mat4 mat) { m_DrawMat = mat; }
void Rectangle::setColor(glm::vec4 color) { m_Color = color; }
void Rectangle::setShader(Shader *shader) { m_Shader = shader; }
void Rectangle::setPos(glm::vec2 pos) { m_Pos = pos; }
glm::vec2 Rectangle::getPos() const { return m_Pos; }

void Rectangle::init() {

    if (!m_Shader->isCompiled())
        m_Shader->compile();

    m_VAO.bind();
    m_VBO.bind();

    if (m_UseText) {

        std::shared_ptr<float[]> vertices(new float[16] {
                m_Pos.x, m_Pos.y, 0, 0,
                m_Pos.x, m_Pos.y + m_Height, 0, 1,
                m_Pos.x + m_Width, m_Pos.y + m_Height, 1, 1,
                m_Pos.x + m_Width, m_Pos.y, 1, 0,
        });

        m_VBO.sendData(vertices, 16, GL_DYNAMIC_DRAW);

    } else {

        std::shared_ptr<float[]> vertices(new float[8] {
                m_Pos.x, m_Pos.y,
                m_Pos.x + m_Width, m_Pos.y,
                m_Pos.x, m_Pos.y + m_Height,
                m_Pos.x + m_Width, m_Pos.y + m_Height,
        });

        m_VBO.sendData(vertices, 8, GL_DYNAMIC_DRAW);

    }

}

void Rectangle::update() {

    m_VAO.bind();
    m_VBO.bind();

    if (m_UseText) {

        std::shared_ptr<float[]> vertices(new float[16] {
                m_Pos.x, m_Pos.y, 0, 0,
                m_Pos.x + m_Width, m_Pos.y, 1, 0,
                m_Pos.x, m_Pos.y + m_Height, 0, 1,
                m_Pos.x + m_Width, m_Pos.y + m_Height, 1, 1,
        });

        m_VBO.sendData(vertices, 16, GL_DYNAMIC_DRAW);

    } else {

        std::shared_ptr<float[]> vertices(new float[8] {
                m_Pos.x, m_Pos.y,
                m_Pos.x + m_Width, m_Pos.y,
                m_Pos.x, m_Pos.y + m_Height,
                m_Pos.x + m_Width, m_Pos.y + m_Height,
        });

        m_VBO.sendData(vertices, 8, GL_DYNAMIC_DRAW);

    }

}

void Rectangle::draw() {

    m_VAO.bind();
    m_VBO.bind();
    m_Shader->bind();

    GLint MatrixID = glGetUniformLocation(m_Shader->getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &m_DrawMat[0][0]);

    if (m_UseText) {

        m_Texture.bind();

        glEnableVertexAttribArray(0);
        glEnableVertexAttribArray(1);

        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
        glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

        glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

        glDisableVertexAttribArray(0);
        glDisableVertexAttribArray(1);

    } else {

        glEnableVertexAttribArray(0);

        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), nullptr);

        glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

        glDisableVertexAttribArray(0);

    }

}
