#include <iostream>
#include <sstream>
#include <cstring>
#include <fstream>
#include <memory>
#include "tools.h"
#include "lodepng.h"
#include "main.h"

using namespace std;

#define SCALING_FACTOR 400

namespace fitchio {

    const char* loadFile(const char *file_path) {

        std::ifstream fs(file_path);

        if (!fs.is_open()) {
            fprintf(stderr, "Error!");
            return "";
        }

        stringstream sstr;
        sstr << fs.rdbuf();
        string contents = sstr.str();

        auto retVal = new char[contents.length() + 1];
        strcpy(retVal, contents.c_str());

        return retVal;

    }

    unsigned char* loadImage(const char* image_path, int* w, int* h) {

        std::vector<unsigned char> png;
        std::vector<unsigned char> image;
        unsigned width, height;

        unsigned error = lodepng::load_file(png, image_path);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return nullptr;
        }

        error = lodepng::decode(image, width, height, png);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return nullptr;
        }

        *w = width;
        *h = height;

        return image.data();

    }

    Texture2D loadBMP(const char* image_path) {

        std::vector<unsigned char> png;
        std::vector<unsigned char> image;
        unsigned width, height;

        unsigned error = lodepng::load_file(png, image_path);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return { 0, 0, 0 };
        }

        error = lodepng::decode(image, width, height, png);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return { 0, 0, 0 };
        }

        GLuint textureID;
        glGenTextures(1, &textureID);
        glBindTexture(GL_TEXTURE_2D, textureID);

        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, image.data());
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);

        return { textureID, (int)width, (int)height };

    }

    vector<string> splitString(const string& str, char delim) {

        vector<string> result = vector<string>();
        string temp;

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

        const char* contents = loadFile(file_path);
        std::string s_contents = contents;
        delete[] contents;
        vector<string> lines = splitString(s_contents, '\n');

        auto blocks = new std::vector<Block>();
        blocks->reserve(lines.size());

        for (const string& line : lines) {

            if (line == "")
                continue;

            vector<string> params = splitString(line, ',');

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

            stringstream stream(params[1]);
            stream >> x;
            stream = stringstream(params[2]);
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
