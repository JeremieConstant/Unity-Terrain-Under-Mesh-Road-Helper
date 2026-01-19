[![Demo Video](https://img.youtube.com/vi/Sh3kh6RbF1E/0.jpg)](https://youtu.be/Sh3kh6RbF1E)

# Unity Terrain Under Mesh Road Helper

Helper to raise terrain under a chosen mesh—e.g., for integrating roads into your terrain.

## Dependencies

- **Unity Version:** Tested with Unity 6.3 LTS
- **Packages Needed:** [Splines Package from Unity Registry](https://docs.unity3d.com/Packages/com.unity.splines@2.8/manual/index.html)

## Usage

1. **Create a terrain** in your scene.
2. **Create a spline** along your terrain.
3. **Add Spline Extrude Component** to the spline object to create a road.
    - Set Shape Extrude Type to "Road" (or any desired type).
    - Select your desired radius.
4. **Add Mesh Collider Component** to the spline object.  
   *Do NOT skip this step!*
5. **Add the `TerrainDeformer` script** to your terrain object.
6. **Assign your spline object** to the `Road Mesh Object` field in the script inspector.
7. *(Optional, pt. 1)* Increase spline radius by 10–20% for better results (you can revert this later).
8. **Open the context menu** and select **Project Mesh to Terrain**.  
   ⚠️ *Warning: This step is destructive and non-reversible!*
9. *(Optional, pt. 2)* Revert the radius back to its original value.
   ⚠️ *Warning: This step is destructive and non-reversible!*
9. *(Optional, pt. 2)* **Revert the radius** back to its original value.