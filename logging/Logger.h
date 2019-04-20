#ifndef FITCH_LOGGER_H
#define FITCH_LOGGER_H

#include <vector>
#include <string>

enum LogSeverity {

    INFO = 0u,
    WARN = 1u,
    ERR = 2u

};

class Logger {

private:
    std::vector<std::string> logList;

public:
    Logger() = default;

    void addString(const std::string& sender, const std::string& str);
    void addString(const std::string& sender, const std::string& str, unsigned int severity);
    void write(const char* file_path);

};


#endif //FITCH_LOGGER_H
