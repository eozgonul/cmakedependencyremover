cmake_minimum_required (VERSION 3.5)
set(CMAKE_CONFIGURATION_TYPES Debug Release)
project (BoggleLibrary)

get_filename_component(ProjectDirectory ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

set(TestDirectory "${ProjectDirectory}/../boggle_test")

file(GLOB Sources "${ProjectDirectory}/source/*.cpp")
file(GLOB Headers "${ProjectDirectory}/include/*.h")

source_group(IncludeFiles FILES ${Headers})
source_group(SourceFiles FILES ${Sources})

add_library(BoggleLibrary SHARED ${Headers} ${Sources})
add_definitions(-DBOGGLE_DLL)

include_directories("${ProjectDirectory}/include")

#ADD_MSVC_PRECOMPILED_HEADER("pchbogglelibrary.h" "pchbogglelibrary.cpp" Sources)

set_target_properties(BoggleLibrary PROPERTIES RUNTIME_OUTPUT_DIRECTORY_DEBUG "${ProjectDirectory}/binary")
set_target_properties(BoggleLibrary PROPERTIES RUNTIME_OUTPUT_DIRECTORY_RELEASE "${ProjectDirectory}/binary")
set_target_properties(BoggleLibrary PROPERTIES ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${ProjectDirectory}/binary")
set_target_properties(BoggleLibrary PROPERTIES ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${ProjectDirectory}/binary")
set_target_properties(BoggleLibrary PROPERTIES OUTPUT_NAME_RELEASE "BoggleLibrary")
set_target_properties(BoggleLibrary PROPERTIES OUTPUT_NAME_DEBUG "BoggleLibrary_Debug")

add_custom_command(TARGET BoggleLibrary POST_BUILD
		COMMAND ${CMAKE_COMMAND} -E copy
				${ProjectDirectory}/binary/BoggleLibrary_Debug.dll
				${TestDirectory}/BoggleLibrary_Debug.dll)

add_custom_command(TARGET BoggleLibrary POST_BUILD
		COMMAND ${CMAKE_COMMAND} -E copy
				${ProjectDirectory}/binary/BoggleLibrary.dll
				${TestDirectory}/BoggleLibrary.dll)
