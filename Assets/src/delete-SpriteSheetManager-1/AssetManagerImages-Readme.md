AssetManagerImages-Readme.md

---

## Image Asset Manager

This module manages loading .png image files
- each image is specified as a string (including path and image name)
- this module returns an integer id, that identifies the asset
- there is a linear array of image structs held by the manager
- the id is index into this array
- the manager is a global singleton generally

- So operations are like
-- int Image Id = getImageId("path/image.png")
-- GetImage(ImageId) //gets a reference to ImageStruct

- image structs have
-- xsize, ysize
- format (enum, RGBA default)
- etc

