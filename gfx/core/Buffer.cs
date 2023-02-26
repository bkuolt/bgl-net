using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

namespace bgl
{
    public record BufferView
    {
        public OpenGL.BufferTarget Target;
        public Buffer Buffer;
        public int Offset;
        public uint Length;
    }

    public record Accessor
    {
        OpenGL.TypeEnum type;
        OpenGL.TypeEnum componentType;
        public uint Count;
        public BufferView BufferView;
        int byteOffset;
    }


    public class PrimitiveList {

    }


    public class VertexArray
    {
        public VertexArray(in Accessor[] acccesors)
        {
            // TODO: ceate VAO
            // TODO bind ibo 
            // TODO bind vbo
            GL.BindVertexArray(handle);

            for (uint i = 0; i < acccesors.Length; ++i)
            {
                SetAttribute(i, acccesors[i]);
            }
        }

        ~VertexArray()
        {

        }

        public void Bind()
        {
            // TODO
        }

        private void SetAttribute(in uint index, in Accessor accessor)
        {
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            // TODO
        }
        private int handle;
    }

    public class Buffer
    {
        private int handle;
        public readonly OpenGL.BufferTarget target;

        public Buffer(OpenGL.BufferTarget target, byte[] data, BufferUsageHint usageHint = BufferUsageHint.DynamicDraw)
        {
            this.handle = GL.GenBuffer();
            this.target = target;
            GL.BindBuffer(target, handle);
            GL.BufferData(target, data.Length, data, usageHint);
        }
        ~Buffer()
        {
            GL.DeleteBuffer(handle);
        }

        public void Bind() {
            // TODO
        }

        // TODO: conversion operator
    }
}