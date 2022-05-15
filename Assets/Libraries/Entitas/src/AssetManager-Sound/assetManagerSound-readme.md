AssetManagerSound-Readme.md

---

## Sound Asset Manager

Note: Could be renamed Audio Asset Manager
- use SoundFileManager?
- use AudioFileManager?

This module manages loading sound files
- each sound file is specified as a string (including path and filename)
- this module returns an integer id, that identifies the asset
- there is a linear array of image structs held by the manager
- the id is index into this array
- the manager is a global singleton generally

- So operations are like
-- int SoundId = getSoundId("path/image.png")
-- GetSoundFile(SoundId) //gets a reference to ImageStruct


