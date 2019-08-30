#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <Box2D/Box2D.h>
#include <boost/format.hpp>
#include "main.h"
#include "game/Block.h"
#include "game/Player.h"
#include "tools.h"
#include "graphics/Mesh.h"
#include "graphics/Rectangle.h"

#define DEBUG false

// Optionally import the ImGUI
// library, only if we are compiling a DEBUG version.
#if DEBUG == true

#include "imgui/imgui.h"
#include "imgui/imgui_impl_glfw.h"
#include "imgui/imgui_impl_opengl3.h"

#endif

namespace fitch {

    // General
    GLFWwindow* window;
    int levelCount = 2;
    float timeStep = 1.0f / 60.0f;
    int width, height;
    bool showDebugWindow = false;
    Logger logger;
//    bool fullscreen = false;

    // Resources
    Texture2D* TEXTURE_SOLID;
    Shader* blockShader;
    Shader* rectShader;

    // Drawables
    Mesh* blockMesh;
    Rectangle* background;
    Player* player;

    // Box2D
    b2Vec2* gravity;
    b2World* world;

    // Lists
    std::vector<Drawable*> drawList;
    std::vector<PhysicsObject*> physicsList;
    std::vector<Block>* blockList;

    Logger& getLogger() { return logger; }

    void GLAPIENTRY glCallback(GLenum, GLenum type, GLuint, GLenum severity, GLsizei, const GLchar* message, const void*) {

        if (type == GL_DEBUG_TYPE_OTHER)
            return;

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
        (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : type == GL_DEBUG_TYPE_PERFORMANCE ? "PERFORMANCE" : "INFO"),
        type, severity, message );

    }

    // Gets run before the main loop starts
    void loadWindow() {

        logger.addString("Main", "Loading window...");

        glEnable(GL_BLEND);
        glEnable(GL_DEPTH_TEST);

        glDepthFunc(GL_GEQUAL);
        glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

        if (getDebug() == Full) {
            glEnable(GL_DEBUG_OUTPUT);
            glDebugMessageCallback(glCallback, nullptr);
        }

        blockMesh = new Mesh();
        gravity = new b2Vec2(0.0f, 2.0f);
        world = new b2World(*gravity);

        // Load and compile the shader for textured rectangles.
        logger.addString("Main", "Compiling shaders...");
        blockShader = new Shader("shaders/tvshader.glsl", "shaders/tfshader.glsl");
        blockShader->compile();
        rectShader = new Shader("shaders/bvshader.glsl", "shaders/tfshader.glsl");
        rectShader->compile();

        // Initialize player object.
        player = new Player(glm::vec2(0, 0), "content/player.png");
        player->initPhysics(world);

        // Load level
        logger.addString("Main", (boost::format("Loading level %d") % levelCount).str());
        std::string sLevel = (std::string)"content/level" + std::to_string(levelCount) + (std::string)".fl";
        blockList = fitchio::loadLevel(sLevel.c_str());

        // Initialize background object.
        glm::vec2 rectPos = player->getPos() - glm::vec2(width / 2, height / 2) + glm::vec2(player->getWidth() / 2, player->getHeight() / 2);
        background = new Rectangle(rectPos.x, rectPos.y, (float)width, (float)height);
        background->setShader(rectShader);
        Texture2D* backgroundTex = fitchio::loadBMP("content/background.png");
        background->setTexture(*backgroundTex);

        // Pre-load solid.png
        TEXTURE_SOLID = fitchio::loadBMP("content/solid.png");

        drawList.reserve(2 + blockList->size());
        physicsList.reserve(1 + blockList->size());

        drawList.push_back(player);
        drawList.push_back(background);
        for (Block& b : *blockList) {

            b.setShader(blockShader);

            if (b.getType() == Solid) {
                b.setTexture(TEXTURE_SOLID);
            } else if (b.getType() == Start) {
                player->setPos(b.screenPos() - glm::vec2(0, player->getHeight() + 100));
                continue;
            }

            blockMesh->addMeshElement(b.getVertices(), 4, 4);
            physicsList.push_back(&b);

        }

        blockMesh->setShader(blockShader);
        blockMesh->setTexture(*TEXTURE_SOLID);
        drawList.push_back(blockMesh);

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

        logger.write("fitch.log");

    }

    void keyCallback(GLFWwindow*, int key, int, int action, int) {

        if (key == GLFW_KEY_F12 && action == GLFW_PRESS) {
            showDebugWindow = !showDebugWindow;
        } else if (key == GLFW_KEY_SPACE && action == GLFW_PRESS) {
            player->getBody()->ApplyForceToCenter(b2Vec2(0, -3), true);
        } else if (key == GLFW_KEY_F11 && action == GLFW_PRESS) {
            glfwSetWindowMonitor(window, glfwGetPrimaryMonitor(), 0, 0, width, height, 60);
        }

    }

    // Gets run before rendering each frame
    void updateFrame() {

        world->Step(timeStep, 6, 6);
        background->setPos(player->getPos() - glm::vec2(width / 2, height / 2) + glm::vec2(player->getWidth() / 2, player->getHeight() / 2));

        if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS && player->getBody()->GetLinearVelocity().x < T_VELOCITY) {
            player->getBody()->ApplyForceToCenter(b2Vec2(0.1, 0), true);
        } else if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS && player->getBody()->GetLinearVelocity().x > -T_VELOCITY) {
            player->getBody()->ApplyForceToCenter(b2Vec2(-0.1, 0), true);
        }

        for (Drawable* drawable : drawList)
            drawable->update();

        background->setPos(player->getPos() - glm::vec2(width / 2, height / 2) + glm::vec2(player->getWidth() / 2, player->getHeight() / 2));
        background->update();

    }

    // Renders the current frame
    void renderFrame() {

        glClearColor(100.0f / 255, 149.0f / 255, 237.0f / 255, 1.0f);
        glClearDepth(0.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        glm::mat4 projMat = glm::ortho(0.0f, (float)width, (float)height, 0.0f);
        glm::mat4 modelMat = glm::mat4(1);
        projMat = translate(projMat, glm::vec3(-player->getPos() + glm::vec2(width / 2, height / 2) - glm::vec2(player->getWidth() / 2, player->getHeight() / 2), 0.0f));
        glm::mat4 viewMat(1);
        glm::mat4 mvp(1);

        mvp *= projMat;
        mvp *= viewMat;
        mvp *= modelMat;

        for (Drawable* drawable : drawList) {
            drawable->setMatrix(mvp);
            drawable->draw();
        }

        if (showDebugWindow) {


            std::string posX = "Player X: " + std::to_string(player->getPos().x);
            std::string posY = "Player Y: " + std::to_string(player->getPos().y);

            std::string bPosX = "Velocity X: " + std::to_string(player->getBody()->GetLinearVelocity().x);
            std::string bPosY = "Velocity Y: " + std::to_string(player->getBody()->GetLinearVelocity().y);

#if DEBUG == true
            ImGui_ImplOpenGL3_NewFrame();
            ImGui_ImplGlfw_NewFrame();
            ImGui::NewFrame();

            ImGui::Begin("Debug", nullptr, ImVec2(400, 200), 0.9f, ImGuiWindowFlags_MenuBar);

            ImGui::Text("%s\n", posX.c_str());
            ImGui::Text("%s\n", posY.c_str());
            ImGui::Text("%s\n", bPosX.c_str());
            ImGui::Text("%s\n", bPosY.c_str());

            ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);

            ImGui::End();

            ImGui::Render();
            ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
#endif

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
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

        const GLFWvidmode* mode = glfwGetVideoMode(glfwGetPrimaryMonitor());
        width = mode->width;
        height = mode->height;
        window = glfwCreateWindow(width, height, "Fitch", nullptr, nullptr);
        glfwMaximizeWindow(window);
        glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
        glfwSetWindowSizeCallback(window, [](GLFWwindow* win, int x, int y) {

            glfwGetFramebufferSize(win, &width, &height);
            glViewport(0, 0, width, height);

        });

        glfwMakeContextCurrent(window);
        glfwSwapInterval(1);

        glViewport(0, 0, width, height);

        std::string version = (char*)glGetString(GL_VERSION);
        std::string glslVersion = (char*)glGetString(GL_SHADING_LANGUAGE_VERSION);
        logger.addString("Main", (boost::format("OpenGL Version: %s") % version).str());
        logger.addString("Main", (boost::format("GLSL Version: %s") % glslVersion).str());

#if DEBUG == true

        ImGui::CreateContext();
        ImGuiIO& io = ImGui::GetIO();

        ImGui::StyleColorsDark();
        ImGui_ImplGlfw_InitForOpenGL(window, false);
        ImGui_ImplOpenGL3_Init("#version 330");

#endif

        if (glewInit() != 0) {
            std::fprintf(stderr, "%s\n", "GLEW initialization failed!");
            return -1;
        }

        loadWindow();

        double lastTime = glfwGetTime();
        double fps = 120;
        double maxPeriod = 1 / fps;

        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);
	    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
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

bool fitch::getDebug() { return DEBUG; }

int main(int argc, const char** argv) {
    return fitch::init();
}
