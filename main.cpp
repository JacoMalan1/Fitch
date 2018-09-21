#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "game/Player.h"

using namespace glm;

namespace fitch {

    GLFWwindow* window;
    Player player = Player(vec2(0, 0), 200, 400);

    void GLAPIENTRY glCallback( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userParam ) {

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
        (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : "INFO"),
        type, severity, message );

    }

    // Gets run before the main loop starts
    int loadWindow() {

        player.initShaders();
        player.initBuffer();

        Player p = player;

    }

    // Gets run after the main loop stops
    void windowClosing() {

    }

    // Gets run before rendering each frame
    void updateFrame() {

        player.handleInput(window);
        player.update();
        player.resendBuffer();

    }

    // Renders the current frame
    void renderFrame() {

        glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        glClear(GL_COLOR_BUFFER_BIT);
        mat4 projMat = ortho(0.0f, 800.0f, 600.0f, 0.0f);

        player.render(projMat);

    }

    int init() {

        glewExperimental = GL_TRUE;

        if (!glfwInit()) {
            std::fprintf(stderr, "GLFW initialization failed!");
            return -1;
        }

        glfwWindowHint(GLFW_SAMPLES, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

        window = glfwCreateWindow(800, 600, "Fitch", nullptr, nullptr);

        glfwMakeContextCurrent(window);

        if (glewInit() != 0) {
            std::fprintf(stderr, "GLEW initialization failed!");
            return -1;
        }

        loadWindow();

        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);
        while (!glfwWindowShouldClose(window) && glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS) {

            updateFrame();
            renderFrame();

            glfwPollEvents();

        }

        windowClosing();

        return 0;
    }

}

int main() {
    return fitch::init();
}