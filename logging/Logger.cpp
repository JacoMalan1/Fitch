#include <iostream>
#include <boost/format.hpp>
#include <sstream>
#include <fstream>
#include <ctime>
#include "Logger.h"

void Logger::addString(const std::string &sender, const std::string &str, unsigned int severity) {

    std::string severities[3] {
        "INFO",
        "WARN",
        "ERROR"
    };

    std::stringstream msg;
    time_t now = time(nullptr);
    tm* ltm = localtime(&now);
    std::stringstream time;
    const std::string& sev = severities[severity];

    time << boost::format("%02d:%02d:%02d") % ltm->tm_hour % ltm->tm_min % ltm->tm_sec;
    msg << boost::format("(%s) { %s }:[%s]: %s") % time.str() % sev % sender % str;

    logList.push_back(msg.str());
    std::fprintf((severity == ERR) ? stderr : stdout, "%s\n", msg.str().c_str());

}

void Logger::addString(const std::string &sender, const std::string &str) { addString(sender, str, INFO); }

void Logger::write(const char *file_path) {

    std::ofstream fs(file_path);

    if (!fs.is_open()) {
        std::fprintf(stderr, "IO Error, couldn't write log!");
        return;
    }

    for (const std::string& str : logList) {
        fs << str << "\n";
    }

    fs.close();

}