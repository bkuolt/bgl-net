

namespace bgl {

using Matrix4D = OpenTK.Mathematics.Matrix4;

public class Scene {
   
    public record Camera {
        public Matrix4D ViewMatrix;
    }

    public class Node {
        public readonly Node Parent;
        public Node[] Children;
        public Matrix4D Transform;

        public Mesh[] Meshes;
        public Camera Camera;
 
        public Node(in Node parent) {
            this.Parent = parent;
        }



        public void Draw(in Matrix4D viewMatrix)
        {
            foreach (Mesh mesh in Meshes) {
                var worldMatrix = Parent.GetWorldTransform() * Transform;
                mesh.Draw(worldMatrix, viewMatrix);
            }
        }

        private Matrix4D GetWorldTransform() {
            return new Matrix4D();  // TODO
        }

    }

    public Node Root;
    public void Draw() {

        Matrix4D worldMatrix = new Matrix4D();

        var cameraNode = cameras[currentCamera];
        Matrix4D viewMatrix = cameraNode.Camera.ViewMatrix;

        foreach (Node node in GetAllNodes())
        {
            viewMatrix = viewMatrix * node.Parent.Transform;
            foreach (Mesh mesh in node.Meshes)
            {
                mesh.Draw(worldMatrix, viewMatrix);
            }
        }

    }

    public void SetCamera(uint index) {
        // TODO
    }

    private Node[] GetAllNodes() {
        return null;  // TODO
    }

    private Node[] cameras;
    private uint currentCamera; 
}

}