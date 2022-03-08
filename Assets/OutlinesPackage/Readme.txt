Oulines Package
============

Developed by Tomasz Pabis (c) 2022

The robot used to showcase: https://assetstore.unity.com/packages/3d/characters/robots/space-robot-kyle-4696

Some shader code used here under MIT: https://github.com/IronWarrior/UnityOutlineShader
UV calculations under MIT: https://gist.github.com/NedMakesGames/13bb699f01baf9817f206bdcaf5000f8

--------------------------------------------------------
How to use:
--------------------------------------------------------

Basic Outlines:
Use Render Objects(experimental) feature and add material for choosen layer. You can also add 
code from BasicOutline.shader to your own shader and use LightMode Tag.

Or just add second material to your mesh renderer directly.

'OUTLINE MODE': 
'NML'(use mesh normals or normals from uv channel or normals from custom texture to draw outlines)
'POS'(use vertex position to draw outliness)

'Baked Normals'(choose between: none, uvs baked in choosen uv channel with the BakeNormalsToUV tool, custom texture)
Tools/BakeNormalsToUV to bake uvs in uv channel.

Outlines changing distance over camera distance:
'Max thickness': the thickness the outlines will have when they reach 'Max distance Thickness'.
'Base thickness distance': the distance when the outlines will have thickness specified in 'Base Thickness'


Edge Detecion Outlines:
Use custom renderer feature called 'Edge Outlines Feature' and choose outlines layer mask, the feature should work on. This feature uses depth of the 
scene and mesh normals tocalculate edges.

Blur Outlines:
Use custom renderer feature called 'Blur Outlines Feature', and choose outlines layer mask, the feature should work on. This feature draws bolded objects.
You can also disable blur and have simple outlines around the objects.

'USE DEPTH MASK' is used to determine, should the feature draw objects behind other or not.
'ALPHA CUTOFF' can be used to cutout objects based on the texture alpha channel.

--------------------------------------------------------
Tools:
--------------------------------------------------------

To bake mesh smoothed normals to UV channel, use Tools/BakeMeshNormalsToUV window.