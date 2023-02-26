

namespace bgl {

public class Scene {
    public record Matrix4D {} /* TODO */

    public record Camera {
        Matrix4D ViewMatrix;
    }

    public class Node {
        public readonly Node Parent;
        public Node[] Children;

        public Camera Camera;
        public Mesh[] Meshes;
 
        public Node(in Node parent) {
            this.Parent = parent;
        }

        public Matrix4D GetWorldTransform() {
            return null; // TODO
        }
    }

    public Node Root;

    public void Draw() {
        // TODO
    }

    public void SetCamera(uint index) {
        // TODO
    }

    private System.Tuple<Matrix4D, Mesh> GetMeshesChildren() {
        return null;  // TODO
    }

    private Node[] cameras;
}

}