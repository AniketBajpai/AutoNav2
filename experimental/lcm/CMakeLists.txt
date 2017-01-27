cmake_minimum_required(VERSION 3.0.1)

project(lcm-test)

#########################################################
set(CMAKE_CXX_FLAGS "-Wall -O3 -march=native")
#########################################################
# FIND C++11 Support
#########################################################
include(CheckCXXCompilerFlag)
CHECK_CXX_COMPILER_FLAG("-std=c++11" COMPILER_SUPPORTS_CXX11)
CHECK_CXX_COMPILER_FLAG("-std=c++0x" COMPILER_SUPPORTS_CXX0X)
if(COMPILER_SUPPORTS_CXX11)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")
elseif(COMPILER_SUPPORTS_CXX0X)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++0x")
else()
        message(STATUS "The compiler ${CMAKE_CXX_COMPILER} has no C++11 support. Please use a different C++ compiler.")
endif()

#########################################################
# FIND Packages
#########################################################
find_package(PkgConfig REQUIRED)
find_package(OpenCV REQUIRED)
pkg_search_module(LCM REQUIRED lcm)
#########################################################
# Include Files
#########################################################
include_directories(${OpenCV_INCLUDE_DIRS} ${LCM_INCLUDE_DIRS} /usr/local/include include)

add_executable(lcm-test src/sender.cpp)

target_link_libraries(lcm-test ${OpenCV_LIBS} ${LCM_LIBRARIES})
