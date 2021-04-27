using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputeHelper
    {
        private readonly ComputeShader _shader;
        private readonly List<RWBuffer> _buffers = new List<RWBuffer>();

        private readonly int _kernel;
        private readonly uint _threadsX;
        private readonly uint _threadsY;
        private readonly uint _threadsZ;

        public ComputeHelper(ComputeShader shader, int kernel)
        {
            _shader = shader;
            _kernel = kernel;

            _shader.GetKernelThreadGroupSizes(_kernel, out _threadsX, out _threadsY, out _threadsZ);
        }

        public void RunShader(int x, int y, int z)
        {
            foreach (RWBuffer b in _buffers)
            {
                if (b.IsWrite)
                {
                    b.Buffer.SetData(b.Array);
                }
            }

            _shader.Dispatch(_kernel, Mathf.CeilToInt(x / (float) _threadsX), Mathf.CeilToInt(y / (float) _threadsY),
                Mathf.CeilToInt(z / (float) _threadsZ));

            foreach (RWBuffer b in _buffers)
            {
                if (b.IsRead)
                {
                    b.Buffer.GetData(b.Array);
                }
            }

            foreach (RWBuffer b in _buffers)
            {
                b.Buffer.Release();
            }
        }

        public void CreateReadBuffer(string name, int size, int stride, Array reference)
        {
            RWBuffer rWBuffer = new RWBuffer()
            {
                Array = reference,
                Buffer = new ComputeBuffer(size, stride),
                IsRead = true,
                IsWrite = false
            };
            _buffers.Add(rWBuffer);
            _shader.SetBuffer(_kernel, name, rWBuffer.Buffer);
        }

        public void CreateWriteBuffer(string name, int size, int stride, Array reference)
        {
            RWBuffer rWBuffer = new RWBuffer
            {
                Array = reference,
                Buffer = new ComputeBuffer(size, stride),
                IsRead = false,
                IsWrite = true
            };
            _buffers.Add(rWBuffer);
            _shader.SetBuffer(_kernel, name, rWBuffer.Buffer);
        }

        public void CreateReadWriteBuffer(string name, int size, int stride, Array reference)
        {
            RWBuffer rWBuffer = new RWBuffer
            {
                Array = reference,
                Buffer = new ComputeBuffer(size, stride),
                IsRead = true,
                IsWrite = true
            };
            _buffers.Add(rWBuffer);
            _shader.SetBuffer(_kernel, name, rWBuffer.Buffer);
        }

        private class RWBuffer
        {
            public ComputeBuffer Buffer;
            public Array Array;
            public bool IsRead;
            public bool IsWrite;
        }
    }