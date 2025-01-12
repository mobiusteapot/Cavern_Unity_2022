# URP ODS Status

The "Single Camera Rig" uses Unity's built-in but mostly deprecated 360 degree ODS shader.

You can read about the shader and see how Unity intended it to be used here:
[Original Unity Blogpost](http://web.archive.org/web/20190509004450/https://blogs.unity3d.com/2018/01/26/stereo-360-image-and-video-capture/)

# What is this useful for?

Apparently, the projection is entirely in world-to-clipspace stage of the shader, and is actually pretty straightforward. The goal is to write a custom render pass to apply this transformation to each object in the scene.

Originally, it wasn't known that this was just a shader that could be modified.

Swapping to our own usage, rather than calling the built in camera stereo properties would eliminate the "black box" nature of the current implementation of the single camera rig, and enable the single camera rig to support URP.

This would be a big step forward for maintability, and would maybe make the single camera rig actually viable in the long run.