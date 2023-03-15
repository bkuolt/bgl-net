using System.Windows;
using OpenTK.Mathematics;



namespace bgl
{    using Matrix4D = OpenTK.Mathematics.Matrix4;
    public record Camera
    {
      public Matrix4D ViewMatrix;
    }
}