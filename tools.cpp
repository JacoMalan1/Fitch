#include <iostream>
#include <sstream>
#include <fstream>
#include "tools.h"
#include "lodepng.h"
#include "main.h"

#define SCALING_FACTOR 400

namespace fitchio {

    std::optional<std::string*> loadFile(const char *file_path) {

        std::ifstream fs(file_path);

        if (!fs.is_open()) {
            fprintf(stderr, "Error: Couldn't open file stream.");
            return {};
        }

        std::stringstream sstr;
        sstr << fs.rdbuf();
        auto contents = new std::string(sstr.str());

        return contents;

    }

    Texture2D* loadBMP(const char* image_path) {

        Texture2D* ret = nullptr;

        std::vector<unsigned char> png;
        std::vector<unsigned char> image;
        unsigned width, height;

        unsigned error = lodepng::load_file(png, image_path);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return new Texture2D();
        }

        error = lodepng::decode(image, width, height, png);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return new Texture2D;
        }

        GLuint textureID;
        glGenTextures(1, &textureID);
        glBindTexture(GL_TEXTURE_2D, textureID);

        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, image.data());
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);

        ret = new Texture2D(textureID, (int)width, (int)height);

        return ret;

    }

    std::vector<std::string> splitString(const std::string& str, char delim) {

        std::vector<std::string> result = std::vector<std::string>();
        std::string temp;

        for (char c : str) {

            if (c != delim) {
                temp += c;
            } else {
                result.emplace_back(temp);
                temp = "";
            }

        }
        result.emplace_back(temp);

        return result;

    }

    std::vector<Block>* loadLevel(const char* file_path) {

        auto data = loadFile(file_path);

        if (!data)
            return nullptr;

        auto s_contents = *data.value();
        std::vector<std::string> lines = splitString(s_contents, '\n');

        auto blocks = new std::vector<Block>();
        blocks->reserve(lines.size());

        for (const std::string& line : lines) {

            if (line.empty())
                continue;

            std::vector<std::string> params = splitString(line, ',');

            BlockType type;

            if (params[0] == "solid") {
                type = Solid;
            } else if (params[0] == "start") {
                type = Start;
            } else {
                type = Air;
            }

            int x = 0;
            int y = 0;

            std::stringstream stream(params[1]);
            stream >> x;
            stream = std::stringstream(params[2]);
            stream >> y;

            if (type != Air) {

                blocks->emplace_back(glm::vec2(x, y), type);

            }

        }

        return blocks;

    }

}

float fitchtools::pixToWorld(float pix) {
    return pix / SCALING_FACTOR;
}

float fitchtools::worldToPix(float world) {
    return world * SCALING_FACTOR;
}

b2Vec2 fitchtools::pixToWorld(glm::vec2 pix) {
    return b2Vec2(fitchtools::pixToWorld(pix.x), fitchtools::pixToWorld(pix.y));
}

glm::vec2 fitchtools::worldToPix(b2Vec2 world) {
    return glm::vec2(fitchtools::worldToPix(world.x), fitchtools::worldToPix(world.y));
}
