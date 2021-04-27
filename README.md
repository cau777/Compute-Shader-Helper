# Compute-Shader-Helper
Simple class to speed up working with compute shaders in Unity

## How to Use
1. Create ComputeHelper object
´ComputeHelper helper = new ComputeHelper(myComputeShader, kernelIndex);´

2. Set properties in the ComputeShader object
´myComputeShader.SetInt("Count", 3);´

3. Create Buffers using the ComputeHelper object. There are 3 types of buffers:
* Write buffer: **writes** the contents of the array to the Buffer in the shader
´CreateWriteBuffer(string name, int size, int stride, Array reference)´
´helper.CreateWriteBuffer("Inputs", inputs.Length, sizeof(float), inputs);´

* Read buffer: **reads** the contents of the Buffer and stores them into the array after the shader is dispached
´CreateReadWriteBuffer(string name, int size, int stride, Array reference)´
´helper.CreateReadBuffer("Outputs", outputs.Length, sizeof(float), outputs);´

* Read/Write buffer: **writes** the contents of the array to the Buffer in the shader and **reads** the contents of the Buffer and stores them into the array after the shader is dispached
´CreateReadWriteBuffer(string name, int size, int stride, Array reference)´
´helper.CreateReadWriteBuffer("ThingToBeModified", thingsToBeModified.Length, sizeof(float), thingsToBeModified);´

4. Run the Shader, specifying the **total** thread count for x, y, z
´RunShader(int x, int y, int z)´
´helper.RunShader(500, 1, 1); // The shader will run 500 times´

After this command, the program retrieves data from the buffers, stores them into the arrays and releases all ComputeBuffers
