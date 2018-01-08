cmake_minimum_required (VERSION 3.5)
set(CMAKE_CONFIGURATION_TYPES Debug Release)
project (BoggleTest)

get_filename_component(ProjectDirectory ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

set(LibraryDirectory "${ProjectDirectory}/../boggle_library")

link_directories("${LibraryDirectory}/binary/")

file(GLOB Sources "${ProjectDirectory}/source/*.cpp")
file(GLOB Headers "${ProjectDirectory}/include/*.h")

source_group(IncludeFiles FILES ${Headers})
source_group(SourceFiles FILES ${Sources})
	
add_executable(BoggleTest ${Headers} ${Sources})
add_dependencies(BoggleTest BoggleLibrary)
target_link_libraries(BoggleTest debug BoggleLibrary_Debug.lib optimized BoggleLibrary.lib)
	
include_directories("${ProjectDirectory}/include"
					"${LibraryDirectory}/include")

#ADD_MSVC_PRECOMPILED_HEADER("pchboggletest.h" "pchboggletest.cpp" Sources)
					
set_target_properties(BoggleTest PROPERTIES RUNTIME_OUTPUT_DIRECTORY_DEBUG "${ProjectDirectory}/binary")
set_target_properties(BoggleTest PROPERTIES RUNTIME_OUTPUT_DIRECTORY_RELEASE "${ProjectDirectory}/binary")
set_target_properties(BoggleTest PROPERTIES ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${ProjectDirectory}/binary")
set_target_properties(BoggleTest PROPERTIES ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${ProjectDirectory}/binary")
set_target_properties(BoggleTest PROPERTIES OUTPUT_NAME_RELEASE "BoggleTest")
set_target_properties(BoggleTest PROPERTIES OUTPUT_NAME_DEBUG "BoggleTest_Debug")