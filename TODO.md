TODO.md

Organizing the code:

## Problems

- unity does not seem to keep .csproj file updated when code is moved

## Top Level Todo

- organize repo so that code can be easily modified outside of Unity
- it is easy for unity to use external libraries
- but very difficult for external libraries to use unity

- may need to put game files into an assembly and move into asset folder, in a post build test

https://stackoverflow.com/questions/30877355/csproj-file-in-unity-changes-everytime-i-reload-the-project

## Unit Tests

Unit tests should be in root folder and outside of assets
- they take too long to run otherwise

## Assemblies

Need to specify binary assemblies to rename the .cxproj file
- all files in Assets/Libraries are seldom changed, therefore should be a binary assembly
-- single binary assembly or one binary assembly for each, then assemble into its own assembly?

## Entitas Cleanup

- in long term (not urgent), see if we can run from entitas source code, instead of .dlls
- should be in its own binary assembly file

## Rename BigGustave

Update:
- repo here: https://github.com/kk-digital/LibPng

Fork repo
- rename PngLib for Png Library Csharp
- or just LibPng
- do changes, update readme here

-- fix readme
- then use to replace BigGustave in Library folder