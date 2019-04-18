#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <cstring>
#include <Box2D/Box2D.h>
#include "game/Block.h"
#include "game/Player.h"
#include "tools.h"
#include "graphics/Mesh.h"
#include "imgui/imgui.h"
#include "imgui/imgui_impl_glfw.h"
#include "imgui/imgui_impl_opengl3.h"

using namespace glm;
DebugMode debugMode = Off;

namespace fitch {

    GLFWwindow* window;
    Player* player;
    Texture2D TEXTURE_SOLID;
    Shader* blockShader;
    int levelCount = 3;
    std::vector<Drawable*> drawList;
    std::vector<PhysicsObject*> physicsList;
    std::vector<Block>* blockList;
    Mesh* blockMesh;
    b2Vec2 gravity;
    b2World* world;

    float timeStep = 1.0f / 60.0f;

    int width, height;

    bool showDebugWindow = true;

    glm::vec2 getWindowDims() {
        return glm::vec2(width, height);
    }

    void GLAPIENTRY glCallback( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userParam ) {

        if (type == GL_DEBUG_TYPE_OTHER)
            return;

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
        (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : type == GL_DEBUG_TYPE_PERFORMANCE ? "PERFORMANCE" : "INFO"),
        type, severity, message );

    }

    // Gets run before the main loop starts
    void loadWindow() {

        glEnable(GL_BLEND);
        glEnable(GL_DEPTH_TEST);

        glDepthFunc(GL_LEQUAL);
        glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

        if (getDebug() == Full) {
            glEnable(GL_DEBUG_OUTPUT);
            glDebugMessageCallback(glCallback, nullptr);
        }

        blockMesh = new Mesh();
        gravity = b2Vec2(0.0f, 2.0f);
        world = new b2World(gravity);

        // Load and compile the shader for textured rectangles.
        blockShader = new Shader("shaders/tvshader.glsl", "shaders/tfshader.glsl");
        blockShader->compile();

        // Initialize player object.
        player = new Player(vec2(0, 0), "content/player.png");
        player->initPhysics(world);
        std::string sLevel = (std::string)"content/level" + std::to_string(levelCount) + (std::string)".fl";
        blockList = fitchio::loadLevel(sLevel.c_str());

        // Pre-load solid.png
        TEXTURE_SOLID = fitchio::loadBMP("content/solid.png");

        drawList.reserve(1 + blockList->size());
        physicsList.reserve(1 + blockList->size());

        drawList.emplace_back((Drawable*)player);
        for (Block& b : *blockList) {

            b.setShader(blockShader);

            if (b.getType() == Solid) {
                b.setTexture(TEXTURE_SOLID);
            } else if (b.getType() == Start) {
                player->setPos(b.screenPos() - vec2(0, player->getHeight() + 100));
                continue;
            }

            blockMesh->addMeshElement(b.getVertices(), 4, 4);
            physicsList.emplace_back((PhysicsObject*)&b);

        }

        blockMesh->setShader(blockShader);
        blockMesh->setTexture(TEXTURE_SOLID);
        drawList.emplace_back((Drawable*)blockMesh);

        for (Drawable* drawable : drawList)
            drawable->init();

        for (PhysicsObject* body : physicsList)
            body->initPhysics(world);

    }

    // Gets run after the main loop stops
    void windowClosing() {

        delete blockShader;
        delete blockList;
        delete player;

    }

    void keyCallback(GLFWwindow* window, int key, int scancode, int action, int mods) {

        if (key == GLFW_KEY_F12 && action == GLFW_PRESS) {
            showDebugWindow = !showDebugWindow;
        } else if (key == GLFW_KEY_SPACE && action == GLFW_PRESS) {
            player->getBody()->ApplyLinearImpulse(b2Vec2(0, -0.03), player->getBody()->GetWorldCenter(), true);
        }

    }

    // Gets run before rendering each frame
    void updateFrame() {

        for (Drawable* drawable : drawList)
            drawable->update();

        world->Step(timeStep, 6, 6);

    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS) {
        player->getBody()->ApplyForce(b2Vec2(0.5, 0), player->getBody()->GetWorldCenter(), true);
    } else if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS) {
        player->getBody()->ApplyForce(b2Vec2(-0.5, 0), player->getBody()->GetWorldCenter(), true);
    }

    }

    // Renders the current frame
    void renderFrame() {

        glClearColor(100.0f / 255, 149.0f / 255, 237.0f / 255, 1.0f);
        glClearDepth(1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        if (showDebugWindow) {
    	    ImGui_ImplOpenGL3_NewFrame();
            ImGui_ImplGlfw_NewFrame();
            ImGui::NewFrame();

            std::string posX = "Player X: " + std::to_string(player->getPos().x);
            std::string posY = "Player Y: " + std::to_string(player->getPos().y);

            std::string bPosX = "Velocity X: " + std::to_string(player->getBody()->GetLinearVelocity().x);
            std::string bPosY = "Velocity Y: " + std::to_string(player->getBody()->GetLinearVelocity().y);

            ImGui::Begin("Debug", nullptr, ImVec2(400, 200), 0.9f, ImGuiWindowFlags_MenuBar);

            ImGui::Text("%s\n", posX.c_str());
            ImGui::Text("%s\n", posY.c_str());
            ImGui::Text("%s\n", bPosX.c_str());
            ImGui::Text("%s\n", bPosY.c_str());

            ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);

            ImGui::End();

            ImGui::Render();
            ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

        }

        mat4 projMat = ortho(0.0f, (float)width, (float)height, 0.0f);
        mat4 modelMat = mat4(1);
        projMat = translate(projMat, vec3(-player->getPos() + vec2(width / 2, height / 2) - vec2(player->getWidth() / 2, player->getHeight() / 2), 0.0f));
        mat4 viewMat(1);
        mat4 mvp(1);

        mvp *= projMat;
        mvp *= viewMat;
        mvp *= modelMat;

        for (Drawable* drawable : drawList) {
            drawable->setMatrix(mvp);
            drawable->draw();
        }

        glfwSwapBuffers(window);

    }

    int init() {

        glewExperimental = GL_TRUE;

        if (!glfwInit()) {
            std::fprintf(stderr, "%s\n", "GLFW initialization failed!");
            return -1;
        }

        glfwWindowHint(GLFW_SAMPLES, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

        width = 800;
        height = 600;
        window = glfwCreateWindow(width, height, "Fitch", nullptr, nullptr);

        glfwMakeContextCurrent(window);
        glfwSwapInterval(1);

        ImGui::CreateContext();
        ImGuiIO& io = ImGui::GetIO();

        ImGui::StyleColorsDark();
        ImGui_ImplGlfw_InitForOpenGL(window, false);
        ImGui_ImplOpenGL3_Init("#version 330");

        if (glewInit() != 0) {
            std::fprintf(stderr, "%s\n", "GLEW initialization failed!");
            return -1;
        }

        loadWindow();

        double lastTime = glfwGetTime();
        double fps = 120;
        double maxPeriod = 1 / fps;
        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);
        glfwSetKeyCallback(window, keyCallback);

        // Start main game loop
        while (!glfwWindowShouldClose(window) && glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS) {

            double time = glfwGetTime();
            double deltaTime = time - lastTime;

            // Insure that the update function only gets called 60 times per second.
            if (deltaTime >= maxPeriod) {

                updateFrame();
                lastTime = time;

            }

            renderFrame();
            glfwPollEvents();

        }

        windowClosing();

        return 0;
    }

}

DebugMode fitch::getDebug() { return debugMode; }

int main(int argc, const char** argv) {

    if (argc > 0 && fitch::getDebug() == Off) {
        if (strncmp(argv[0], "--debug", strlen(argv[0])) != 0 ||
            strncmp(argv[0], "-d", strlen(argv[0])) != 0) {
            debugMode = Full;
        }
    }

    return fitch::init();
}
