cmake_minimum_required(VERSION 3.0.1)

project(angle_estimation)

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
find_package(OpenCV REQUIRED)
# find_package(gflags REQUIRED)
#########################################################
# Include Files
#########################################################
include_directories(${OpenCV_INCLUDE_DIRS}
					 include
					 ../undistort/include
					 /usr/include/eigen3)

# add_library(helpers-cc /home/quantumcoder/Documents/kg-BTP/slam/src/triangulation.cc
# 					   /home/quantumcoder/Documents/kg-BTP/slam/src/estimate_twoview_info.cc
# 					   /home/quantumcoder/Documents/kg-BTP/slam/src/correspondance.cc
#                        /home/quantumcoder/Documents/kg-BTP/slam/src/helpers.cc)
add_library(correspondance-cc src/correspondance.cc)

# add_executable(angle_estimation_5pt src/find_deviation_angle.cpp)
# add_executable(find_trajectory src/find_trajectory.cpp)
add_executable(trajectory_tracker src/trajectory_tracker.cpp)

# target_link_libraries(angle_estimation_5pt ${OpenCV_LIBS} helpers-cc)
# target_link_libraries(find_trajectory ${OpenCV_LIBS})
target_link_libraries(trajectory_tracker ${OpenCV_LIBS} correspondance-cc)
